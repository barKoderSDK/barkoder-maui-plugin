#if IOS
using Plugin.Maui.Barkoder.Common;
using Barkoder.Xamarin;

namespace Plugin.Maui.Barkoder.Common
{
    static partial class BarkoderGlobal
    {
        static partial void SetCustomOptionGlobalPlatform(string option, int value)
        {
            // Call directly on the class, not an instance
            BarkoderView.SetCustomOptionGlobal(option, value);
        }
    }
}
#endif





