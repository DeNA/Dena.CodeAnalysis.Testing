using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.Composition.Reflection;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    // XXX: This is internal in Microsoft.CodeAnalysis.Testing, but it is needed by AnalyzerRunner.
    /// <inheritdoc cref="Microsoft.CodeAnalysis.Testing.ComposableCatalogExtensions" />
    internal static class ComposableCatalogExtensions
    {
        /// <inheritdoc cref="Microsoft.CodeAnalysis.Testing.ComposableCatalogExtensions.WithDocumentTextDifferencingService" />
        public static ComposableCatalog WithDocumentTextDifferencingService(this ComposableCatalog catalog)
        {
            var assemblyQualifiedServiceTypeName =
                $"Microsoft.CodeAnalysis.IDocumentTextDifferencingService, {typeof(Workspace).GetTypeInfo().Assembly.GetName()}";

            // Check to see if IDocumentTextDifferencingService is exported
            foreach (var part in catalog.Parts)
            {
                foreach (var pair in part.ExportDefinitions)
                {
                    var exportDefinition = pair.Value;
                    if (exportDefinition.ContractName != "Microsoft.CodeAnalysis.Host.IWorkspaceService")
                    {
                        continue;
                    }

                    if (!exportDefinition.Metadata.TryGetValue("ServiceType", out var value)
                        || !(value is string serviceType))
                    {
                        continue;
                    }

                    if (serviceType != assemblyQualifiedServiceTypeName)
                    {
                        continue;
                    }

                    // The service is exported by default
                    return catalog;
                }
            }

            // If IDocumentTextDifferencingService is not exported by default, export it manually
            var manualExportDefinition = new ExportDefinition(
                typeof(IWorkspaceService).FullName,
                new Dictionary<string, object>
                {
                    {"ExportTypeIdentity", typeof(IWorkspaceService).FullName},
                    {nameof(ExportWorkspaceServiceAttribute.ServiceType), assemblyQualifiedServiceTypeName},
                    {nameof(ExportWorkspaceServiceAttribute.Layer), ServiceLayer.Default},
                    {typeof(CreationPolicy).FullName ?? string.Empty, CreationPolicy.Shared},
                    {"ContractType", typeof(IWorkspaceService)},
                    {"ContractName", null}
                }
            );

            var serviceImplType = typeof(Workspace).GetTypeInfo().Assembly
                .GetType("Microsoft.CodeAnalysis.DefaultDocumentTextDifferencingService");

            return catalog.AddPart(
                new ComposablePartDefinition(
                    TypeRef.Get(serviceImplType, Resolver.DefaultInstance),
                    new Dictionary<string, object> {{"SharingBoundary", null}},
                    new[] {manualExportDefinition},
                    new Dictionary<MemberRef, IReadOnlyCollection<ExportDefinition>>(),
                    Enumerable.Empty<ImportDefinitionBinding>(),
                    string.Empty,
                    default,
                    MethodRef.Get(
                        serviceImplType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).First(),
                        Resolver.DefaultInstance
                    ),
                    new List<ImportDefinitionBinding>(),
                    CreationPolicy.Shared,
                    new[] {typeof(Workspace).GetTypeInfo().Assembly.GetName()}
                )
            );
        }
    }
}