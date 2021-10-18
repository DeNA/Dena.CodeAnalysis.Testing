using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestClass]
    public class LocationAssertTest
    {
        [TestMethod]
        public async Task HaveTheSpanWithPath_Success()
        {
            var actual = await LocationFactory.Create();

            LocationAssert.HaveTheSpan(
                "/0/Test0.cs",
                new LinePosition(8, 0),
                new LinePosition(8, 5),
                actual
            );
        }


        [TestMethod]
        public async Task HaveTheSpanWithPath_Failed()
        {
            var actual = await LocationFactory.Create();

            try
            {
                LocationAssert.HaveTheSpan(
                    "/0/Test999.cs",
                    new LinePosition(999, 999),
                    new LinePosition(999, 999),
                    actual
                );
            }
            catch (AssertFailedException e)
            {
                MSTestAssert.AreEqual(
                    @"  {
-     Path = ""/0/Test999.cs""
+     Path = ""/0/Test0.cs""
-     // It will be shown by 1-based index like: ""/0/Test999.cs(1000,1000): Lorem Ipsum ..."")
-     StartLinePosition = new LinePosition(999, 999)
+     // It will be shown by 1-based index like: ""/0/Test0.cs(9,1): Lorem Ipsum ..."")
+     StartLinePosition = new LinePosition(8, 0)
-     // It will be shown by 1-based index like: ""/0/Test999.cs(1000,1000): Lorem Ipsum ..."")
-     EndLinePosition = new LinePosition(999, 999)
+     // It will be shown by 1-based index like: ""/0/Test0.cs(9,6): Lorem Ipsum ..."")
+     EndLinePosition = new LinePosition(8, 5)
  }
",
                    e.Message
                );
            }
        }


        [TestMethod]
        public async Task HaveTheSpanWithoutPath_Success()
        {
            var actual = await LocationFactory.Create();

            LocationAssert.HaveTheSpan(
                new LinePosition(8, 0),
                new LinePosition(8, 5),
                actual
            );
        }


        [TestMethod]
        public async Task HaveTheSpanWithoutPath_Failed()
        {
            var actual = await LocationFactory.Create();

            try
            {
                LocationAssert.HaveTheSpan(
                    new LinePosition(999, 999),
                    new LinePosition(999, 999),
                    actual
                );
            }
            catch (AssertFailedException e)
            {
                MSTestAssert.AreEqual(
                    @"  {
-     // It will be shown by 1-based index like: ""/path/to/unchecked.cs(1000,1000): Lorem Ipsum ..."")
-     StartLinePosition = new LinePosition(999, 999)
+     // It will be shown by 1-based index like: ""/path/to/unchecked.cs(9,1): Lorem Ipsum ..."")
+     StartLinePosition = new LinePosition(8, 0)
-     // It will be shown by 1-based index like: ""/path/to/unchecked.cs(1000,1000): Lorem Ipsum ..."")
-     EndLinePosition = new LinePosition(999, 999)
+     // It will be shown by 1-based index like: ""/path/to/unchecked.cs(9,6): Lorem Ipsum ..."")
+     EndLinePosition = new LinePosition(8, 5)
  }
",
                    e.Message
                );
            }
        }
    }
}