// (c) 2021 DeNA Co., Ltd.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Dena.CodeAnalysis.Testing.Tests")]

namespace Dena.CodeAnalysis.CSharp.Testing
{
    public static class TestDataParser
    {
        /// <summary>
        /// Create source and expected diagnostic from formatting embedded.
        /// </summary>
        /// <remarks>
        /// Separate *testData* into source and diagnostic.
        /// *testData* is the source enclosed by format where it is expected to be reported by the analyzer.
        /// Specify the part to be reported in the following format.
        /// The format is enclosed in { } and separated by |.
        /// <c>{|source|Identifier of the DiagnosticDescriptor (DDID)|DiagnosticMessage|}</c>/// 
        /// <c>e.g., {|new Stack()|CS1002|; expected|}</c>
        /// </remarks>
        /// <param name="testData">String containing format in source</param>
        /// <param name="path">Pathname of the source file</param>
        /// <returns>
        /// 1. Source extracted from *testData*
        /// 2. Returns a List of Diagnostic containing Location, DiagnosticMessage, and DDID
        /// </returns>
        public static (string source, List<Diagnostic> expectedDiagnostics) CreateSourceAndExpectedDiagnostic(
            string testData, string path = "/0/Test0.cs")
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var (target, ddid, msg) in ExtractMaker(testData))
            {
                var format = "{|" + target + "|" + ddid + "|" + msg + "|}";
                Location location = CreateLocation(testData, format, target.Length, path);
                testData = testData.Replace(format, target);
                var diagnosticDescriptor = new DiagnosticDescriptor(
                    id: ddid,
                    title: null!,
                    messageFormat: msg,
                    category: "",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true);
                var diagnostic = Diagnostic.Create(
                    diagnosticDescriptor,
                    location);
                diagnostics.Add(diagnostic);
            }

            return (testData, diagnostics);
        }

        internal static IEnumerable<(string target, string ddid, string msg)> ExtractMaker(
            string testData)
        {
            const string Pattern = "\\{\\|([^\\|]*)\\|([^\\|]*)\\|([^\\|]*)\\|\\}";
            // TODO: Write Patterns more simply using repeat formatting.
            var match = Regex.Match(testData, Pattern);
            while (match.Success)
            {
                yield return (match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                match = match.NextMatch();
            }
        }

        internal static Location CreateLocation(string sourceBeforeExtractFormat, string format, int targetLength, string path)
        {
            var start = CreateLinePositionStart(sourceBeforeExtractFormat, format);
            var end = new LinePosition(start.Line, start.Character + targetLength);
            Location location = Location.Create(
                path,
                new TextSpan(
                    sourceBeforeExtractFormat.IndexOf(format, StringComparison.Ordinal),
                    targetLength),
                new LinePositionSpan(
                    start,
                    end)
            );
            return location;
        }

        internal static LinePosition CreateLinePositionStart(string sourceBeforeExtractFormat, string format)
        {
            // Currently, Line returns 0 as origin and character returns 1 as origin
            var (newLineCount, newLinePosition, characterCount) = (0, -1, 0);
            var formatIndex = sourceBeforeExtractFormat.IndexOf(format, StringComparison.Ordinal);
            // add a newline at the end, as without a newline after the format it would be an infinite loop
            sourceBeforeExtractFormat += "\n";

            while (newLinePosition < formatIndex)
            {
                characterCount = formatIndex - (newLinePosition + 1);
                newLinePosition =
                    sourceBeforeExtractFormat.IndexOf("\n", newLinePosition + 1, StringComparison.Ordinal);
                newLineCount++;
            }

            return new LinePosition(newLineCount - 1, characterCount);
        }
    }
}