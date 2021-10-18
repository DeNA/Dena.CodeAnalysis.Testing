using System;

namespace Dena.CodeAnalysis.CSharp.Testing
{
    /// <summary>
    /// Framework-independent assertions.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Throw an exception.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <exception cref="AssertFailedException"></exception>
        public static void Fail(string message)
        {
            throw new AssertFailedException(message);
        }
    }


    /// <summary>
    /// An exception for assertions.
    /// </summary>
    public class AssertFailedException : Exception
    {
        public AssertFailedException(string message) : base(message)
        {
        }
    }
}