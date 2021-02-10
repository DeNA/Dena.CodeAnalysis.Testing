using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    [TestClass]
    public class LocationAssertTest
    {
        [TestMethod]
        public async Task HaveTheSpanWithPath_Success()
        {
            var actual = await LocationFactory.Create();

            LocationAssert.HaveTheSpan(
                "/0/Test0.",
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
                    "/0/Test999.",
                    new LinePosition(999, 999),
                    new LinePosition(999, 999),
                    actual
                );
            }
            catch (AssertFailedException e)
            {
                Assert.AreEqual(
                    @"Assert.IsFalse failed.   {
-     Path = ""/0/Test999.""
+     Path = ""/0/Test0.""
-     StartLinePosition = new LinePosition(999, 999)
+     StartLinePosition = new LinePosition(8, 0)
-     EndLinePosition = new LinePosition(999, 999)
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
                new LinePosition(1, 0),
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
                Assert.AreEqual(
                    @"Assert.IsFalse failed.   {
-     StartLinePosition = new LinePosition(999, 999)
+     StartLinePosition = new LinePosition(8, 0)
-     EndLinePosition = new LinePosition(999, 999)
+     EndLinePosition = new LinePosition(8, 5)
  }
",
                    e.Message
                );
            }
        }
    }
}