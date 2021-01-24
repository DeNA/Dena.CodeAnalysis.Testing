using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    [TestClass]
    public class AnalyzerRunnerTests
    {
        [TestMethod]
        public async Task TestNoCodeException()
        {
            var spy = new SpyAnalyzer();

            await Assert.ThrowsExceptionAsync<DiagnosticAnalyzerRunner.AtLeastOneCodeMustBeRequired>(
                async () => { await DiagnosticAnalyzerRunner.Run(spy); }
            );
        }


        [TestMethod]
        public async Task TestSuccessfullyCompilable()
        {
            var spy = new SpyAnalyzer();

            var diagnostics = await DiagnosticAnalyzerRunner.Run(spy, ExampleCode.SuccessfullyCompilable);

            Assert.IsTrue(spy.IsInitialized);
            Assert.AreEqual(0, diagnostics.Length);
        }


        [TestMethod]
        public async Task TestContainingSyntaxError()
        {
            var spy = new SpyAnalyzer();

            var diagnostics = await DiagnosticAnalyzerRunner.Run(spy, ExampleCode.ContainingSyntaxError);

            Assert.IsTrue(spy.IsInitialized);
            Assert.AreNotEqual(0, diagnostics.Length);
        }
    }
}