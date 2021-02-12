using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// Assertions for <see cref="Microsoft.CodeAnalysis.Location"/>.
    /// </summary>
    public static class LocationAssert
    {
        /// <summary>
        /// Tests whether the following 3 properties of <see cref="Location.GetLineSpan" /> and throws an exception
        /// if the properties are not match.
        ///
        /// The 3 properties are:
        /// <list type="bullet">
        /// <item><see cref="FileLinePositionSpan.Path" /></item>
        /// <item><see cref="FileLinePositionSpan.StartLinePosition" /></item>
        /// <item><see cref="FileLinePositionSpan.EndLinePosition" /></item>
        /// </list>
        /// </summary>
        /// <param name="expectedPath">The file path that expected.</param>
        /// <param name="expectedStart">The start line position that expected.</param>
        /// <param name="expectedEnd">The end line position that expected.</param>
        /// <param name="actual">The actual location.</param>
        /// <exception cref="Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Thrown if <paramref name="actual" /> have wrong properties.
        /// </exception>
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
            var startDiff = DiffStartLinePos(
                builder,
                expectedStart,
                actualSpan.StartLinePosition,
                expectedPath,
                actualSpan.Path
            );
            var endDiff = DiffEndLinePos(
                builder,
                expectedEnd,
                actualSpan.EndLinePosition,
                expectedPath,
                actualSpan.Path
            );
            builder.AppendLine("  }");

            if (pathDiff || startDiff || endDiff)
            {
                Assert.Fail(builder.ToString());
            }
        }


        /// <summary>
        /// Tests whether the 2 properties of <see cref="Location.GetLineSpan" /> and throws an exception
        /// if the properties are not match.
        ///
        /// The 2 major properties are:
        /// <list type="bullet">
        /// <item><see cref="FileLinePositionSpan.StartLinePosition" /></item>
        /// <item><see cref="FileLinePositionSpan.EndLinePosition" /></item>
        /// </list>
        /// </summary>
        /// <param name="expectedStart">The start line position that expected.</param>
        /// <param name="expectedEnd">The end line position that expected.</param>
        /// <param name="actual">The actual location.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Thrown if <paramref name="actual" /> have wrong properties.
        /// </exception>
        public static void HaveTheSpan(
            LinePosition expectedStart,
            LinePosition expectedEnd,
            Location actual
        )
        {
            var actualSpan = actual.GetLineSpan();

            var builder = new StringBuilder();

            builder.AppendLine("  {");
            var startDiff = DiffStartLinePos(
                builder,
                expectedStart,
                actualSpan.StartLinePosition,
                UncheckedFilePath,
                UncheckedFilePath
            );
            var endDiff = DiffEndLinePos(
                builder,
                expectedEnd,
                actualSpan.EndLinePosition,
                UncheckedFilePath,
                UncheckedFilePath
            );
            builder.AppendLine("  }");

            if (startDiff || endDiff)
            {
                Assert.Fail(builder.ToString());
            }
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


        private static bool DiffStartLinePos(
            StringBuilder builder,
            LinePosition expected,
            LinePosition actual,
            string expectedPath,
            string actualPath
        )
        {
            if (actual.Equals(expected))
            {
                builder.AppendLine(
                    $"      // It will be shown by 1-based index like: \"{actualPath}({actual.Line + 1},{actual.Character + 1}): Lorem Ipsum ...\")"
                );
                builder.AppendLine(
                    $"      StartLinePosition = new LinePosition({actual.Line}, {actual.Character})"
                );
                return false;
            }

            builder.AppendLine(
                $"-     // It will be shown by 1-based index like: \"{expectedPath}({expected.Line + 1},{expected.Character + 1}): Lorem Ipsum ...\")"
            );
            builder.AppendLine(
                $"-     StartLinePosition = new LinePosition({expected.Line}, {expected.Character})"
            );
            builder.AppendLine(
                $"+     // It will be shown by 1-based index like: \"{actualPath}({actual.Line + 1},{actual.Character + 1}): Lorem Ipsum ...\")"
            );
            builder.AppendLine(
                $"+     StartLinePosition = new LinePosition({actual.Line}, {actual.Character})"
            );
            return true;
        }


        private static bool DiffEndLinePos(
            StringBuilder builder,
            LinePosition expected,
            LinePosition actual,
            string expectedPath,
            string actualPath
        )
        {
            if (actual.Equals(expected))
            {
                builder.AppendLine(
                    $"      // It will be shown by 1-based index like: \"{expectedPath}({actual.Line + 1},{actual.Character + 1}): Lorem Ipsum ...\")"
                );
                builder.AppendLine(
                    $"      EndLinePosition = new LinePosition({actual.Line}, {actual.Character})"
                );
                return false;
            }

            builder.AppendLine(
                $"-     // It will be shown by 1-based index like: \"{expectedPath}({expected.Line + 1},{expected.Character + 1}): Lorem Ipsum ...\")"
            );
            builder.AppendLine(
                $"-     EndLinePosition = new LinePosition({expected.Line}, {expected.Character})"
            );
            builder.AppendLine(
                $"+     // It will be shown by 1-based index like: \"{actualPath}({actual.Line + 1},{actual.Character + 1}): Lorem Ipsum ...\")"
            );
            builder.AppendLine(
                $"+     EndLinePosition = new LinePosition({actual.Line}, {actual.Character})"
            );
            return true;
        }


        private const string UncheckedFilePath = "/path/to/unchecked.cs";
    }
}