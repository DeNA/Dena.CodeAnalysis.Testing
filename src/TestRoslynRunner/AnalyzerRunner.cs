using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.MSBuild;



namespace Dena.CodeAnalysis.Testing
{
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
        )
        {
            return await Run(analyzer, CancellationToken.None, codes);
        }


        /// <summary>
        /// Run the specified <see cref="DiagnosticAnalyzer" />.
        /// </summary>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer" /> to run.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> that the task will observe.</param>
        /// <param name="codes">The target code that the <paramref name="analyzer" /> analyze.</param>
        /// <returns>ImmutableArray contains all reported <see cref="Diagnostic" />.</returns>
        /// <throws>Throws <c cref="AtLeastOneCodeMustBeRequired" /> if <paramref name="codes" /> are empty.</throws>
        public static async Task<ImmutableArray<Diagnostic>> Run(
            DiagnosticAnalyzer analyzer,
            CancellationToken cancellationToken,
            params string[] codes
        )
        {
            if (!codes.Any())
            {
                throw new AtLeastOneCodeMustBeRequired();
            }

            MSBuidLocatorInitializer.Setup();
            using (var workspace = MSBuildWorkspace.Create())
            {
                var projectId = ProjectId.CreateNewId();
                var solution = workspace
                    .CurrentSolution
                    .AddProject(projectId, DefaultTestProjectName, DefaultAssemblyName, LanguageNames.CSharp);

                foreach (var code in codes)
                {
                    var documentId = DocumentId.CreateNewId(projectId);
                    solution = solution.AddDocument(documentId, DefaultFilePath, code, filePath: DefaultFilePath);
                }

                var noMetadataReferencedProject = solution.Projects.First();

                // NOTE: Make standard libraries visible to the specified codes to analyze.
                var metadataReferences =
                    await ReferenceAssemblies.Default.ResolveAsync(LanguageNames.CSharp, CancellationToken.None);

                var project = noMetadataReferencedProject.AddMetadataReferences(metadataReferences);
                var compilation = await project.GetCompilationAsync();
                var withnalyzers = compilation!.WithAnalyzers(
                    ImmutableArray.Create(analyzer)
                );
                return await withnalyzers.GetAllDiagnosticsAsync(cancellationToken);
            }
        }


        /// <summary>
        /// Gets the prefix to apply to source files added without an explicit name.
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest.DefaultFilePathPrefix" />
        /// </summary>
        public static readonly string DefaultFilePathPrefix = "/0/Test";

        /// <summary>
        /// Gets the name of the default project created for testing.
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest.DefaultTestProjectName" />
        /// </summary>
        public static readonly string DefaultTestProjectName = "TestProject";

        /// <summary>
        /// Gets the default full name of the first source file added for a test.
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest.DefaultFilePath" />
        /// </summary>
        public static string DefaultFilePath => $"{DefaultFilePathPrefix}{0}.{DefaultFileExt}";

        /// <summary>
        /// Gets the default file extension to use for files added to the test without an explicit name.
        /// This value is equivalent to <see cref="Microsoft.CodeAnalysis.Testing.AnalyzerTest.DefaultFileExt" />
        /// </summary>
        public static readonly string DefaultFileExt;

        /// <summary>
        /// Gets the default assembly name.
        /// </summary>
        public static string DefaultAssemblyName => $"{DefaultTestProjectName}.dll";



        /// <summary>
        /// None of codes specified but at least one code must be required.
        /// </summary>
        public class AtLeastOneCodeMustBeRequired : Exception
        {
            public override string ToString() => "None of codes specified but at least one code must be required";
        }
    }
}