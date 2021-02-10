using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    public static class LocationAssert
    {
        public static void HaveTheSpan(
            string expectedPath,
            LinePosition expectedStart,
            LinePosition expectedEnd,
            Location actual
        )
        {
            var actualSpan = actual.GetLineSpan();

            var builder = new StringBuilder();

            builder.AppendLine("  {");
            var pathDiff = DiffPath(builder, expectedPath, actualSpan.Path);
            var startDiff = DiffStartLinePos(builder, expectedStart, actualSpan.StartLinePosition);
            var endDiff = DiffEndLinePos(builder, expectedEnd, actualSpan.EndLinePosition);
            builder.AppendLine("  }");

            Assert.IsFalse(pathDiff && startDiff && endDiff, builder.ToString());
        }


        public static void HaveTheSpan(
            LinePosition expectedStart,
            LinePosition expectedEnd,
            Location actual
        )
        {
            var actualSpan = actual.GetLineSpan();

            var builder = new StringBuilder();

            builder.AppendLine("  {");
            var startDiff = DiffStartLinePos(builder, expectedStart, actualSpan.StartLinePosition);
            var endDiff = DiffEndLinePos(builder, expectedEnd, actualSpan.EndLinePosition);
            builder.AppendLine("  }");

            Assert.IsFalse(startDiff && endDiff, builder.ToString());
        }


        private static bool DiffPath(StringBuilder builder, string expected, string actual)
        {
            if (actual.Equals(expected))
            {
                builder.AppendLine($"      Path = \"{actual}\"");
                return false;
            }

            builder.AppendLine($"-     Path = \"{expected}\"");
            builder.AppendLine($"+     Path = \"{actual}\"");
            return true;
        }


        private static bool DiffStartLinePos(StringBuilder builder, LinePosition expected, LinePosition actual)
        {
            if (actual.Equals(expected))
            {
                builder.AppendLine(
                    $"      StartLinePosition = new LinePosition({actual.Line}, {actual.Character})"
                );
                return false;
            }

            builder.AppendLine(
                $"-     StartLinePosition = new LinePosition({expected.Line}, {expected.Character})"
            );
            builder.AppendLine(
                $"+     StartLinePosition = new LinePosition({actual.Line}, {actual.Character})"
            );
            return true;
        }


        private static bool DiffEndLinePos(StringBuilder builder, LinePosition expected, LinePosition actual)
        {
            if (actual.Equals(expected))
            {
                builder.AppendLine(
                    $"      EndLinePosition = new LinePosition({actual.Line}, {actual.Character})"
                );
                return false;
            }

            builder.AppendLine(
                $"-     EndLinePosition = new LinePosition({expected.Line}, {expected.Character})"
            );
            builder.AppendLine(
                $"+     EndLinePosition = new LinePosition({actual.Line}, {actual.Character})"
            );
            return true;
        }
    }
}