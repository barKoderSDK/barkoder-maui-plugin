using System;

namespace Plugin.Maui.Barkoder.Common
{
    public static partial class BarkoderGlobal
    {
        public static void SetCustomOptionGlobal(string option, int value)
        {
            SetCustomOptionGlobalPlatform(option, value);
        }

        // Partial method; implemented per platform
        static partial void SetCustomOptionGlobalPlatform(string option, int value);
    }
}
