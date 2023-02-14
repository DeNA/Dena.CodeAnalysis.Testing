using System.IO;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    public static class ReadFromFile
    {
        /// <summary>
        /// Return a contents of specified file.
        /// </summary>
        /// <param name="path">File relative path (relative to project root)</param>
        /// <returns>
        /// File contents.
        /// </returns>
        public static string ReadFile(string path)
        {
            return File.ReadAllText(Path.Combine("./../../..", path));
        }
    }
}