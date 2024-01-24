using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestClass]
    public class DiagnosticsFormatterTests
    {
        [TestMethod]
        public async Task Format()
        {
            var diagnostics = await DiagnosticAnalyzerRunner.Run(
                new NullAnalyzer(),
                codes: ExampleCode.ContainingSyntaxError
            );

            var actual = DiagnosticsFormatter.Format(diagnostics);
            var expected =
                @"// /0/Test0.cs(9,1): error CS0116: A namespace cannot directly contain members such as fields or methods
DiagnosticResult.CompilerError(""CS0116"").WithSpan(""/0/Test0.cs"", 9, 1, 9, 6),
";
            MSTestAssert.AreEqual(expected, actual);
        }
    }
}