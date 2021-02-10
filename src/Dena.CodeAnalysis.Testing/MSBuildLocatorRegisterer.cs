using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;



namespace Dena.CodeAnalysis.Testing
{
    /// <summary>
    /// A thread-safe initializer for <see cref="MSBuildLocator" />.
    /// NOTE: MSBuildLocator must be registered ONLY ONCE until call <see cref="MSBuildWorkspace.Create()" /> or get exceptions.
    /// </summary>
    public static class MSBuildLocatorRegisterer
    {
        private static bool _registered;
        private static readonly object RegisteredLock = new();


        /// <summary>
        /// Register <see cref="MSBuildLocator" /> by <see cref="MSBuildLocator.RegisterDefaults()" />.
        /// Do nothing if it have been already registered in the process.
        /// This method is thread-safe.
        /// </summary>
        public static void RegisterIfNecessary()
        {
            lock (RegisteredLock)
            {
                if (_registered)
                {
                    return;
                }

                _registered = true;
            }

            MSBuildLocator.RegisterDefaults();
        }
    }
}