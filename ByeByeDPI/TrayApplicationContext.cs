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

		public TrayApplicationContext()
		{
			_form = new MainForm(this);

			_trayIcon = new NotifyIcon()
			{
				Icon = Properties.Resources.icons8_so_so,
				Visible = true,
				Text = "ByeByeDPI"
			};

			var trayMenu = new ContextMenuStrip();
			trayMenu.Items.Add("Show", null, (s, e) => ShowMainWindow());
			trayMenu.Items.Add("Exit", null, (s, e) => ExitApplication());

			_trayIcon.ContextMenuStrip = trayMenu;

			_trayIcon.DoubleClick += (s, e) =>
			{
				if (_form.Visible)
					HideMainWindow();
				else
					ShowMainWindow();
			};

			if (SettingsLoader.Current.HideToTray && !TempConfigLoader.Current.AdminPriviligesRequested)
			{
				HideMainWindow();
			} else
			{
				ShowMainWindow();
			}

			if (SettingsLoader.Current.CheckUpdates)
			{
				StartAutoUpdateCheck();
			}

			TempConfigLoader.Reset_AdminPriviligesRequested();
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
					bool updateAvailable = await UpdateService.CheckForUpdateAsync(Application.ProductVersion);
					if (updateAvailable)
					{
						if (!_form.IsDisposed)
						{
							_form.UpdateCheckUpdateNowBtnText("Update Now");
						}
						_trayIcon.BalloonTipTitle = "ByeByeDPI Update";
						_trayIcon.BalloonTipText = "A new version is available. Click to \"Update Now\" for update.";
						_trayIcon.BalloonTipIcon = ToolTipIcon.Info;
						_trayIcon.Visible = true;
						_trayIcon.ShowBalloonTip(5000);
						_trayIcon.BalloonTipClicked -= TrayIcon_BalloonTipClicked;
						_trayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;
					}
					else
					{
						if(!_form.IsDisposed)
						{
							_form.UpdateCheckUpdateNowBtnText("Check Update");
						}
					}
				}
				catch
				{
				}
				await Task.Delay(TimeSpan.FromHours(1));
			}
			_updateChecksStarted = false;
		}

		private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			ShowMainWindow();
		}

		private void ShowMainWindow()
		{
			if(_form.IsDisposed)
			{
				_form = new MainForm(this);
			}
			_form.Show();
			_form.WindowState = FormWindowState.Normal;
			_form.ShowInTaskbar = true;
		}

		public void HideMainWindow()
		{
			if (_form.IsDisposed)
			{
				_form.Hide();
				_form.ShowInTaskbar = false;
			}
			_trayIcon.ShowBalloonTip(1000, "ByeByeDPI", "Application minimized to tray.", ToolTipIcon.Info);
		}

		public void ExitApplication()
		{
			_trayIcon.Visible = false;
			if (!_form.IsDisposed)
			{
				_form.Close();
			}
			Application.Exit();
		}
	}
}
