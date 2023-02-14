using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestFixture]
    public class ReadFromFileTest
    {
        [Test]
        [TestCaseSource(nameof(GoodTestCases))]
        public void GoodTestCase(string path)
        {
            NUnitAssert.That(ReadFromFile.ReadFile(path), Does.Contain("hello-world"));
        }


        private static object[][] GoodTestCases =>
            new[]
            {
                new object[] { TestData.GetPath("Example.txt") }
            };
    }
}