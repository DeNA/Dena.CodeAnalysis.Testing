using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    [TestClass]
    public class LocationAssertTest
    {
        [TestMethod]
        public async Task HaveTheSpanWithPath_Sucecss()
        {
            var actual = await LocationFactory.Create();

            LocationAssert.HaveTheSpan(
                "/0/Test0.",
                new LinePosition(1, 0),
                new LinePosition(8, 5),
                actual
            );
        }


        [TestMethod]
        public async Task HaveTheSpanWithPath_Failed()
        {
            var actual = await LocationFactory.Create();

            Assert.ThrowsException<AssertFailedException>(
                () =>
                {
                    LocationAssert.HaveTheSpan(
                        "/0/Test999.",
                        new LinePosition(999, 999),
                        new LinePosition(999, 999),
                        actual
                    );
                }
            );
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

            Assert.ThrowsException<AssertFailedException>(
                () =>
                {
                    LocationAssert.HaveTheSpan(
                        new LinePosition(999, 999),
                        new LinePosition(999, 999),
                        actual
                    );
                }
            );
        }
    }
}