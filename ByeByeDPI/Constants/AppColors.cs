using ByeByeDPI.Utils;
using ReaLTaiizor.Enum.Poison;
using System.Drawing;

namespace ByeByeDPI.Constants
{
    internal static class AppColors
    {
        // ----- Shared -----
        public static ColorStyle MainStyle { get; } = ColorStyle.Orange;
        public static Color MainColor { get; } = Color.DarkOrange;

        // ----- Light Theme -----
        public static Color LightFormColor { get; } = Color.White;
        public static Color LightBackColor { get; } = Color.White;
        public static Color LightForeColor { get; } = Color.Black;
        public static Color LightAlternateColor { get; } = Color.FromArgb(230, 245, 255); // Zebra
        public static Color LightLineNumberBackground { get; } = Color.FromArgb(220, 220, 220);
        public static Color LightButtonActive { get; } = Color.Orange;
        public static Color LightSelection { get; } = Color.LightSkyBlue;

        // ----- Dark Theme -----
        public static Color DarkFormColor { get; } = Color.FromArgb(17, 17, 17);
        public static Color DarkBackColor { get; } = Color.FromArgb(30, 30, 30);
        public static Color DarkForeColor { get; } = Color.White;
        public static Color DarkAlternateColor { get; } = Color.FromArgb(35, 35, 35); // Zebra
        public static Color DarkLineNumberBackground { get; } = Color.FromArgb(50, 50, 50);
        public static Color DarkButtonActive { get; } = Color.DarkOrange;
        public static Color DarkSelection { get; } = Color.DodgerBlue;

        // ----- Highlight / Selection -----
        public static Color HighlightColor { get; } = Color.Yellow;
        public static Color SelectedHighlightColor { get; } = Color.Gold;


        // ----- Helper Properties -----
        public static bool IsDark => GetTheme() == ThemeStyle.Dark;

        public static Color GetFormColor() => IsDark ? DarkFormColor : LightFormColor;
        public static Color GetBackColor() => IsDark ? DarkBackColor : LightBackColor;
        public static Color GetForeColor() => IsDark ? DarkForeColor : LightForeColor;
        public static Color GetAlternateColor() => IsDark ? DarkAlternateColor : LightAlternateColor;
        public static Color GetBackControlColor() => IsDark ? DarkLineNumberBackground : LightBackColor;
        public static Color GetButtonActiveColor() => IsDark ? DarkButtonActive : LightButtonActive;
        public static Color GetSelectionColor() => IsDark ? DarkSelection : LightSelection;
        public static Color GetLineNumberBackground() => IsDark ? DarkLineNumberBackground : LightLineNumberBackground;

        public static Color GetHighlightColor(bool selected) => selected ? SelectedHighlightColor : HighlightColor;

        private static ThemeStyle GetTheme() => ThemeHelper.GetTheme();
    }
}
