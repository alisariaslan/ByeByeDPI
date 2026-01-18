using ByeByeDPI.Constants;
using ByeByeDPI.Core;
using ByeByeDPI.Models;
using ByeByeDPI.Services;
using ByeByeDPI.Utils;
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
            _viewModel.OnMessage += MessageWriteLine;

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

            _viewModel.LoadData();

            ThemeHelper.ApplySavedThemeToForm(this, poisonStyleManager1);

            UpdateStage(FormStage.Toggle);

            this.Text = $"{Application.ProductName}";

            this.Load += MainForm_Load;
            this.Shown += MainForm_Shown;
            this.Resize += MainForm_Resize;
            this.Move += MainForm_Move;
            this.Deactivate += (s, e) => MainFormDeactivated();

            _layoutManager = new FormLayoutManager(this);
        }

        private bool IsFirtTime => string.IsNullOrWhiteSpace(SettingsLoader.Current.ChosenParam);
        private bool IsRunning => _viewModel.IsGoodbyeDPIRunning;


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



        private void UpdateStage(FormStage stage)
        {
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

        private void UpdateTheme(FormStage stage)
        {
            parrotCircleProgressBar1.FilledColor = AppColors.MainColor;

            royalEllipseButton1_toggle.BackColor = AppColors.GetFormColor();
            royalEllipseButton1_toggle.ForeColor = royalEllipseButton1_toggle.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton1_toggle.HotTrackColor = AppColors.GetBackControlColor();
            royalEllipseButton1_toggle.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton1_toggle.BorderColor = royalEllipseButton1_toggle.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            royalEllipseButton2_reset.BackColor = AppColors.GetFormColor();
            royalEllipseButton2_reset.ForeColor = royalEllipseButton2_reset.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton2_reset.HotTrackColor = AppColors.GetBackControlColor();
            royalEllipseButton2_reset.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton2_reset.BorderColor = royalEllipseButton2_reset.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            royalEllipseButton3_apply.BackColor = AppColors.GetFormColor();
            royalEllipseButton3_apply.ForeColor = royalEllipseButton3_apply.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton3_apply.HotTrackColor = AppColors.GetBackControlColor();
            royalEllipseButton3_apply.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton3_apply.BorderColor = royalEllipseButton3_apply.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            royalEllipseButton4_next.BackColor = AppColors.GetFormColor();
            royalEllipseButton4_next.ForeColor = royalEllipseButton4_next.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();
            royalEllipseButton4_next.HotTrackColor = AppColors.GetBackControlColor();
            royalEllipseButton4_next.PressedColor = AppColors.GetButtonActiveColor();
            royalEllipseButton4_next.BorderColor = royalEllipseButton4_next.Enabled ? AppColors.GetForeColor() : AppColors.GetAlternateColor();

            listBox1_results.BackColor = AppColors.GetBackColor();
            listBox1_results.ForeColor = AppColors.GetForeColor();


            if (stage == FormStage.Toggle)
            {

                // --- Toggle text / renk mantığı
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

        private void MessageWriteLine(string msg)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => MessageWriteLine(msg)));
                return;
            }

            listBox1_results.Items.Add(msg);
            listBox1_results.TopIndex = listBox1_results.Items.Count - 1;
        }


        public void ResetFormPositionAndSize() => _layoutManager.ResetToDefault();

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
    }
}
