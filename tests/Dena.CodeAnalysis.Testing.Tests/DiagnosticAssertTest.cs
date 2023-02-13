using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    [TestFixture]
    public class DiagnosticAssertTest
    {
        [Test]
        public void DiagnosticAssert_SameDiagnostic_Success()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage"));

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage"));

            NUnitAssert.DoesNotThrow(
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); });
        }

        [Test]
        public void DiagnosticAssert_ActualGraterThanExpected_Failed()
        {
            var diagnostic1 = ImmutableArray.Create(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>();

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 0 diagnostics, extra 1 diagnostics of all 1 diagnostics:
	extra	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_LargeNumberOfExpected_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>();

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 1 diagnostics, extra 0 diagnostics of all 0 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NoDiagnostics_Success()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>();

            var diagnostic2 = ImmutableArray.Create<Diagnostic>();

            NUnitAssert.DoesNotThrow(
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NotEqualsDDID_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID02",
                    "defaultMessage")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 1 diagnostics, extra 1 diagnostics of all 1 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	extra	path/to/defaultFile.cs: (100,200)-(300,400), ID02, defaultMessage
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NotEqualsLocationLinePositionSpan_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(5, 6, 7, 8, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 1 diagnostics, extra 1 diagnostics of all 1 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	extra	path/to/defaultFile.cs: (5,6)-(7,8), ID01, defaultMessage
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NotEqualsLocationPath_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "hoge/fuga/moge.cs"),
                    "ID01",
                    "defaultMessage")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 1 diagnostics, extra 1 diagnostics of all 1 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	extra	hoge/fuga/moge.cs: (100,200)-(300,400), ID01, defaultMessage
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NotEqualsDiagnosticMessage_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "message2")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 1 diagnostics, extra 1 diagnostics of all 1 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	extra	path/to/defaultFile.cs: (100,200)-(300,400), ID01, message2
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_NotEqualsMultipleDiagnostics_Failed()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(9, 10, 11, 12, "path/to/target3.cs"),
                    "ID03",
                    "message3"),
                CreateDummyDiagnostic(
                    CreateDummyLocation(13, 14, 15, 16, "path/to/target4.cs"),
                    "ID04",
                    "message4")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage"),
                CreateDummyDiagnostic(
                    CreateDummyLocation(5, 6, 7, 8, "path/to/target2.cs"),
                    "ID02",
                    "message2")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"Missing 2 diagnostics, extra 2 diagnostics of all 2 diagnostics:
	missing	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	missing	path/to/target2.cs: (5,6)-(7,8), ID02, message2
	extra	path/to/target3.cs: (9,10)-(11,12), ID03, message3
	extra	path/to/target4.cs: (13,14)-(15,16), ID04, message4
"),
                delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); }
            );
        }

        [Test]
        public void DiagnosticAssert_EqualsMultipleDiagnostics_Success()
        {
            var diagnostic1 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(5, 6, 7, 8, "path/to/target2.cs"),
                    "ID02",
                    "message2"),
                CreateDummyDiagnostic(
                    CreateDummyLocation(9, 10, 11, 12, "path/to/target3.cs"),
                    "ID03",
                    "message3")
            );

            var diagnostic2 = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(5, 6, 7, 8, "path/to/target2.cs"),
                    "ID02",
                    "message2"),
                CreateDummyDiagnostic(
                    CreateDummyLocation(9, 10, 11, 12, "path/to/target3.cs"),
                    "ID03",
                    "message3")
            );

            NUnitAssert.DoesNotThrow(delegate { DiagnosticsAssert.AreEqual(diagnostic2, diagnostic1); });
        }

        [Test]
        public void IsEmpty_ZeroActual_Success()
        {
            var actual = ImmutableArray.Create<Diagnostic>();
            NUnitAssert.DoesNotThrow(delegate { DiagnosticsAssert.IsEmpty(actual); });
        }

        [Test]
        public void IsEmpty_OneActual_Failed()
        {
            var actuals = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"expected no diagnostics, but 1 diagnostics are reported
	extra	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
"),
                delegate { DiagnosticsAssert.IsEmpty(actuals); }
            );
        }

        [Test]
        public void IsEmpty_ManyActuals_Failed()
        {
            var actuals = ImmutableArray.Create<Diagnostic>(
                CreateDummyDiagnostic(
                    CreateDummyLocation(100, 200, 300, 400, "path/to/defaultFile.cs"),
                    "ID01",
                    "defaultMessage"),
                CreateDummyDiagnostic(
                    CreateDummyLocation(5, 6, 7, 8, "path/to/target2.cs"),
                    "ID02",
                    "message2")
            );

            NUnitAssert.Throws(Is.TypeOf<AssertFailedException>()
                    .And.Message.EqualTo(
                        @"expected no diagnostics, but 2 diagnostics are reported
	extra	path/to/defaultFile.cs: (100,200)-(300,400), ID01, defaultMessage
	extra	path/to/target2.cs: (5,6)-(7,8), ID02, message2
"),
                delegate { DiagnosticsAssert.IsEmpty(actuals); }
            );
        }

        private static Diagnostic CreateDummyDiagnostic(Location location, string id, string message,
            string category = "defaultCategory", DiagnosticSeverity severity = DiagnosticSeverity.Hidden)
        {
            var diagnosticDescriptor = new DiagnosticDescriptor(
                id,
                "title",
                message,
                category,
                severity,
                true);
            return Diagnostic.Create(
                diagnosticDescriptor,
                location);
        }

        private static Location CreateDummyLocation(int startLine, int startCharacter, int endLine,
            int endCharacter,
            string path)
        {
            return Location.Create(
                path,
                // DiagnosticAssertに於いて、TextSpanは使用しない
                new TextSpan(0, 0),
                new LinePositionSpan(new LinePosition(startLine, startCharacter),
                    new LinePosition(endLine, endCharacter))
            );
        }
    }
}