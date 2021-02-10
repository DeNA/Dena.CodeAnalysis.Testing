using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// A stub collections for <see cref="DiagnosticDescriptor" />.
    /// </summary>
    public static class StubDiagnosticDescriptor
    {
        /// <summary>
        /// A stub <see cref="DiagnosticDescriptor" /> to test <see cref="DiagnosticAnalyzer" />.
        /// </summary>
        [SuppressMessage(
            "ReSharper",
            "RS2008",
            Justification = "This is for only test doubles for analyzers. So release tracking is not needed"
        )]
        public static readonly DiagnosticDescriptor ForTest = new(
            "TEST",
            "This is a diagnostics stub",
            "This is a diagnostics stub",
            "AnalyzerTest",
            DiagnosticSeverity.Info,
            true
        );
    }
}