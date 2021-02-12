using System.Threading.Tasks;
using Microsoft.CodeAnalysis;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    public static class LocationFactory
    {
        public static async Task<Location> Create()
        {
            var ds = await DiagnosticAnalyzerRunner.Run(
                new NullAnalyzer(),
                ExampleCode.ContainingSyntaxError
            );
            return ds[0].Location;
        }
    }
}