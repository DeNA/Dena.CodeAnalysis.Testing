using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// A null object class for <see cref="DiagnosticAnalyzer" />.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is an exposed API")]
    public sealed class NullAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="context">The analysis context but it will be not used.</param>
        [SuppressMessage("ReSharper", "RS1025")]
        [SuppressMessage("ReSharper", "RS1026")]
        public override void Initialize(AnalysisContext context)
        {
            // Do nothing.
        }


        /// <summary>
        /// Returns no <see cref="DiagnosticDescriptor" />.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray<DiagnosticDescriptor>.Empty;
    }
}