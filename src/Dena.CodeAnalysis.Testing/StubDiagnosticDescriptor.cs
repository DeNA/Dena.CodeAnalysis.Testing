using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;



namespace Dena.CodeAnalysis.Testing
{
    public static class StubDiagnosticDescriptor
    {
        [SuppressMessage("ReSharper", "RS2008")]
        public static DiagnosticDescriptor Test = new DiagnosticDescriptor(
            "TEST",
            "This is a diagnostics stub",
            "This is a diagnostics stub",
            "AnalyzerTest",
            DiagnosticSeverity.Info,
            true
        );
    }
}