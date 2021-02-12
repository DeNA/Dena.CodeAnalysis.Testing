using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// A stub class for <see cref="DiagnosticAnalyzer" />.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is an exposed API")]
    public sealed class StubAnalyzer : DiagnosticAnalyzer
    {
        private readonly AnalyzerActions _actions;


        /// <summary>
        /// Instantiate a stub for <see cref="DiagnosticAnalyzer" /> with no actions.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is an exposed API")]
        public StubAnalyzer() => _actions = new AnalyzerActions();


        /// <summary>
        /// Instantiate a stub for <see cref="DiagnosticAnalyzer" /> with the specified actions.
        /// All actions in <paramref name="actions" /> will be registered at <see cref="Initialize" />.
        /// </summary>
        public StubAnalyzer(AnalyzerActions actions) => _actions = actions;


        /// <summary>
        /// Register all actions specified to the constructor.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics
            );

            _actions.RegisterAllTo(context);
        }


        /// <summary>
        /// Return only <see cref="StubDiagnosticDescriptor" />.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(StubDiagnosticDescriptor.ForTest);
    }
}