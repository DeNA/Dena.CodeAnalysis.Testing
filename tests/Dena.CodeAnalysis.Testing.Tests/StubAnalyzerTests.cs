using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    [TestClass]
    public class StubAnalyzerTests
    {
        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCodeBlockAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CodeBlockAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCodeBlockStartAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CodeBlockStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCompilationAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CompilationAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCompilationStartAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CompilationStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationBlockAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationBlockAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationBlockStartAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationBlockStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSemanticModelAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SemanticModelAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSymbolAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SymbolAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSymbolStartAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SymbolStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSyntaxNodeAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SyntaxNodeAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSyntaxTreeAction()
        {
            int callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SyntaxTreeAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }
    }
}