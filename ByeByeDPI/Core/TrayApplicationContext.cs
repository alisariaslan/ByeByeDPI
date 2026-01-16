using ByeByeDPI.Constants;
using ByeByeDPI.Utils;
using System;
using System.Windows.Forms;

namespace ByeByeDPI.Core
{
	public class TrayApplicationContext : ApplicationContext
	{
        public static TrayApplicationContext Instance { get; private set; }
        public bool ApplicationExiting { get; private set; }
        private MainForm _form;
        private TrayIconManager _trayManager;
        private bool _trayMinimizedNotifyShown;
        private readonly UpdateService _updateService = new UpdateService();

        public TrayApplicationContext()
		{
			Instance = this;
            //BindEvents();
            _form = new MainForm(this);
            var forceHandleCreation = _form.Handle;
            _trayManager = new TrayIconManager(
                onShow: ShowMainWindow,
                onHide: HideMainWindow,
                onReset: ResetFormPositionAndSize,
                onExit: ExitApplication
            );
            if (SettingsLoader.Current.CheckUpdates)
                _updateService.StartPeriodicCheck(AppVersionHelper.GetBuildNumber());
            HandleInitialVisibility();
        }

        private void HandleInitialVisibility()
        {
            // Logic to determine if the app should start minimized or visible
            if (SettingsLoader.Current.HideToTray && !TempConfigLoader.Current.AdminPriviligesRequested)
                HideMainWindow();
            else
                ShowMainWindow();

            TempConfigLoader.Current.AdminPriviligesRequested = false;
            TempConfigLoader.Save();
        }

        public void ShowMainWindow()
        {
            RunOnUI(() =>
            {
                if (_form.IsDisposed) _form = new MainForm(this);
                _form.Show();
                _form.WindowState = FormWindowState.Normal;
                _form.ShowInTaskbar = SettingsLoader.Current.ShowInTaskbar;
                _form.Activate();
                _form.BringToFront();
            });
        }

        public void HideMainWindow()
        {
            if (_form == null) return;
            RunOnUI(() =>
            {
                foreach (Form owned in _form.OwnedForms)
                    if (owned != null && !owned.IsDisposed) try { owned.Close(); } catch { }

                _form.Hide();
                if (!_trayMinimizedNotifyShown)
                {
                    _trayManager.ShowNotification(AppConstants.AppName, "Minimized to tray.");
                    _trayMinimizedNotifyShown = true;
                }
            });
        }
        private void ResetFormPositionAndSize()
        {
            var cfg = TempConfigLoader.Current;
            cfg.MainFormWidth = cfg.MainFormHeight = cfg.MainFormX = cfg.MainFormY = -1;
            TempConfigLoader.Save();
            RunOnUI(() =>
            {
                _form.ResetFormPositionAndSize();
                HideMainWindow();
                ShowMainWindow();
                ScreenHelper.CenterFormManually(_form);
            });
        }

        public void ExitApplication()
        {
            if (ApplicationExiting) return;
            ApplicationExiting = true;

            // Cleanup services and resources
            _updateService.StopPeriodicCheck();
            _trayManager.Dispose();

            if (_form != null && !_form.IsDisposed)
            {
                _form.Close();
                _form.Dispose();
            }
            ExitThread();
        }

        private void RunOnUI(Action action)
        {
            if (_form == null || _form.IsDisposed) return;

            if (!_form.IsHandleCreated)
            {
                IntPtr dummy = _form.Handle;
            }

            if (_form.InvokeRequired)
            {
                _form.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        public UpdateService GetUpdateService() => _updateService;

    }
}
