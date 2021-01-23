using Microsoft.Build.Locator;



namespace Dena.CodeAnalysis.Testing
{
    // ReSharper disable once InconsistentNaming
    public static class MSBuidLocatorInitializer
    {
        private static bool _registered;
        private static object registeredLock = new object();


        public static void Setup()
        {
            lock (registeredLock)
            {
                if (_registered)
                {
                    return;
                }

                _registered = true;
            }

            // MSBuildLocatorが何をしているのかはまだ詳しくわかっていない
            // standaloneのtempleteだと、標準入力を求められる可能性があったので、標準入力を求められないAPIに変更した
            MSBuildLocator.RegisterDefaults();
        }
    }
}