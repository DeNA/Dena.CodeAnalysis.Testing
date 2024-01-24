using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestClass]
    public class SpyAnalyzerTests
    {
        [TestMethod]
        public async Task WhenGivenAnyCodes_RecordAllActionHistory()
        {
            var spy = new SpyAnalyzer();
            var builder = new StringBuilder();
            var failed = false;

            await DiagnosticAnalyzerRunner.Run(spy, codes: ExampleCode.DiagnosticsFreeClassLibrary);

            if (0 == spy.CodeBlockActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.CodeBlockActionHistory));
            }

            if (0 == spy.CodeBlockStartActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.CodeBlockStartActionHistory));
            }

            if (0 == spy.CompilationActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.CompilationActionHistory));
            }

            if (0 == spy.CompilationStartActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.CompilationStartActionHistory));
            }

            if (0 == spy.OperationActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.OperationActionHistory));
            }

            if (0 == spy.OperationBlockActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.OperationBlockActionHistory));
            }

            if (0 == spy.OperationBlockStartActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.OperationBlockStartActionHistory));
            }

            if (0 == spy.SemanticModelActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.SemanticModelActionHistory));
            }

            if (0 == spy.SymbolActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.SymbolActionHistory));
            }

            if (0 == spy.SymbolStartActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.SymbolStartActionHistory));
            }

            if (0 == spy.SyntaxNodeActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.SyntaxNodeActionHistory));
            }

            if (0 == spy.SyntaxTreeActionHistory.Count)
            {
                failed = true;
                builder.AppendLine(nameof(spy.SyntaxTreeActionHistory));
            }

            MSTestAssert.IsFalse(failed, builder.ToString());
        }
    }
}