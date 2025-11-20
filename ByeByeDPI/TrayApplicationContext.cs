using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class TrayApplicationContext : ApplicationContext
	{
		private NotifyIcon _trayIcon;
		private MainForm _form;
		private bool _updateChecksStarted;
		private bool _trayMinimizedNotifyShown;
		public bool ApplicationExiting;

		public static TrayApplicationContext Instance { get; private set; }

		public TrayApplicationContext()
		{
			Instance = this;

			_form = new MainForm(this);

			if (!_form.IsHandleCreated)
			{
				var handle = _form.Handle;
			}

			_trayIcon = new NotifyIcon()
			{
				Icon = Properties.Resources.icons8_so_so,
				Visible = true,
				Text = $"{Constants.AppName}"
			};

			var trayMenu = new ContextMenuStrip();
			trayMenu.Items.Add("Show", null, (s, e) => ShowMainWindow());
			trayMenu.Items.Add("Exit", null, (s, e) => ExitApplication());

			_trayIcon.ContextMenuStrip = trayMenu;

			_trayIcon.DoubleClick += (s, e) =>
			{
				ShowMainWindow();
			};

			if (SettingsLoader.Current.HideToTray && !TempConfigLoader.Current.AdminPriviligesRequested)
			{
				HideMainWindow();
			}
			else
			{
				ShowMainWindow();
			}


			if (SettingsLoader.Current.CheckUpdates)
			{
				StartAutoUpdateCheck();
			}

			TempConfigLoader.Current.AdminPriviligesRequested = false;
			TempConfigLoader.Save();
		}

		public async void StartAutoUpdateCheck()
		{
			if (_updateChecksStarted)
				return;
			_updateChecksStarted = true;

			while (SettingsLoader.Current.CheckUpdates)
			{
				try
				{
					var now = DateTime.UtcNow;
					var last = TempConfigLoader.Current.LastUpdateCheck;

					if (last == default || (now - last) >= Constants.applicationUpdateInterval)
					{
						await DoUpdateCheck();
						TempConfigLoader.Current.LastUpdateCheck = DateTime.UtcNow;
						TempConfigLoader.Save();
					}

					var remaining = Constants.applicationUpdateInterval - (now - last);
					if (remaining < TimeSpan.Zero)
						remaining = TimeSpan.Zero;

					await Task.Delay(remaining);
				}
				catch
				{
				}
			}
			_updateChecksStarted = false;
		}

		private async Task DoUpdateCheck()
		{
			var update = await UpdateService.CheckForUpdateAsync(Application.ProductVersion, true);

			if (update != null)
			{
				if (!_form.IsDisposed)
					_form.UpdateCheckUpdateNowBtnText("Update Now");

				_trayIcon.BalloonTipTitle = $"{Constants.AppName} Update";
				_trayIcon.BalloonTipText = "A new version is available. Click \"Update Now\".";
				_trayIcon.BalloonTipIcon = ToolTipIcon.Info;
				_trayIcon.ShowBalloonTip(5000);

				_trayIcon.BalloonTipClicked -= TrayIcon_BalloonTipClicked;
				_trayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;
			}
			else
			{
				if (!_form.IsDisposed)
					_form.UpdateCheckUpdateNowBtnText("Check Update");
			}
		}

		private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			ShowMainWindow();
		}

		public void ShowMainWindow()
		{
			if (_form.InvokeRequired)
			{
				_form.Invoke(new MethodInvoker(ShowMainWindow));
				return;
			}

			if (_form.IsDisposed)
			{
				_form = new MainForm(this);
				if (!_form.IsHandleCreated)
				{
					var handle = _form.Handle;
				}
			}

			_form.Show();
			_form.WindowState = FormWindowState.Normal;
			_form.ShowInTaskbar = true;

			_form.Activate();
			_form.BringToFront();
		}

		public void HideMainWindow()
		{
			_form?.Hide();
			if(!_trayMinimizedNotifyShown)
			{
				_trayIcon.ShowBalloonTip(1000, $"{Constants.AppName}", "Application minimized to tray.", ToolTipIcon.Info);
				_trayMinimizedNotifyShown = true;
			}
		}

		public void ExitApplication()
		{
			if (ApplicationExiting)
				return;
			ApplicationExiting = true;
			if (_form != null && !_form.IsDisposed)
			{
				_form.Close();
				_form.Dispose();
			}
			_trayIcon.Visible = false;
			_trayIcon.Dispose();
			ExitThread();
		}
	}
}
