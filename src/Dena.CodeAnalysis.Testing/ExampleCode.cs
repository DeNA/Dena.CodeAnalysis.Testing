using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// Example codes for testing <see cref="DiagnosticAnalyzer"/>.
    /// </summary>
    public static class ExampleCode
    {
        /// <summary>
        /// An example code that can be compiled successfully.
        /// </summary>
        public static string SuccessfullyCompilable = @"
public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}
";

        /// <summary>
        /// An example code that contains a syntax error.
        /// </summary>
        public static string ContainingSyntaxError = SuccessfullyCompilable + "ERROR";
    }
}