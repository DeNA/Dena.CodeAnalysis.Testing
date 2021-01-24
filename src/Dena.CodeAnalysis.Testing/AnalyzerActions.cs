using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// Actions to register to <see cref="AnalysisContext"/>.
    /// </summary>
    public sealed class AnalyzerActions
    {
        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterCodeBlockAction" />
        /// </summary>
        public Action<CodeBlockAnalysisContext> CodeBlockAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterCodeBlockStartAction{SyntaxKind}" />
        /// </summary>
        public Action<CodeBlockStartAnalysisContext<SyntaxKind>> CodeBlockStartAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterCompilationAction" />
        /// </summary>
        public Action<CompilationAnalysisContext> CompilationAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterCompilationStartAction" />
        /// </summary>
        public Action<CompilationStartAnalysisContext> CompilationStartAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterOperationAction(Action{OperationAnalysisContext}, OperationKind[])" />
        /// </summary>
        public Action<OperationAnalysisContext> OperationAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterOperationBlockAction" />
        /// </summary>
        public Action<OperationBlockAnalysisContext> OperationBlockAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterOperationBlockStartAction"/>
        /// </summary>
        public Action<OperationBlockStartAnalysisContext> OperationBlockStartAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterSemanticModelAction"/>
        /// </summary>
        public Action<SemanticModelAnalysisContext> SemanticModelAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterSymbolAction(Action{SymbolAnalysisContext}, SymbolKind[])" />
        /// </summary>
        public Action<SymbolAnalysisContext> SymbolAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterSymbolStartAction(Action{SymbolStartAnalysisContext}, SymbolKind)" />
        /// </summary>
        public Action<SymbolStartAnalysisContext> SymbolStartAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterSyntaxNodeAction{SyntaxKind}(Action{SyntaxNodeAnalysisContext}, SyntaxKind[])" />
        /// </summary>
        public Action<SyntaxNodeAnalysisContext> SyntaxNodeAction = context => { };

        /// <summary>
        /// The action for <see cref="AnalysisContext.RegisterSyntaxTreeAction"/>
        /// </summary>
        public Action<SyntaxTreeAnalysisContext> SyntaxTreeAction = context => { };


        /// <summary>
        /// Compose two actions.
        /// </summary>
        /// <param name="a">The first action will be executed before <paramref name="b" />.</param>
        /// <param name="b">The second action will be executed after <paramref name="a" />.</param>
        /// <typeparam name="T">The type of the argument of <paramref name="a" /> and <paramref name="b" />.</typeparam>
        /// <returns>Composed actions.</returns>
        public static Action<T> ComposeAction<T>(Action<T> a, Action<T> b) =>
            x =>
            {
                a(x);
                b(x);
            };


        /// <summary>
        /// Compose two <see cref="AnalyzerActions" />.
        /// </summary>
        /// <param name="a">The first actions will be executed before <paramref name="b" />.</param>
        /// <param name="b">The second actions will be executed after <paramref name="a" />.</param>
        /// <returns>Composed actions.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")] // WHY: This is an exposed API.
        public static AnalyzerActions Compose(AnalyzerActions a, AnalyzerActions b)
        {
            var result = new AnalyzerActions();
            result.CodeBlockAction = ComposeAction(a.CodeBlockAction, b.CodeBlockAction);
            result.CodeBlockStartAction = ComposeAction(a.CodeBlockStartAction, b.CodeBlockStartAction);
            result.CompilationAction = ComposeAction(a.CompilationAction, b.CompilationAction);
            result.CompilationStartAction = ComposeAction(a.CompilationStartAction, b.CompilationStartAction);
            result.OperationAction = ComposeAction(a.OperationAction, b.OperationAction);
            result.OperationBlockAction = ComposeAction(a.OperationBlockAction, b.OperationBlockAction);
            result.OperationBlockStartAction = ComposeAction(a.OperationBlockStartAction, b.OperationBlockStartAction);
            result.SemanticModelAction = ComposeAction(a.SemanticModelAction, b.SemanticModelAction);
            result.SymbolAction = ComposeAction(a.SymbolAction, b.SymbolAction);
            result.SymbolStartAction = ComposeAction(a.SymbolStartAction, b.SymbolStartAction);
            result.SyntaxNodeAction = ComposeAction(a.SyntaxNodeAction, b.SyntaxNodeAction);
            result.SyntaxTreeAction = ComposeAction(a.SyntaxTreeAction, b.SyntaxTreeAction);
            return result;
        }


        /// <summary>
        /// Register all actions to the specified <see cref="AnalysisContext" />.
        /// </summary>
        /// <param name="context">The context that the actions register to.</param>
        public void RegisterAllTo(AnalysisContext context)
        {
            context.RegisterCodeBlockAction(CodeBlockAction);
            context.RegisterCodeBlockStartAction(CodeBlockStartAction);
            context.RegisterCompilationAction(CompilationAction);
            context.RegisterCompilationStartAction(CompilationStartAction);
            context.RegisterOperationAction(
                OperationAction,
                (OperationKind[]) Enum.GetValues(typeof(OperationKind))
            );
            context.RegisterOperationBlockAction(OperationBlockAction);
            context.RegisterOperationBlockAction(OperationBlockAction);
            context.RegisterOperationBlockStartAction(OperationBlockStartAction);
            context.RegisterSemanticModelAction(SemanticModelAction);
            context.RegisterSymbolAction(SymbolAction, (SymbolKind[]) Enum.GetValues(typeof(SymbolKind)));

            foreach (SymbolKind symbolKind in Enum.GetValues(typeof(SymbolKind)))
            {
                context.RegisterSymbolStartAction(SymbolStartAction, symbolKind);
            }

            context.RegisterSyntaxNodeAction(SyntaxNodeAction, (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind)));
            context.RegisterSyntaxTreeAction(SyntaxTreeAction);
        }
    }
}