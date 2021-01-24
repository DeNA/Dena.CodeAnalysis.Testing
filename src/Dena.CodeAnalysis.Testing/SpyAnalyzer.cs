using System;
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

        private readonly AnalyzerActions _actions;


        /// <summary>
        /// Instantiate a new SpyAnalyzer.
        /// </summary>
        public SpyAnalyzer() => _actions = CreateSpyActions(this);


        /// <summary>
        /// Record that the method has been called to <see cref="IsInitialized" />.
        /// </summary>
        [SuppressMessage("ReSharper", "RS1026")] // WHY: Avoid exclusive access control for call histories.
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

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterCodeBlockAction" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<SyntaxNode> CodeBlockActionHistory = new List<SyntaxNode>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterCodeBlockStartAction{SyntaxKind}" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<SyntaxNode> CodeBlockStartActionHistory = new List<SyntaxNode>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterCompilationAction" />
        /// </summary>
        [SuppressMessage("ReSharper", "RS1008")] // WHY: Especially for testing, this should be allowed.
        // WHY: This is an exposed API.
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
        public readonly IList<Compilation> CompilationActionHistory = new List<Compilation>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterCompilationStartAction" />
        /// </summary>
        [SuppressMessage("ReSharper", "RS1008")] // WHY: Especially for testing, this should be allowed.
        // WHY: This is an exposed API.
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
        public readonly IList<Compilation> CompilationStartActionHistory = new List<Compilation>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterOperationAction(Action{OperationAnalysisContext}, OperationKind[])" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<IOperation> OperationActionHistory = new List<IOperation>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterOperationBlockAction" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<ImmutableArray<IOperation>> OperationBlockActionHistory =
            new List<ImmutableArray<IOperation>>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterOperationBlockStartAction"/>
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<ImmutableArray<IOperation>> OperationBlockStartActionHistory =
            new List<ImmutableArray<IOperation>>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterSemanticModelAction"/>
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<SemanticModel> SemanticModelActionHistory = new List<SemanticModel>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterSymbolAction(Action{SymbolAnalysisContext}, SymbolKind[])" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<ISymbol> SymbolActionHistory = new List<ISymbol>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterSymbolStartAction(Action{SymbolStartAnalysisContext}, SymbolKind)" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<ISymbol> SymbolStartActionHistory = new List<ISymbol>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterSyntaxNodeAction{SyntaxKind}(Action{SyntaxNodeAnalysisContext}, SyntaxKind[])" />
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<SyntaxNode> SyntaxNodeActionHistory = new List<SyntaxNode>();

        /// <summary>
        /// History of calls of the action registered via <see cref="AnalysisContext.RegisterSyntaxTreeAction"/>
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")] // WHY: This is an exposed API.
        public readonly IList<SyntaxTree> SyntaxTreeActionHistory = new List<SyntaxTree>();


        /// <summary>
        /// Create an <see cref="AnalyzerActions" /> to record all events.
        /// </summary>
        /// <returns><see cref="AnalyzerActions" /> to record all events.</returns>
        public static AnalyzerActions CreateSpyActions(SpyAnalyzer spy) =>
            new AnalyzerActions
            {
                CodeBlockAction = context => spy.CodeBlockActionHistory.Add(context.CodeBlock),
                CodeBlockStartAction = context => spy.CodeBlockStartActionHistory.Add(context.CodeBlock),
                CompilationAction = context => spy.CompilationActionHistory.Add(context.Compilation),
                CompilationStartAction = context => spy.CompilationStartActionHistory.Add(context.Compilation),
                OperationAction = context => spy.OperationActionHistory.Add(context.Operation),
                OperationBlockAction = context => spy.OperationBlockActionHistory.Add(context.OperationBlocks),
                OperationBlockStartAction =
                    context => spy.OperationBlockStartActionHistory.Add(context.OperationBlocks),
                SemanticModelAction = context => spy.SemanticModelActionHistory.Add(context.SemanticModel),
                SymbolAction = context => spy.SymbolActionHistory.Add(context.Symbol),
                SymbolStartAction = context => spy.SymbolStartActionHistory.Add(context.Symbol),
                SyntaxNodeAction = context => spy.SyntaxNodeActionHistory.Add(context.Node),
                SyntaxTreeAction = context => spy.SyntaxTreeActionHistory.Add(context.Tree)
            };
    }
}