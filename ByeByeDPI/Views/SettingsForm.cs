using ByeByeDPI.Constants;
using ByeByeDPI.Core;
using ByeByeDPI.Utils;
using ReaLTaiizor.Enum.Poison;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Views
{
    public partial class SettingsForm : PoisonForm
    {
        private MainForm _mainForm;
        private Keys _pendingHotkeyModifiers;
        private Keys _pendingHotkeyKey;

        public SettingsForm(MainForm mainForm)
        {
            InitializeComponent();

            ApplyTheme();
            UpdateThemeDropdownText();

            _mainForm = mainForm;

            RemoveSettingEvents();

            //GENERAL
            poisonToggle1_checkUpdates.Checked = SettingsLoader.Current.CheckUpdates;
            poisonToggle1_startWithWindows.Checked = SettingsLoader.Current.StartWithWindows;
            poisonToggle2_autoRun.Checked = SettingsLoader.Current.AutoRunGoodbyeDPI;
            //APPEARANCE
            poisonToggle1_alwaysTopMost.Checked = SettingsLoader.Current.AlwaysTopMost;
            poisonToggle1_showInTaskbar.Checked = SettingsLoader.Current.ShowInTaskbar;
            //BEHAVIOUR
            poisonToggle1_hideToSystemTray.Checked = SettingsLoader.Current.HideToTray;
            poisonToggle1_autoHide.Checked = SettingsLoader.Current.AutoHideWhenUnfocus;
            //HOTKEY
            poisonToggle1_globalHotkeys.Checked = SettingsLoader.Current.EnableGlobalHotkey;
            poisonTextBox1_showWindowHotkey.Text = FormatHotkey(SettingsLoader.Current.HotkeyModifiers, SettingsLoader.Current.HotkeyKey);
            poisonTextBox1_showWindowHotkey.Enabled = SettingsLoader.Current.EnableGlobalHotkey;
            _pendingHotkeyKey = SettingsLoader.Current.HotkeyKey;
            _pendingHotkeyModifiers = SettingsLoader.Current.HotkeyModifiers;

            AddSettingEvents();
        }

        private void RemoveSettingEvents()
        {
            //GENERAL
            poisonToggle1_checkUpdates.CheckedChanged -= poisonToggle1_checkUpdates_CheckedChanged;
            poisonToggle1_startWithWindows.CheckedChanged -= poisonToggle1_startWithWindows_CheckedChanged;
            poisonToggle2_autoRun.CheckedChanged -= poisonToggle2_autoRun_CheckedChanged;
            //APPEARANCE
            poisonToggle1_alwaysTopMost.CheckedChanged -= poisonToggle1_alwaysTopMost_CheckedChanged;
            poisonToggle1_showInTaskbar.CheckedChanged -= poisonToggle1_showInTaskbar_CheckedChanged;
            //BEHAVIOUR
            poisonToggle1_hideToSystemTray.CheckedChanged -= poisonToggle1_hideToSystemTray_CheckedChanged;
            poisonToggle1_autoHide.CheckedChanged -= poisonToggle1_autoHide_CheckedChanged;
            //HOTKEY
            poisonToggle1_globalHotkeys.CheckedChanged -= poisonToggle1_globalHotkeys_CheckedChanged;
            poisonTextBox1_showWindowHotkey.KeyDown -= poisonTextBox1_showWindowHotkey_KeyDown;
        }

        private void AddSettingEvents()
        {
            //GENERAL
            poisonToggle1_checkUpdates.CheckedChanged += poisonToggle1_checkUpdates_CheckedChanged;
            poisonToggle1_startWithWindows.CheckedChanged += poisonToggle1_startWithWindows_CheckedChanged;
            poisonToggle2_autoRun.CheckedChanged += poisonToggle2_autoRun_CheckedChanged;
            //APPEARANCE
            poisonToggle1_alwaysTopMost.CheckedChanged += poisonToggle1_alwaysTopMost_CheckedChanged;
            poisonToggle1_showInTaskbar.CheckedChanged += poisonToggle1_showInTaskbar_CheckedChanged;
            //BEHAVIOUR
            poisonToggle1_hideToSystemTray.CheckedChanged += poisonToggle1_hideToSystemTray_CheckedChanged;
            poisonToggle1_autoHide.CheckedChanged += poisonToggle1_autoHide_CheckedChanged;
            //HOTKEY
            poisonToggle1_globalHotkeys.CheckedChanged += poisonToggle1_globalHotkeys_CheckedChanged;
            poisonTextBox1_showWindowHotkey.KeyDown += poisonTextBox1_showWindowHotkey_KeyDown;
        }

        private async void poisonButton1_resetDefaults_Click(object sender, EventArgs e)
        {
            //if (!await PrivilegesHelper.EnsureAdministrator())
            //    return;

            var result = MessageBox.Show(
      "Are you sure you want to reset all settings to default?",
      "Confirm Reset",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Warning
  );

            if (result != DialogResult.Yes)
                return;

            var def = new SettingsModel();

            RemoveSettingEvents();

            //GENERAL
            poisonToggle1_checkUpdates.Checked = def.CheckUpdates;
            poisonToggle1_startWithWindows.Checked = def.StartWithWindows;
            poisonToggle2_autoRun.Checked = def.AutoRunGoodbyeDPI;
            //APPEARANCE
            poisonToggle1_alwaysTopMost.Checked = def.AlwaysTopMost;
            poisonToggle1_showInTaskbar.Checked = def.ShowInTaskbar;
            //BEHAVIOUR
            poisonToggle1_hideToSystemTray.Checked = def.HideToTray;
            poisonToggle1_autoHide.Checked = def.AutoHideWhenUnfocus;
            //HOTKEY
            poisonToggle1_globalHotkeys.Checked = def.EnableGlobalHotkey;
            poisonTextBox1_showWindowHotkey.Text = FormatHotkey(def.HotkeyModifiers, def.HotkeyKey);
            poisonTextBox1_showWindowHotkey.Enabled = def.EnableGlobalHotkey;

            try
            {
                await _mainForm.ViewModel.TaskService.SetStartWithWindowsAsync(def.StartWithWindows);
                await _mainForm.ViewModel.TaskService.SetAutoRunOnLoginAsync(def.AutoRunGoodbyeDPI);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to update registry for StartWithWindows or AutoRunOnLogin",
                    MessageBoxButtons.OK);
            }

            AddSettingEvents();
            SettingsLoader.Current = def;
            SettingsLoader.Save();
            TrayApplicationContext.Instance?.ReloadGlobalHotkey();
            MessageBox.Show("Settings have been reset to default.", "Defaults", MessageBoxButtons.OK);
        }

        #region GENERAL
        private void poisonToggle1_checkUpdates_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.CheckUpdates = poisonToggle1_checkUpdates.Checked;
            SettingsLoader.Save();
            _mainForm.ApplyFormBehaviorSettings(true);
        }


        #endregion

        #region APPARANCE
        private void poisonToggle1_alwaysTopMost_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.AlwaysTopMost = poisonToggle1_alwaysTopMost.Checked;
            SettingsLoader.Save();
            _mainForm.ApplyFormBehaviorSettings(true);
        }
        private void poisonToggle1_showInTaskbar_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.ShowInTaskbar = poisonToggle1_showInTaskbar.Checked;
            SettingsLoader.Save();
            _mainForm.ApplyFormBehaviorSettings(true);
            this.Close();
        }
        #endregion

        #region BEHAVIOURS
        private void poisonToggle1_hideToSystemTray_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.HideToTray = poisonToggle1_hideToSystemTray.Checked;
            SettingsLoader.Save();
            _mainForm.ApplyFormBehaviorSettings(true);
        }
        private void poisonToggle1_autoHide_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.AutoHideWhenUnfocus = poisonToggle1_autoHide.Checked;
            SettingsLoader.Save();
            _mainForm.ApplyFormBehaviorSettings(true);
        }
        #endregion

        #region HOTKEYS
        private void poisonToggle1_globalHotkeys_CheckedChanged(object sender, EventArgs e)
        {
            SettingsLoader.Current.EnableGlobalHotkey = poisonToggle1_globalHotkeys.Checked;
            poisonTextBox1_showWindowHotkey.Enabled = poisonToggle1_globalHotkeys.Checked;
            SettingsLoader.Save();
            TrayApplicationContext.Instance?.ReloadGlobalHotkey();
        }
        private void poisonTextBox1_showWindowHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            var key = e.KeyCode;
            var mods = NormalizeModifiers(e.Modifiers);
            if (IsModifierKey(key))
                return;
            if (mods == Keys.None)
            {
                MessageBox.Show("You must use at least one modifier key (Ctrl, Alt, Shift, or Win).", "Invalid Shortcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _pendingHotkeyKey = key;
            _pendingHotkeyModifiers = mods;
            SettingsLoader.Current.HotkeyKey = _pendingHotkeyKey;
            SettingsLoader.Current.HotkeyModifiers = _pendingHotkeyModifiers;
            SettingsLoader.Current.EnableGlobalHotkey = true;
            poisonToggle1_globalHotkeys.Checked = true;
            poisonTextBox1_showWindowHotkey.Text = FormatHotkey(_pendingHotkeyModifiers, _pendingHotkeyKey);
            SettingsLoader.Save();
            TrayApplicationContext.Instance?.ReloadGlobalHotkey();
        }
        private string FormatHotkey(Keys modifiers, Keys key)
        {
            var parts = new System.Collections.Generic.List<string>();
            if (modifiers.HasFlag(Keys.Control)) parts.Add("Ctrl");
            if (modifiers.HasFlag(Keys.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(Keys.Alt)) parts.Add("Alt");
            if (modifiers.HasFlag(Keys.LWin) || modifiers.HasFlag(Keys.RWin)) parts.Add("Win");
            if (key != Keys.None)
            {
                parts.Add(key.ToString());
            }
            return string.Join("+", parts);
        }
        private Keys NormalizeModifiers(Keys modifiers)
        {
            Keys result = Keys.None;
            if (modifiers.HasFlag(Keys.Control)) result |= Keys.Control;
            if (modifiers.HasFlag(Keys.Shift)) result |= Keys.Shift;
            if (modifiers.HasFlag(Keys.Alt)) result |= Keys.Alt;
            if (modifiers.HasFlag(Keys.LWin)) result |= Keys.LWin;
            if (modifiers.HasFlag(Keys.RWin)) result |= Keys.RWin;
            return result;
        }
        private bool IsModifierKey(Keys key)
        {
            return key == Keys.ControlKey || key == Keys.ShiftKey || key == Keys.Menu || key == Keys.LWin || key == Keys.RWin;
        }
        #endregion

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        #region MANUAL RESIZE (BORDERLESS SUPPORT)
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HTCLIENT)
                {
                    Point cursor = PointToClient(Cursor.Position);

                    IntPtr hit = ResizeHitTestHelper.GetHitTest(this, cursor, 8);
                    if (hit != IntPtr.Zero)
                    {
                        m.Result = hit;
                        return;
                    }
                }
                return;
            }

            base.WndProc(ref m);
        }
        #endregion

        #region THEMES
        private void UpdateThemeDropdownText()
        {
            poisonDropDownButton1_selectTheme.Text = ThemeHelper.GetTheme().ToString();
        }
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThemeHelper.SaveTheme(ThemeStyle.Default);
            ApplyTheme();
            _mainForm.RefreshStage();
            UpdateThemeDropdownText();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThemeHelper.SaveTheme(ThemeStyle.Light);
            ApplyTheme();
            _mainForm.RefreshStage();
            UpdateThemeDropdownText();
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThemeHelper.SaveTheme(ThemeStyle.Dark);
            ApplyTheme();
            _mainForm.RefreshStage();
            UpdateThemeDropdownText();
        }
        #endregion

        private void ApplyTheme()
        {
            ThemeHelper.ApplySavedThemeToForm(this, poisonStyleManager1);
            var mainStyle = AppColors.MainStyle;
            poisonTabControl1.Style = mainStyle;

            poisonTile1_checkUpdates.Style = mainStyle;
            poisonLabel1_checkUpdates.Style = mainStyle;
            poisonToggle1_checkUpdates.Style = mainStyle;

            poisonTile1_startWithWindows.Style = mainStyle;
            poisonLabel1_startWithWindows.Style = mainStyle;
            poisonToggle1_startWithWindows.Style = mainStyle;

            poisonTile1_hideToSystemTray.Style = mainStyle;
            poisonLabel1_hideToSystemTray.Style = mainStyle;
            poisonToggle1_hideToSystemTray.Style = mainStyle;

            poisonTile1_autoHide.Style = mainStyle;
            poisonLabel1_autoHide.Style = mainStyle;
            poisonToggle1_autoHide.Style = mainStyle;

            poisonTile1_alwaysTopMost.Style = mainStyle;
            poisonLabel1_alwaysTopMost.Style = mainStyle;
            poisonToggle1_alwaysTopMost.Style = mainStyle;

            poisonTile1_showInTaskbar.Style = mainStyle;
            poisonLabel1_showInTaskbar.Style = mainStyle;
            poisonToggle1_showInTaskbar.Style = mainStyle;

            poisonTile1_theme.Style = mainStyle;
            poisonLabel1_theme.Style = mainStyle;
            poisonDropDownButton1_selectTheme.Style = mainStyle;

            poisonTile1_globalHotkey.Style = mainStyle;
            poisonLabel1_globalHotkey.Style = mainStyle;
            poisonToggle1_globalHotkeys.Style = mainStyle;

            poisonTile1_showWindowHotkey.Style = mainStyle;
            poisonLabel1_showWindowHotkey.Style = mainStyle;
            poisonTextBox1_showWindowHotkey.Style = mainStyle;

            poisonTile1_autoRun.Style = mainStyle;
            poisonLabel1_autoRun.Style = mainStyle;
            poisonToggle2_autoRun.Style = mainStyle;

        }

        private void poisonDropDownButton1_selectTheme_Click(object sender, EventArgs e)
        {
            poisonDropDownButton1_selectTheme.OpenDropDown();

        }

        private void RevertStartWithWindows()
        {
            poisonToggle1_startWithWindows.CheckedChanged -= poisonToggle1_startWithWindows_CheckedChanged;
            poisonToggle1_startWithWindows.Checked = !poisonToggle1_startWithWindows.Checked;
            poisonToggle1_startWithWindows.CheckedChanged += poisonToggle1_startWithWindows_CheckedChanged;
        }

        private async void poisonToggle1_startWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
            {
                RevertStartWithWindows();
                return;
            }
            try
            {
                await _mainForm.ViewModel.TaskService.SetStartWithWindowsAsync(poisonToggle1_startWithWindows.Checked);
                SettingsLoader.Current.StartWithWindows = poisonToggle1_startWithWindows.Checked;
                SettingsLoader.Save();
                _mainForm.ApplyFormBehaviorSettings(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                ex.Message,
                    "Failed to update application startup (Registry)",
                    MessageBoxButtons.OK);
                RevertStartWithWindows();
            }
        }

        private void RevertAutoRun()
        {
            poisonToggle2_autoRun.CheckedChanged -= poisonToggle2_autoRun_CheckedChanged;
            poisonToggle2_autoRun.Checked = !poisonToggle2_autoRun.Checked;
            poisonToggle2_autoRun.CheckedChanged += poisonToggle2_autoRun_CheckedChanged;
        }

        private async void poisonToggle2_autoRun_CheckedChanged(object sender, EventArgs e)
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
            {
                RevertAutoRun();
                return;
            }
            try
            {
                await _mainForm.ViewModel.TaskService.SetAutoRunOnLoginAsync(
              poisonToggle2_autoRun.Checked
          );

                SettingsLoader.Current.AutoRunGoodbyeDPI = poisonToggle2_autoRun.Checked;
                SettingsLoader.Save();
                _mainForm.ApplyFormBehaviorSettings(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating auto-run setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RevertAutoRun();
            }

        }

    }
}
