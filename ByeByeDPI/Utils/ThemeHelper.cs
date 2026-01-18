using ByeByeDPI.Constants; // AppColors
using Microsoft.Win32;
using ReaLTaiizor.Enum.Poison;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Utils
{
    public static class ThemeHelper
    {
        public static void SaveTheme(ThemeStyle theme)
        {
            SettingsLoader.Current.SelectedTheme = theme;
            SettingsLoader.Save();
        }

        public static ThemeStyle GetTheme()
        {
            var savedTheme = SettingsLoader.Current.SelectedTheme;
            if (savedTheme == ThemeStyle.Default)
            {
                return GetSystemTheme();
            }
            return savedTheme;
        }

        public static void ApplySavedThemeToForm(PoisonForm form, PoisonStyleManager poisonStyleManager)
        {
            if (form is null) throw new Exception("Form is null");
            if (poisonStyleManager is null) throw new Exception("PoisonStyleManager is null");

            var theme = GetTheme();

            form.Theme = theme;
            poisonStyleManager.Theme = theme;
            form.Style = AppColors.MainStyle;

            switch (theme)
            {
                case ThemeStyle.Light:
                    form.BackColor = AppColors.LightBackColor;
                    form.ForeColor = AppColors.LightForeColor;
                    break;
                case ThemeStyle.Dark:
                    form.BackColor = AppColors.DarkBackColor;
                    form.ForeColor = AppColors.DarkForeColor;
                    break;
                default:
                    form.BackColor = SystemColors.Control;
                    form.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        public static void ApplyThemeToControl(Control control, ThemeStyle theme)
        {
            if (control is null) return;

            switch (theme)
            {
                case ThemeStyle.Light:
                    control.BackColor = AppColors.LightBackColor;
                    control.ForeColor = AppColors.LightForeColor;
                    break;
                case ThemeStyle.Dark:
                    control.BackColor = AppColors.DarkBackColor;
                    control.ForeColor = AppColors.DarkForeColor;
                    break;
                default:
                    control.BackColor = SystemColors.Control;
                    control.ForeColor = SystemColors.ControlText;
                    break;
            }

            foreach (Control child in control.Controls)
                ApplyThemeToControl(child, theme);
        }

        public static ThemeStyle GetSystemTheme()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object registryValue = key.GetValue("AppsUseLightTheme");
                        if (registryValue != null)
                        {
                            return (int)registryValue == 0 ? ThemeStyle.Dark : ThemeStyle.Light;
                        }
                    }
                }
            }
            catch { }

            return ThemeStyle.Light;
        }
    }
}
