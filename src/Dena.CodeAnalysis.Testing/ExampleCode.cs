namespace Dena.CodeAnalysis.Testing
{
    public static class ExampleCode
    {
        public static string SuccessfullyCompilable = @"
public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}
";

        public static string ContainingSyntaxError = SuccessfullyCompilable + "ERROR";
    }
}