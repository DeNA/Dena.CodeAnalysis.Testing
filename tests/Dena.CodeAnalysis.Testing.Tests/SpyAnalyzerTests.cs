using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    [TestClass]
    public class SpyAnalyzerTests
    {
        [TestMethod]
        public async Task WhenGivenAnyCodes_RecordAllActionHistory()
        {
            var spy = new SpyAnalyzer();

            await DiagnosticAnalyzerRunner.Run(spy, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, spy.CodeBlockActionHistory.Count);
            Assert.AreNotEqual(0, spy.CodeBlockStartActionHistory.Count);
            Assert.AreNotEqual(0, spy.CompilationActionHistory.Count);
            Assert.AreNotEqual(0, spy.CompilationStartActionHistory.Count);
            Assert.AreNotEqual(0, spy.OperationActionHistory.Count);
            Assert.AreNotEqual(0, spy.OperationBlockActionHistory.Count);
            Assert.AreNotEqual(0, spy.OperationBlockStartActionHistory.Count);
            Assert.AreNotEqual(0, spy.SemanticModelActionHistory.Count);
            Assert.AreNotEqual(0, spy.SymbolActionHistory.Count);
            Assert.AreNotEqual(0, spy.SymbolStartActionHistory.Count);
            Assert.AreNotEqual(0, spy.SyntaxNodeActionHistory.Count);
            Assert.AreNotEqual(0, spy.SyntaxTreeActionHistory.Count);
        }
    }
}