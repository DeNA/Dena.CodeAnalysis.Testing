using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// Example codes for testing <see cref="DiagnosticAnalyzer"/>.
    /// </summary>
    public static class ExampleCode
    {
        /// <summary>
        /// An example code that can be compiled successfully with no Diagnostics.
        /// </summary>
        public const string DiagnosticsFreeClassLibrary = @"
internal static class Foo
{
    internal static void Bar()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}
";

        /// <summary>
        /// An example code that contains a syntax error.
        /// </summary>
        public const string ContainingSyntaxError = DiagnosticsFreeClassLibrary + "ERROR";
    }
}