using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.MSBuild;



namespace TestRoslynRunner
{
    public class AnalyzerTest {
        /// <summary>
        /// 渡された <c cref="DiagnosticAnalyzer">DiagnosticAnalyzer</c> を使って指定したコードを静的検査する。
        /// </summary>
        /// <param name="analyzer">実行したい <c cref="DiagnosticAnalyzer">DiagnosticAnalyzer</c></param>
        /// <param name="codes">静的検査したいコード。1つ以上指定されてないと例外がでる</param>
        /// <returns><param name="analyzer"/>が作成した<c cref="Diagnostic"/>の配列</returns>
        /// <throws>与えた<param name="codes"/>が空だったら<c cref="Exception"/>が投げられる</throws>
        public async Task<ImmutableArray<Diagnostic>> RunAnalyzer(DiagnosticAnalyzer analyzer, string[] codes)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                MSBuidLocatorInitializer.Setup();
                var projectId = ProjectId.CreateNewId(debugName: null);
                var solution = workspace
                    .CurrentSolution
                    .AddProject(projectId, "", "", LanguageNames.CSharp);
                // Note: documentは、solution経由でprojectにセットする
                foreach (var code in codes)
                {
                    var documentId = DocumentId.CreateNewId(projectId, debugName: null);
                    solution = solution.AddDocument(documentId, "", code, filePath: "");
                }

                var noMetadataReferenceProject = solution.Projects.First();
                // Note: noMetadataReferenceProjectに標準ライブラリが存在していないため
                var metadataReferences =
                    await ReferenceAssemblies.Default.ResolveAsync(LanguageNames.CSharp, CancellationToken.None);
                var project = noMetadataReferenceProject.AddMetadataReferences(metadataReferences);
                var compilation = await project.GetCompilationAsync();
                var withnalyzers = compilation!.WithAnalyzers(
                    ImmutableArray.Create<DiagnosticAnalyzer>(analyzer)
                );
                return await withnalyzers.GetAllDiagnosticsAsync();
            }
        }
    }
}
