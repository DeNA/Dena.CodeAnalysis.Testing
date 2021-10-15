using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestClass]
    public class StubAnalyzerTests
    {
        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCodeBlockAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CodeBlockAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCodeBlockStartAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CodeBlockStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCompilationAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CompilationAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallCompilationStartAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    CompilationStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationBlockAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationBlockAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallOperationBlockStartAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    OperationBlockStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSemanticModelAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SemanticModelAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSymbolAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SymbolAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSymbolStartAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SymbolStartAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSyntaxNodeAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SyntaxNodeAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }


        [TestMethod]
        public async Task WhenGivenAnyCodes_ItShouldGetToCallSyntaxTreeAction()
        {
            var callCount = 0;
            var stubAnalyzer = new StubAnalyzer(
                new AnalyzerActions
                {
                    SyntaxTreeAction = _ => callCount++
                }
            );

            await DiagnosticAnalyzerRunner.Run(stubAnalyzer, ExampleCode.DiagnosticsFreeClassLibrary);

            MSTestAssert.AreNotEqual(0, callCount);
        }
    }
}