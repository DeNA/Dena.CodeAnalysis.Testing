// (c) 2021 DeNA Co., Ltd.

using System.IO;
using System.Runtime.CompilerServices;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    public static class TestData
    {
        public static string GetPath(string fileName) => Path.Combine(Path.GetDirectoryName(GetFilePath())!, fileName);

        private static string GetFilePath([CallerFilePath] string path = "") => path;
    }
}
