using Microsoft.Build.Locator;



namespace Dena.CodeAnalysis.Testing
{
    // ReSharper disable once InconsistentNaming
    public static class MSBuidLocatorInitializer
    {
        private static bool _registered;
        private static object _registeredLock = new object();


        public static void Setup()
        {
            lock (_registeredLock)
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