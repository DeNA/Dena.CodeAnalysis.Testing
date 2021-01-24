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
                    CodeBlockAction = context => callCount++
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
                    CodeBlockStartAction = context => callCount++
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
                    CompilationAction = context => callCount++
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
                    CompilationStartAction = context => callCount++
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
                    OperationAction = context => callCount++
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
                    OperationBlockAction = context => callCount++
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
                    OperationBlockStartAction = context => callCount++
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
                    SemanticModelAction = context => callCount++
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
                    SymbolAction = context => callCount++
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
                    SymbolStartAction = context => callCount++
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
                    SyntaxNodeAction = context => callCount++
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
                    SyntaxTreeAction = context => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFree);

            Assert.AreNotEqual(0, callCount);
        }
    }
}