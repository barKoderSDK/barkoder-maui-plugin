
#if ANDROID

namespace Plugin.Maui.Barkoder.Common
{
    static partial class BarkoderGlobal
    {
        static partial void SetCustomOptionGlobalPlatform(string option, int value)
        {
            Com.Barkoder.Barkoder.SetCustomOptionGlobal(option, value);
        }
    }
}
#endif
