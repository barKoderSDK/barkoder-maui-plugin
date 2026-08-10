#if WINDOWS

using Barkoder.Windows;
using Barkoder.Windows.Decoder;

namespace Plugin.Maui.Barkoder.Common
{
    static partial class BarkoderGlobal
    {
        static partial void SetCustomOptionGlobalPlatform(string option, int value)
        {
            // Windows SDK uses per-instance custom options via BarkoderView.SetCustomOption
            // Global options are set through the BarkoderNET config when initialized
        }
    }
}
#endif
