using Microsoft.Win32;

namespace BlackTitlebar
{
    public static class RegUtils
    {
        internal const int Black = unchecked((int) 0xFF000000);
        internal const int Fallback = 0x0;
        private const string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM";
        private const int ColoredTitlebar = 0x1;
        private const int WhiteTitlebar = 0x0;
        internal static void SetAccentColor(object hex)
        {
            Registry.SetValue(KeyPath, "AccentColor", hex, RegistryValueKind.DWord);
        }

        internal static void SetInactiveAccentColor(object hex)
        {
            Registry.SetValue(KeyPath, "AccentColorInactive", hex, RegistryValueKind.DWord);
        }

        internal static void SetColorPrevalence(bool enable)
        {
            Registry.SetValue(KeyPath, "ColorPrevalence", enable ? ColoredTitlebar : WhiteTitlebar, RegistryValueKind.DWord);
        }
        
        internal static bool GetStatus(out long accentColor)
        {
            var colorPrevalence = int.Parse(Registry.GetValue(KeyPath, "ColorPrevalence", -1)?.ToString()!);
            var activeAccentColor = accentColor = long.Parse(Registry.GetValue(KeyPath, "AccentColor", -1)?.ToString()!);
            var isTitlebarBlack = (colorPrevalence == ColoredTitlebar && activeAccentColor == Black);
            return isTitlebarBlack;
        }
    }
}