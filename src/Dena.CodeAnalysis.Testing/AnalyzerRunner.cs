using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.Composition;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// A runner for <see cref="DiagnosticAnalyzer" />.
    /// The purpose of the runner is providing another helpers instead of <see cref="AnalyzerVerifier{T1, T2, T3}.VerifyAnalyzerAsync" />.
    /// The AnalyzerVerifier has several problems:
    ///
    ///   1. Using AnalyzerVerifier, it is hard to instantiate analyzer with custom arguments (it will be needed
    ///      if your analyzer is composed by several small analyzer-like components.)
    ///   2. AnalyzerVerifier do diagnostics assertion, but it should be optional because analyzer-like small components
    ///      may not need it.
    /// </summary>
    public static class DiagnosticAnalyzerRunner
    {
        /// <summary>
        /// Run the specified <see cref="DiagnosticAnalyzer" />.
        /// </summary>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer" /> to run.</param>
        /// <param name="codes">The target code that the <paramref name="analyzer" /> analyze.</param>
        /// <returns>ImmutableArray contains all reported <see cref="Diagnostic" />.</returns>
        /// <throws>Throws <c cref="AtLeastOneCodeMustBeRequired" /> if <paramref name="codes" /> are empty.</throws>
        public static async Task<ImmutableArray<Diagnostic>> Run(
            DiagnosticAnalyzer analyzer,
            params string[] codes
        ) =>
            await Run(
                analyzer,
                CancellationToken.None,
                ParseOptionsForLanguageVersionsDefault(),
                CompilationOptionsForDynamicClassLibrary(),
                codes
            );


        /// <summary>
        /// Run the specified <see cref="DiagnosticAnalyzer" />.
        /// </summary>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer" /> to run.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> that the task will observe.</param>
        /// <param name="parseOptions">The <see cref="ParseOptions" />. </param>
        /// <param name="compilationOptions">The <see cref="CompilationOptions" />. Use <see cref="OutputKind.ConsoleApplication"/> if you want to analyze codes including <c>Main()</c> (default: <see cref="OutputKind.DynamicallyLinkedLibrary" />).</param>
        /// <param name="codes">The target code that the <paramref name="analyzer" /> analyze. Use <see cref="LanguageVersion" /> if you want to specify C# version (default: <see cref="LanguageVersion.Default" />)."</param>
        /// <returns>ImmutableArray contains all reported <see cref="Diagnostic" />.</returns>
        /// <throws>Throws <c cref="AtLeastOneCodeMustBeRequired" /> if <paramref name="codes" /> are empty.</throws>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is an exposed API")]
        public static async Task<ImmutableArray<Diagnostic>> Run(
            DiagnosticAnalyzer analyzer,
            CancellationToken cancellationToken,
            ParseOptions parseOptions,
            CompilationOptions compilationOptions,
            params string[] codes
        )
        {
            if (!codes.Any()) throw new AtLeastOneCodeMustBeRequired();

            using var workspace = CreateWorkspace();
            var projectId = ProjectId.CreateNewId();
            var solution = workspace
                .CurrentSolution
                .AddProject(projectId, DefaultTestProjectName, DefaultAssemblyName, Language);

            foreach (var code in codes)
            {
                var documentId = DocumentId.CreateNewId(projectId);
                solution = solution.AddDocument(documentId, DefaultFilePath, code, filePath: DefaultFilePath);
            }

            var noMetadataReferencedProject = solution.Projects.First();

            // NOTE: Make .NET standard libraries visible to the specified codes to analyze.
            var metadataReferences =
                await ReferenceAssemblies.Default.ResolveAsync(Language, CancellationToken.None);

            var project = noMetadataReferencedProject
                .AddMetadataReferences(metadataReferences)
                .WithParseOptions(parseOptions)
                .WithCompilationOptions(compilationOptions);

            var compilation = await project.GetCompilationAsync(cancellationToken);
            if (compilation is null) throw new NoCompilationError();

            var withAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer));
            return await withAnalyzers.GetAllDiagnosticsAsync(cancellationToken);
        }


        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest{DiagnosticAnalyzer, IVerifier}.CreateCompilationOptions" />
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is an exposed API.")]
        public static CompilationOptions CompilationOptionsForDynamicClassLibrary() =>
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);


        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest{DiagnosticAnalyzer, IVerifier}.CreateParseOptions" />
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "This is an exposed API.")]
        public static ParseOptions ParseOptionsForLanguageVersionsDefault() =>
            new CSharpParseOptions(DefaultLanguageVersion, DocumentationMode.Diagnose);


        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}.DefaultFilePathPrefix" />
        /// </summary>
        private const string DefaultFilePathPrefix = "/0/Test";

        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}.DefaultTestProjectName" />
        /// </summary>
        private const string DefaultTestProjectName = "TestProject";

        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}.DefaultTestProjectName" />
        /// </summary>
        private static readonly string DefaultFilePath = $"{DefaultFilePathPrefix}{0}.{DefaultFileExt}";

        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest{DiagnosticAnalyzer, IVerifier}.DefaultFileExt" />
        /// </summary>
        private const string DefaultFileExt = "cs";

        /// <summary>
        /// Gets the default assembly name.
        /// </summary>
        private static readonly string DefaultAssemblyName = $"{DefaultTestProjectName}.dll";

        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest{DiagnosticAnalyzer, IVerifier}.Language" />
        /// </summary>
        private const string Language = LanguageNames.CSharp;


        /// <summary>
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest{DiagnosticAnalyzer, IVerifier}.DefaultLanguageVersion" />
        /// </summary>
        private static readonly LanguageVersion DefaultLanguageVersion =
            Enum.TryParse("Default", out LanguageVersion version) ? version : LanguageVersion.CSharp6;

        private static readonly Lazy<IExportProviderFactory> ExportProviderFactory;


        /// <inheritdoc cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}.CreateWorkspace" />
        private static AdhocWorkspace CreateWorkspace()
        {
            var exportProvider = ExportProviderFactory.Value.CreateExportProvider();
            var host = MefHostServices.Create(exportProvider.AsCompositionContext());
            return new AdhocWorkspace(host);
        }


        /// <inheritdoc cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest{IVerifier}"/>>
        static DiagnosticAnalyzerRunner()
        {
            ExportProviderFactory = new Lazy<IExportProviderFactory>(
                () =>
                {
                    var discovery = new AttributedPartDiscovery(Resolver.DefaultInstance, true);
                    var parts = Task.Run(() => discovery.CreatePartsAsync(MefHostServices.DefaultAssemblies))
                        .GetAwaiter().GetResult();
                    var catalog = ComposableCatalog.Create(Resolver.DefaultInstance).AddParts(parts)
                        .WithDocumentTextDifferencingService();

                    var configuration = CompositionConfiguration.Create(catalog);
                    var runtimeComposition = RuntimeComposition.CreateRuntimeComposition(configuration);
                    return runtimeComposition.CreateExportProviderFactory();
                },
                LazyThreadSafetyMode.ExecutionAndPublication
            );
        }



        /// <summary>
        /// None of codes specified but at least one code must be required.
        /// </summary>
        public class AtLeastOneCodeMustBeRequired : Exception
        {
            public override string ToString() => "None of codes specified but at least one code must be specified";
        }



        /// <summary>
        /// No compilation.
        /// </summary>
        public class NoCompilationError : Exception
        {
            public override string ToString() => "Cannot get a compilation";
        }
    }
}