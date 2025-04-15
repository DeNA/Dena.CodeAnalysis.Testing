// (c) 2021 DeNA Co., Ltd.

using System.Linq;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestFixture]
    public class TestDataParserTest
    {
        private const string TestData = @"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = {|new Stack()|DENA001|Do not use Stack because non-generic collection|};
        }
    }
}";

        [Test]
        public void CreateSourceAndExpectedDiagnostic_HasOneReportPart_extractSourceCode()
        {
            var (source, _) =
                TestDataParser.CreateSourceAndExpectedDiagnostic(TestData);
            NUnitAssert.That(source, Is.EqualTo(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = new Stack();
        }
    }
}"
            ));
        }

        [Test]
        public void CreateSourceAndExpectedDiagnosticFromFile_HasOneReportPart_CreateDiagnostic()
        {
            var (_, actualDiagnostics) =
                TestDataParser.CreateSourceAndExpectedDiagnostic(TestData);
            NUnitAssert.That(actualDiagnostics, Has.Count.EqualTo(1));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actualDiagnostics[0].Id, Is.EqualTo("DENA001"));
                NUnitAssert.That(actualDiagnostics[0].GetMessage(),
                    Is.EqualTo("Do not use Stack because non-generic collection"));
                NUnitAssert.That(actualDiagnostics[0].Location.SourceSpan, Is.EqualTo(new TextSpan(198, 11)));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_HasOneReportPart_ReturnReportPoint()
        {
            var actual = TestDataParser.ExtractMaker(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = {|new Stack()|DENA001|Do not use Stack because non-generic collection|};
        }
    }
}").ToList();

            NUnitAssert.That(actual.Count, Is.EqualTo(1));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual[0].target, Is.EqualTo("new Stack()"));
                NUnitAssert.That(actual[0].ddid, Is.EqualTo("DENA001"));
                NUnitAssert.That(actual[0].msg, Is.EqualTo("Do not use Stack because non-generic collection"));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_ContainsBracketsInMarker_ReturnReportPoint()
        {
            var actual = TestDataParser.ExtractMaker(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = {|new Stack[5] {}|DENA001|Do not use Stack because non-generic collection|};
        }
    }
}").ToList();

            NUnitAssert.That(actual, Has.Count.EqualTo(1));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual[0].target, Is.EqualTo("new Stack[5] {}"));
                NUnitAssert.That(actual[0].ddid, Is.EqualTo("DENA001"));
                NUnitAssert.That(actual[0].msg, Is.EqualTo("Do not use Stack because non-generic collection"));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_HasNoReportPart_ReturnEmptyList()
        {
            var actual = TestDataParser.ExtractMaker(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = new Stack();
        }
    }
}").Any();

            NUnitAssert.That(actual, Is.False);
        }

        [Test]
        public void ExtractMakerFromTestData_WithTwoReportPoints_ReturnTwoReportPoints()
        {
            var actual = TestDataParser.ExtractMaker(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            var fuga = {|new Stack()|DENA001|Do not use Stack because non-generic collection|};
            var moge = {|new ArrayList()|DENA002|Do not use ArrayList because non-generic collection|};
        }
    }
}").ToList();

            NUnitAssert.That(actual, Has.Count.EqualTo(2));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual[0].target, Is.EqualTo("new Stack()"));
                NUnitAssert.That(actual[0].ddid, Is.EqualTo("DENA001"));
                NUnitAssert.That(actual[0].msg, Is.EqualTo("Do not use Stack because non-generic collection"));
                NUnitAssert.That(actual[1].target, Is.EqualTo("new ArrayList()"));
                NUnitAssert.That(actual[1].ddid, Is.EqualTo("DENA002"));
                NUnitAssert.That(actual[1].msg, Is.EqualTo("Do not use ArrayList because non-generic collection"));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_TwoReportPointsOnTheSameLine_ReturnTwoReportPoints()
        {
            var actual = TestDataParser.ExtractMaker(@"
using System.Collections;
namespace BanNonGenericCollectionsAnalyzer.Test.TestData.OperationAction
{
    public class Instantiate
    {
        private void Hoge()
        {
            {|Banned1|DENA001|Message1|} = {|Banned2|DENA002|Message2|};
        }
    }
}").ToList();

            NUnitAssert.That(actual, Has.Count.EqualTo(2));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual[0].target, Is.EqualTo("Banned1"));
                NUnitAssert.That(actual[0].ddid, Is.EqualTo("DENA001"));
                NUnitAssert.That(actual[0].msg, Is.EqualTo("Message1"));
                NUnitAssert.That(actual[1].target, Is.EqualTo("Banned2"));
                NUnitAssert.That(actual[1].ddid, Is.EqualTo("DENA002"));
                NUnitAssert.That(actual[1].msg, Is.EqualTo("Message2"));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_OneCharacterPerElementInEachReportPart_ReturnOneReportPoint()
        {
            var actual = TestDataParser.ExtractMaker(@"{|A|B|C|}").ToList();

            NUnitAssert.That(actual, Has.Count.EqualTo(1));
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual[0].target, Is.EqualTo("A"));
                NUnitAssert.That(actual[0].ddid, Is.EqualTo("B"));
                NUnitAssert.That(actual[0].msg, Is.EqualTo("C"));
            });
        }

        [Test]
        public void ExtractMakerFromTestData_IncludeVerticalBarsInMarker_NoReturnReportPoint()
        {
            var actual = TestDataParser.ExtractMaker(@"{||A||B||C|}").ToList();

            NUnitAssert.That(actual, Is.Empty);
        }

        [Test]
        public void CreateLinePositionStart_BeginningReportParts_GetCorrectPosition()
        {
            var actual = TestDataParser.CreateLinePositionStart("aaa\nbbbc\nccc", "aaa");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.Line, Is.EqualTo(0));
                NUnitAssert.That(actual.Character, Is.EqualTo(0));
            });
        }

        [Test]
        public void CreateLinePositionStart_ReportPartIsOnTheFirstLine_GetCorrectPosition()
        {
            var actual = TestDataParser.CreateLinePositionStart("hogeaaa\nbbbc\nccc", "aaa");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.Line, Is.EqualTo(0));
                NUnitAssert.That(actual.Character, Is.EqualTo(4));
            });
        }

        [Test]
        public void CreateLinePositionStart_ReportPartExistsMiddleLineFirstCol_GetCorrectPosition()
        {
            var actual = TestDataParser.CreateLinePositionStart("aaa\nbcbb\nccc", "bc");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.Line, Is.EqualTo(1));
                NUnitAssert.That(actual.Character, Is.EqualTo(0));
            });
        }

        [Test]
        public void CreateLinePositionStart_ReportPartExistsMiddleLineLastCol_GetCorrectPosition()
        {
            var actual = TestDataParser.CreateLinePositionStart("aaa\nbbbc\nccc", "bc");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.Line, Is.EqualTo(1));
                NUnitAssert.That(actual.Character, Is.EqualTo(2));
            });
        }

        [Test]
        public void CreateLinePositionStart_ReportPointExistsFirstLine_GetCorrectPosition()
        {
            var actual = TestDataParser.CreateLinePositionStart("aaa\nbbbc\nccc", "ccc");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.Line, Is.EqualTo(2));
                NUnitAssert.That(actual.Character, Is.EqualTo(0));
            });
        }

        [Test]
        public void CreateLocation_ReportPointExistsMiddleLine_GetCorrectSourceSpan()
        {
            var actual = TestDataParser.CreateLocation("aaa\nbbbc\nccc", "bc", 2, "/0/Test0.cs");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.SourceSpan.Start, Is.EqualTo(6));
                NUnitAssert.That(actual.SourceSpan.End, Is.EqualTo(8));
            });
        }

        [Test]
        public void CreateLocation_ReportPointExistsLastLine_GetCorrectSourceSpan()
        {
            var actual = TestDataParser.CreateLocation("aaa\nbbbc\nccc", "ccc", 3, "/0/Test0.cs");
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(actual.SourceSpan.Start, Is.EqualTo(9));
                NUnitAssert.That(actual.SourceSpan.End, Is.EqualTo(12));
            });
        }
    }
}
