using ByeByeDPI.Constants;
using ByeByeDPI.Core;
using ByeByeDPI.Html;
using ByeByeDPI.Models;
using ByeByeDPI.Services;
using ByeByeDPI.Utils;
using ByeByeDPI.Views;
using ReaLTaiizor.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI
{
    public partial class MainForm : PoisonForm
    {
        private readonly TrayApplicationContext _trayApplicationContext;
        private readonly MainFormViewModel _viewModel;
        private FormLayoutManager _layoutManager;
        private bool _suppressAutoHide = false;

        public MainForm(TrayApplicationContext trayApplicationContext)
        {
            InitializeComponent();

            _trayApplicationContext = trayApplicationContext;
            _viewModel = new MainFormViewModel(trayApplicationContext);
            _viewModel.OnMessage += message =>
            {
                listBox1_results.Items.Add(message);
                listBox1_results.TopIndex = listBox1_results.Items.Count - 1;
            };

            _viewModel.OnStageChanged += stage =>
            {
                RunOnSafeUI(() => UpdateStage(stage));
            };

            _viewModel.OnProgressChanged += (current, total) =>
            {
                RunOnSafeUI(() => UpdateProgressBar(current, total));
            };

            _viewModel.OnClearRequested += () =>
            {
                RunOnSafeUI(() => listBox1_results.Items.Clear());
            };

            _viewModel.OnStatusChanged += newStatus =>
            {
                RunOnSafeUI(() => toolStripStatusLabel1.Text = newStatus);
            };

            _viewModel.LoadData();

            UpdateStage(FormStage.Toggle);

            this.Text = $"{Application.ProductName}";

            this.Load += MainForm_Load;
            this.Shown += MainForm_Shown;
            this.Resize += MainForm_Resize;
            this.Move += MainForm_Move;
            this.Deactivate += (s, e) => MainFormDeactivated();

            _layoutManager = new FormLayoutManager(this);
        }

        public void ResetFormPositionAndSize() => _layoutManager.ResetToDefault();
        private bool IsFirtTime => string.IsNullOrWhiteSpace(SettingsLoader.Current.ChosenParam);
        private bool IsRunning => _viewModel.IsGoodbyeDPIRunning;

        /// <summary>
        /// Safely runs an action on the UI thread
        /// </summary>
        /// <param name="action"></param>
        private void RunOnSafeUI(Action action)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        #region FORM LIFECYCLE
        private async void MainForm_Load(object sender, EventArgs e)
        {
            _layoutManager.OnLoad();
            ApplyFormBehaviorSettings();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            _layoutManager.OnShown();
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            _layoutManager?.OnResize();
        }
        private void MainForm_Move(object sender, EventArgs e)
        {
            _layoutManager?.OnMove();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _layoutManager.OnClosing();
            if (_trayApplicationContext.ApplicationExiting)
            {
                e.Cancel = true;
                return;
            }
            if (!SettingsLoader.Current.HideToTray || TempConfigLoader.Current.AdminPriviligesRequested)
            {
                _trayApplicationContext.ExitApplication();
            }
            else
            {
                e.Cancel = true;
                _layoutManager.OnClosing();
                _trayApplicationContext.HideMainWindow();
            }
        }
        private void MainFormDeactivated()
        {
            if (_suppressAutoHide)
                return;
            if (!SettingsLoader.Current.AutoHideWhenUnfocus || !_layoutManager.IsLoaded)
                return;
            BeginInvoke(new MethodInvoker(async () =>
            {
                await System.Threading.Tasks.Task.Delay(150); // Biraz daha toleranslı bir süre
                if (!NativeMethods.IsCurrentProcessFocused())
                {
                    _trayApplicationContext.HideMainWindow();
                }
            }));
        }
        #endregion

        /// <summary>
        /// Updates the progress bar based on current and total values
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        private void UpdateProgressBar(int current, int total)
        {
            if (total <= 0) return;

            int percent = (int)Math.Round((double)current / total * 100);

            if (parrotCircleProgressBar1.InvokeRequired)
            {
                parrotCircleProgressBar1.BeginInvoke(new Action(() =>
                {
                    parrotCircleProgressBar1.Percentage = percent;
                }));
            }
            else
            {
                parrotCircleProgressBar1.Percentage = percent;
            }
        }

        /// <summary>
        /// Refreshes the current stage of the form
        /// </summary>
        public void RefreshStage()
        {
            var latestStage = _viewModel.CurrentStage;
            UpdateStage(latestStage);
        }

        /// <summary>
        /// Updates the form's UI based on the current stage
        /// </summary>
        /// <param name="stage"></param>
        private void UpdateStage(FormStage stage)
        {
            toolStripStatusLabel1.Text = "";

            royalEllipseButton1_toggle.Visible = false;
            parrotCircleProgressBar1.Visible = false;
            listBox1_results.Visible = false;

            royalEllipseButton2_reset.Enabled = false;
            royalEllipseButton3_apply.Enabled = false;
            royalEllipseButton4_next.Enabled = false;

            parrotCircleProgressBar1.IsAnimated = false;

            if (stage == FormStage.Toggle)
            {
                royalEllipseButton1_toggle.Visible = true;
                royalEllipseButton2_reset.Enabled = true;

                if (IsFirtTime && !IsRunning)
                {
                    royalEllipseButton1_toggle.Text = "Install";
                }
                else
                {
                    if (IsRunning)
                    {
                        royalEllipseButton1_toggle.Text = "Stop";
                    }
                    else
                    {
                        royalEllipseButton1_toggle.Text = "Start";
                    }
                }
            }
            else if (stage == FormStage.Loading)
            {
                parrotCircleProgressBar1.Visible = true;
                parrotCircleProgressBar1.Percentage = 0;
                parrotCircleProgressBar1.IsAnimated = true;
            }
            else if (stage == FormStage.Result)
            {
                listBox1_results.Visible = true;

                royalEllipseButton2_reset.Enabled = true;
                royalEllipseButton3_apply.Enabled = true;
                royalEllipseButton4_next.Enabled = true;
            }
            UpdateTheme(stage);
        }

        /// <summary>
        /// Updates the form's theme based on the current stage
        /// </summary>
        /// <param name="stage"></param>
        private void UpdateTheme(FormStage stage)
        {
            ThemeHelper.ApplySavedThemeToForm(this, poisonStyleManager1);

            parrotCircleProgressBar1.FilledColor = AppColors.MainColor;

            royalEllipseButton1_toggle.BackColor = AppColors.GetFormColor();
            royalEllipseButton1_toggle.ForeColor = royalEllipseButton1_toggle.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton1_toggle.HotTrackColor = AppColors.GetButtonBackColor();
            royalEllipseButton1_toggle.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton1_toggle.BorderColor = royalEllipseButton1_toggle.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            royalEllipseButton2_reset.BackColor = AppColors.GetFormColor();
            royalEllipseButton2_reset.ForeColor = royalEllipseButton2_reset.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton2_reset.HotTrackColor = AppColors.GetButtonBackColor();
            royalEllipseButton2_reset.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton2_reset.BorderColor = royalEllipseButton2_reset.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            royalEllipseButton3_apply.BackColor = AppColors.GetFormColor();
            royalEllipseButton3_apply.ForeColor = royalEllipseButton3_apply.Enabled ? AppColors.MainColor : AppColors.GetAlternateColor();
            royalEllipseButton3_apply.HotTrackColor = AppColors.GetButtonBackColor();
            royalEllipseButton3_apply.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton3_apply.BorderColor = royalEllipseButton3_apply.Enabled ? AppColors.MainColor : AppColors.GetAlternateColor();

            royalEllipseButton4_next.BackColor = AppColors.GetFormColor();
            royalEllipseButton4_next.ForeColor = royalEllipseButton4_next.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton4_next.HotTrackColor = AppColors.GetButtonBackColor();
            royalEllipseButton4_next.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton4_next.BorderColor = royalEllipseButton4_next.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            listBox1_results.BackColor = AppColors.GetBackColor();
            listBox1_results.ForeColor = AppColors.GetForeColor();

            if (stage == FormStage.Toggle)
            {
                if (IsFirtTime && !IsRunning)
                {
                    royalEllipseButton1_toggle.ForeColor = AppColors.GetForeColor();
                    royalEllipseButton1_toggle.BorderColor = AppColors.GetForeColor();
                }
                else
                {
                    if (IsRunning)
                    {
                        royalEllipseButton1_toggle.ForeColor = Color.LimeGreen;
                        royalEllipseButton1_toggle.BorderColor = Color.LimeGreen;
                    }
                    else
                    {
                        royalEllipseButton1_toggle.ForeColor = Color.Red;
                        royalEllipseButton1_toggle.BorderColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the GoodbyeDPI state when the toggle button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void royalEllipseButton1_toggle_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;

            try
            {
                var toggleResult = await _viewModel.ToggleGoodbyeDPIAsync();
                if (toggleResult == -1)
                {
                    MessageBox.Show("Unfortunately, no parameter in the list could establish a connection.", "No Result Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (toggleResult == 1)
                {
                    _viewModel.SetCurrentStage(FormStage.Result);
                    return;
                }
                else if (toggleResult == 2)
                {
                    MessageBox.Show("Looks like your dpi settings lost. Returning to Installation sequence.");
                }
                _viewModel.SetCurrentStage(FormStage.Toggle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Toggle failed: {ex}");
                MessageBox.Show(
                    "An error occurred while toggle dpi.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                _viewModel.SetCurrentStage(FormStage.Toggle);
            }
        }

        /// <summary>
        /// Closes dpi, then resets all parameter settings when the reset button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void royalEllipseButton2_reset_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;
            var fullConfirm = MessageBox.Show(
                "All settings will be reset. Are you sure?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (fullConfirm != DialogResult.Yes)
                return;
            try
            {
                await _viewModel.ResetWorkflowAsync();
                _viewModel.SetCurrentStage(FormStage.Toggle);
                Debug.WriteLine("🧹 All settings have been reset.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Reset failed: {ex}");
                MessageBox.Show(
                    "An error occurred while resetting parameters.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Applies the current parameter when the apply button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void royalEllipseButton3_apply_Click(object sender, EventArgs e)
        {
            var (reachable, unreachable) = _viewModel.GetAccessibilityStats();
            if (unreachable > reachable)
            {
                var result = MessageBox.Show(
                    $"Number of unreachable sites ({unreachable}) " +
                    $"is higher than reachable sites ({reachable}).\n\n" +
                    "This parameter may negatively affect internet access.\n\n" +
                    "Do you still want to apply it?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;
            }
            try
            {
                await _viewModel.ApplyCurrentParameter();
                _viewModel.SetCurrentStage(FormStage.Toggle);
                Debug.WriteLine($"Apply success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Apply failed: {ex}");
                MessageBox.Show(
                    "An error occurred while applying the parameter.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Next parameter testing when the next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void royalEllipseButton4_next_Click(object sender, EventArgs e)
        {
            if (_viewModel.CurrentStage == FormStage.Loading)
                return;
            try
            {
                var noMoreToTest = await _viewModel.TestNextParameterAsync();
                if (noMoreToTest)
                {
                    MessageBox.Show("All parameters have been tested.", "Scan Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _viewModel.SetCurrentStage(FormStage.Toggle);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Next failed: {ex}");
                MessageBox.Show(
                    "An error occurred while testing the next parameter.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHtmlDialog(AboutHtml.GetTitle(), AboutHtml.GetHtml());
        }

        private void ShowHtmlDialog(string title, string html)
        {
            _suppressAutoHide = true;
            using (var dlg = new WebDialog(title, html))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.TopMost = true;
                dlg.ShowDialog(this);
            }
            _suppressAutoHide = false;
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _suppressAutoHide = true;
            using (var f = new SettingsForm(this))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.TopMost = true;
                f.ShowDialog(this); // parent VERME
            }
            _suppressAutoHide = false;
        }

        private async void checkUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            checkUpdateToolStripMenuItem1.Enabled = false;
            try
            {
                var updateService = _trayApplicationContext.GetUpdateService();
                var updateWrapper = await updateService.CheckForUpdateAsync(false);

                if (!updateWrapper.Success)
                {
                    MessageBox.Show(
                        $"Failed to check updates:\n{updateWrapper.ErrorMessage}",
                        "Update Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (updateWrapper.UpdateInfo != null)
                {
                    var info = updateWrapper.UpdateInfo;
                    var result = MessageBox.Show(
                        $"New version v{info.Version} ({info.BuildNumber}) is available!\n\nNotes: {info.Notes}\n\nDownload now?",
                        "Update Found",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                        await updateService.DownloadAndRunUpdateAsync();
                }
                else
                {
                    MessageBox.Show(
                        "Your application is up to date.",
                        "No Update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unexpected error:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                checkUpdateToolStripMenuItem1.Enabled = true;
            }
        }

        public void ApplyFormBehaviorSettings(bool showWarnings = false)
        {
            this.TopMost = SettingsLoader.Current.AlwaysTopMost;
            this.ShowInTaskbar = SettingsLoader.Current.ShowInTaskbar;
            if (SettingsLoader.Current.AutoHideWhenUnfocus && showWarnings)
            {
                MessageBox.Show(
                    "Auto-hide is enabled. App will temporarily minimize when it loses focus.",
                    "Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            _suppressAutoHide = false;
        }

        private void logsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowHtmlDialog(UnderDevelopmentHtml.GetTitle(), UnderDevelopmentHtml.GetHtml("Logs"));
        }

        private void paramsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
          "⚠️ You are about to open the parameter file.\n" +
          "Be careful when editing it, incorrect values may break the application.\n\n" +
          "Do you want to continue?",
          "Warning - Edit Params Carefully",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Warning
      );

            if (result == DialogResult.Yes)
            {
                FileOpener.OpenFile(AppConstants.ParamsPath);
            }
        }

        private async void flushDNCCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;
            var result = await NetworkHelper.FlushDNSAsync();
            MessageBox.Show(result.Message, "Operation Summary", MessageBoxButtons.OK,
                            result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void applyGoogleDNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;
            var result = await NetworkHelper.ApplyGoogleDNSAsync();
            MessageBox.Show(result.Message, "Operation Summary", MessageBoxButtons.OK,
                            result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void superonlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;
            try
            {
                _viewModel.SetCurrentStage(FormStage.Loading);
                await NetworkHelper.FlushDNSAsync();
                await NetworkHelper.ApplyGoogleDNSAsync();
                bool success = await _viewModel.TestParamByNameAsync("mode5");
                _viewModel.SetCurrentStage(success ? FormStage.Result : FormStage.Toggle);
            }
            catch
            {
                _viewModel.SetCurrentStage(FormStage.Toggle);
            }
        }

        private async void resetDNSConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(await PrivilegesHelper.EnsureAdministrator()))
                return;
            var result = await NetworkHelper.SetDNSToAutomaticAsync();
            MessageBox.Show(result.Message, "Operation Summary", MessageBoxButtons.OK,
                            result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHtmlDialog(HelpHTML.GetTitle(), HelpHTML.GetHtml());
        }

        private void openDomainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
"⚠️ You are about to open the domains file.\n" +
"Be careful when editing it, incorrect values may break the application.\n\n" +
"Do you want to continue?",
"Warning - Edit Domains Carefully",
MessageBoxButtons.YesNo,
MessageBoxIcon.Warning
);

            if (result == DialogResult.Yes)
            {
                FileOpener.OpenFile(AppConstants.DomainListPath);
            }
        }
    }
}
