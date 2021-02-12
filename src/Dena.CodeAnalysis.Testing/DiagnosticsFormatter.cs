using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// A formatter to help debugging diagnostics.
    /// </summary>
    public static class DiagnosticsFormatter
    {
        /// <summary>
        /// Helper method to format a <see cref="Diagnostic"/> into an easily readable string.
        /// </summary>
        /// <param name="path">The default file path for diagnostics.</param>
        /// <param name="diagnostics">A collection of <see cref="Diagnostic"/>s to be formatted.</param>
        /// <returns>The <paramref name="diagnostics"/> formatted as a string.</returns>
        public static string Format(IEnumerable<Diagnostic> diagnostics, string path = "/path/to/file.cs") =>
            FormatDiagnostics(path, diagnostics.ToArray());


        /// <inheritdoc cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}.FormatDiagnostics(ImmutableArray{DiagnosticAnalyzer}, string, Diagnostic[])"/>
        private static string FormatDiagnostics(
            string defaultFilePath,
            params Diagnostic[] diagnostics
        )
        {
            var builder = new StringBuilder();
            foreach (var diagnostic in diagnostics)
            {
                var location = diagnostic.Location;

                builder.Append("// ").AppendLine(diagnostic.ToString());

                builder.Append(
                    diagnostic.Severity switch
                    {
                        DiagnosticSeverity.Error =>
                            $"{nameof(DiagnosticResult)}.{nameof(DiagnosticResult.CompilerError)}(\"{diagnostic.Id}\")",
                        DiagnosticSeverity.Warning =>
                            $"{nameof(DiagnosticResult)}.{nameof(DiagnosticResult.CompilerWarning)}(\"{diagnostic.Id}\")",
                        var severity =>
                            $"new {nameof(DiagnosticResult)}(\"{diagnostic.Id}\", {nameof(DiagnosticSeverity)}.{severity})"
                    }
                );

                if (location == Location.None)
                {
                    // No additional location data needed
                }
                else
                {
                    AppendLocation(diagnostic.Location);
                    foreach (var additionalLocation in diagnostic.AdditionalLocations)
                        AppendLocation(additionalLocation);
                }

                var arguments = GetArguments(diagnostic);
                if (arguments.Count > 0)
                {
                    builder.Append($".{nameof(DiagnosticResult.WithArguments)}(");
                    builder.Append(string.Join(", ", arguments.Select(a => $"\"{a}\"")));
                    builder.Append(')');
                }

                builder.AppendLine(",");
            }

            return builder.ToString();

            // Local functions
            void AppendLocation(Location location)
            {
                var lineSpan = location.GetLineSpan();
                var pathString = location.IsInSource && lineSpan.Path == defaultFilePath
                    ? string.Empty
                    : $"\"{lineSpan.Path}\", ";
                var linePosition = lineSpan.StartLinePosition;
                var endLinePosition = lineSpan.EndLinePosition;
                builder.Append(
                    $".WithSpan({pathString}{linePosition.Line + 1}, {linePosition.Character + 1}, {endLinePosition.Line + 1}, {endLinePosition.Character + 1})"
                );
            }
        }


        private static IReadOnlyList<object> GetArguments(Diagnostic diagnostic) =>
            (IReadOnlyList<object>) diagnostic.GetType().GetProperty(
                "Arguments",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            )?.GetValue(diagnostic)
            ?? Array.Empty<object>();
    }
}