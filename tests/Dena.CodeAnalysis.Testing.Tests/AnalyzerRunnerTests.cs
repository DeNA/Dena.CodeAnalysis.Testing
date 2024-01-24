using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestClass]
    public class AnalyzerRunnerTests
    {
        [TestMethod]
        public async Task WhenGivenNoCodes_ItShouldThrowAnException()
        {
            var anyAnalyzer = new NullAnalyzer();

            await MSTestAssert.ThrowsExceptionAsync<DiagnosticAnalyzerRunner.AtLeastOneCodeMustBeRequired>(
                async () => { await DiagnosticAnalyzerRunner.Run(anyAnalyzer); }
            );
        }


        [TestMethod]
        public async Task WhenGivenDiagnosticCleanCode_ItShouldReturnNoDiagnostics()
        {
            var anyAnalyzer = new NullAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(
                anyAnalyzer,
                codes: ExampleCode.DiagnosticsFreeClassLibrary
            );

            MSTestAssert.AreEqual(0, diagnostics.Length, DiagnosticsFormatter.Format(diagnostics));
        }

        [TestMethod]
        public async Task WhenGivenUniTaskImport_ItShouldReturnNoDiagnostics()
        {
            var anyAnalyzer = new NullAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(
                anyAnalyzer,
                new[] { typeof(UniTask) },
                ExampleCode.UniTaskImport
            );

            MSTestAssert.AreEqual(1, diagnostics.Length, DiagnosticsFormatter.Format(diagnostics));
        }

        [TestMethod]
        public async Task WhenGivenContainingASyntaxError_ItShouldReturnSeveralDiagnostics()
        {
            var anyAnalyzer = new NullAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(
                anyAnalyzer,
                codes: ExampleCode.ContainingSyntaxError
            );

            MSTestAssert.AreNotEqual(0, diagnostics.Length);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldCallAnalyzerInitialize()
        {
            var spyAnalyzer = new SpyAnalyzer();

            await DiagnosticAnalyzerRunner.Run(spyAnalyzer, codes: ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.IsTrue(spyAnalyzer.IsInitialized);
        }
    }
}