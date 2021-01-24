using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// A collection of helper classes to test various conditions associated
    /// with sets within unit tests. If the condition being tested is not
    /// met, an exception is thrown.
    /// </summary>
    public static class SetAssert
    {
        /// <summary>
        /// Tests whether the specified sets are equal and throws an exception
        /// if the two sets are not equal.
        /// </summary>
        /// <param name="expected">The first set to compare. This is the set the tests expectes.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <typeparam name="T">The type of the element of the sets to compare.</typeparam>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Thrown if <paramref name="expected" /> is not equal to <paramref name="actual" />.
        /// </exception>
        public static void AreEqual<T>(ISet<T> expected, ISet<T> actual)
        {
            if (expected.SetEquals(actual))
            {
                return;
            }

            var missing = new HashSet<T>(expected);
            var extra = new HashSet<T>(actual);
            var common = new HashSet<T>(expected);

            missing.ExceptWith(actual);
            extra.ExceptWith(expected);
            common.IntersectWith(actual);

            var builder = new StringBuilder();
            foreach (var l in missing)
            {
                builder.Append($"\n- {l}");
            }

            foreach (var r in extra)
            {
                builder.Append($"\n+ {r}");
            }

            foreach (var x in common)
            {
                builder.Append($"\n  {x}");
            }

            Assert.Fail(builder.ToString());
        }
    }
}