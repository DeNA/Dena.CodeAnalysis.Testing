using Microsoft.Build.Locator;

namespace TestRoslynRunner.Tests
{
    public class MsBuidLocatorInitializer
    {
        private static bool registered;
        private static object registeredLock = new object();

        public static void Setup()
        {
            lock (registeredLock)
            {
                if (registered)
                {
                    return;
                }
                registered = true;
            }

            // MSBuildLocatorが何をしているのかはまだ詳しくわかっていない
            // standaloneのtempleteだと、標準入力を求められる可能性があったので、標準入力を求められないAPIに変更した
            MSBuildLocator.RegisterDefaults();
        }
    }
}
