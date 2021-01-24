using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// A spy <see cref="DiagnosticAnalyzer" /> that record whether <see cref="Initialize" /> has been called.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SpyAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Whether <see cref="Initialize" /> has been called or not.
        /// </summary>
        public bool IsInitialized;

        private AnalyzerActions _actions;


        /// <summary>
        /// Instantiate a new SpyAnalyzer.
        /// </summary>
        public SpyAnalyzer() => _actions = CreateSpyActions(this);


        /// <summary>
        /// Record that the method has been called to <see cref="IsInitialized" />.
        /// </summary>
        [SuppressMessage("ReSharper", "RS1026")]
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
            IsInitialized = true;
            _actions.RegisterAllTo(context);
        }


        /// <summary>
        /// Returns only <see cref="StubDiagnosticDescriptor.Test"/>.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(StubDiagnosticDescriptor.Test);

        public readonly IList<SyntaxNode> CodeBlockActionHistory = new List<SyntaxNode>();
        public readonly IList<SyntaxNode> CodeBlockStartActionHistory = new List<SyntaxNode>();

        [SuppressMessage("ReSharper", "RS1008")]
        public readonly IList<Compilation> CompilationActionHistory = new List<Compilation>();

        [SuppressMessage("ReSharper", "RS1008")]
        public readonly IList<Compilation> CompilationStartActionHistory = new List<Compilation>();

        public readonly IList<IOperation> OperationActionHistory = new List<IOperation>();

        public readonly IList<ImmutableArray<IOperation>> OperationBlockActionHistory =
            new List<ImmutableArray<IOperation>>();

        public readonly IList<ImmutableArray<IOperation>> OperationBlockStartActionHistory =
            new List<ImmutableArray<IOperation>>();

        public readonly IList<SemanticModel> SemanticModelActionHistory = new List<SemanticModel>();
        public readonly IList<ISymbol> SymbolActionHistory = new List<ISymbol>();
        public readonly IList<ISymbol> SymbolStartActionHistory = new List<ISymbol>();
        public readonly IList<SyntaxNode> SyntaxNodeActionHistory = new List<SyntaxNode>();
        public readonly IList<SyntaxTree> SyntaxTreeActionHistory = new List<SyntaxTree>();


        /// <summary>
        /// Create an <see cref="AnalyzerActions" /> to record all events.
        /// </summary>
        /// <returns><see cref="AnalyzerActions" /> to record all events.</returns>
        public static AnalyzerActions CreateSpyActions(SpyAnalyzer spy)
        {
            var actions = new AnalyzerActions();
            actions.CodeBlockAction = context => spy.CodeBlockActionHistory.Add(context.CodeBlock);
            actions.CodeBlockStartAction = context => spy.CodeBlockStartActionHistory.Add(context.CodeBlock);
            actions.CompilationAction = context => spy.CompilationActionHistory.Add(context.Compilation);
            actions.CompilationStartAction = context => spy.CompilationStartActionHistory.Add(context.Compilation);
            actions.OperationAction = context => spy.OperationActionHistory.Add(context.Operation);
            actions.OperationBlockAction = context => spy.OperationBlockActionHistory.Add(context.OperationBlocks);
            actions.OperationBlockStartAction =
                context => spy.OperationBlockStartActionHistory.Add(context.OperationBlocks);
            actions.SemanticModelAction = context => spy.SemanticModelActionHistory.Add(context.SemanticModel);
            actions.SymbolAction = context => spy.SymbolActionHistory.Add(context.Symbol);
            actions.SymbolStartAction = context => spy.SymbolStartActionHistory.Add(context.Symbol);
            actions.SyntaxNodeAction = context => spy.SyntaxNodeActionHistory.Add(context.Node);
            actions.SyntaxTreeAction = context => spy.SyntaxTreeActionHistory.Add(context.Tree);
            return actions;
        }
    }
}