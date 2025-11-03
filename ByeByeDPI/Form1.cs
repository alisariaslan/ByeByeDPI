using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public partial class Form1 : Form, IDisposable
	{
		private readonly Form1ViewModel _viewModel;
		private NotifyIcon _trayIcon;
		private bool _updateChecksStarted;

		public Form1(Form1ViewModel viewModel)
		{
			InitializeComponent();
			InitializeTray();

			_viewModel = viewModel;
			_viewModel.SetFormView(this);

			_viewModel.OnMessage += new Action<string>(msg =>
			{
				if (ListBoxForMessages.InvokeRequired)
				{
					ListBoxForMessages.Invoke(new Action(() => MessageWriteLine(msg)));
				}
				else
				{
					MessageWriteLine(msg);
				}
			});

			_viewModel.LoadSettings();
			_viewModel.LoadCheckList();
			_viewModel.LoadParams();

			if (_viewModel.IsByeByeDPIRunning)
			{
				ToggleDPIBtn.Text = "Stop Access";
				MessageWriteLine(Constants.GoodbyeDPIFileName + " is already running!");
				MessageWriteLine("Please kill the process via Task Manager.");
				MessageWriteLine("Or try to stop in here.");
			}

			HideToTrayChbox.Checked = SettingsLoader.Current.HideToTray;
			CheckUpdatesChbox.Checked = SettingsLoader.Current.CheckUpdates;
			StartWithWindowsChbox.Checked = SettingsLoader.Current.StartWithWindows;
			StartMinimizedChbox.Checked = SettingsLoader.Current.StartMinimized;

			if (SettingsLoader.Current.StartMinimized)
			{
				this.WindowState = FormWindowState.Minimized;
				this.ShowInTaskbar = false;
				_trayIcon.Visible = true;
				_trayIcon.ShowBalloonTip(1000, "ByeByeDPI", "Application minimized to tray.", ToolTipIcon.Info);
			}

			if (SettingsLoader.Current.CheckUpdates)
			{
				StartAutoUpdateCheck();
			}
		}


		private async void StartAutoUpdateCheck()
		{
			if (_updateChecksStarted)
				return;
			_updateChecksStarted = true;
			string currentVersion = Application.ProductVersion;
			while (SettingsLoader.Current.CheckUpdates)
			{
				try
				{
					bool updateAvailable = await UpdateService.CheckForUpdateAsync(currentVersion);
					if (updateAvailable)
					{
						CheckUpdateNow.Text = "Update Now";
						_trayIcon.BalloonTipTitle = "ByeByeDPI Update";
						_trayIcon.BalloonTipText = "A new version is available. Click to \"Update Now\" for update.";
						_trayIcon.BalloonTipIcon = ToolTipIcon.Info;
						_trayIcon.Visible = true;
						_trayIcon.ShowBalloonTip(5000);
						_trayIcon.BalloonTipClicked -= TrayIcon_BalloonTipClicked;
						_trayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;
					} else
					{
						CheckUpdateNow.Text = "Check Update";
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
			_trayIcon.Visible = false;
			this.WindowState = FormWindowState.Normal;
			this.ShowInTaskbar = true;
		}

		private void InitializeTray()
		{
			_trayIcon = new NotifyIcon();
			_trayIcon.Icon = Properties.Resources.so_so_64px_ico;
			_trayIcon.Visible = false;
			_trayIcon.DoubleClick += (s, e) =>
			{
				this.Show();
				this.WindowState = FormWindowState.Normal;
				this.ShowInTaskbar = true;
				_trayIcon.Visible = false;
			};

		}

		private void MessageWriteLine(string msg)
		{
			ListBoxForMessages.Items.Add(msg);

			int lastIndex = ListBoxForMessages.Items.Count - 1;
			if (lastIndex >= 0)
			{
				ListBoxForMessages.TopIndex = lastIndex;
			}
		}

		private void ClearMessages()
		{
			ListBoxForMessages.Items.Clear();
		}

		private async void ToggleDPIBtn_Click(object sender, EventArgs e)
		{
			ToggleDPIBtn.Enabled = false;
			ClearMessages();

			await _viewModel.ToggleByeByeDPIAsync();

			ToggleDPIBtn.Text = _viewModel.IsByeByeDPIRunning ? "Stop Access" : "Start Access";

			if (_viewModel.IsByeByeDPIRunning)
			{
				MessageWriteLine(Constants.GoodbyeDPIFileName + " is running.");
			}
			else
			{
				MessageWriteLine(Constants.GoodbyeDPIFileName + " is stopped.");
			}

			await Task.Delay(3000);
			ToggleDPIBtn.Enabled = true;
		}

		private void HideToTrayChbox_CheckedChanged(object sender, EventArgs e)
		{
			SettingsLoader.Current.HideToTray = HideToTrayChbox.Checked;
			SettingsLoader.Save();
		}

		private void CheckUpdatesChbox_CheckedChanged(object sender, EventArgs e)
		{
			SettingsLoader.Current.CheckUpdates = CheckUpdatesChbox.Checked;
			SettingsLoader.Save();
			if (CheckUpdatesChbox.Checked)
			{
				StartAutoUpdateCheck();
			}
		}
		private void StartMinimizedChbox_CheckedChanged(object sender, EventArgs e)
		{
			SettingsLoader.Current.StartMinimized = StartMinimizedChbox.Checked;
			SettingsLoader.Save();
		}

		private void StartWithWindowsChbox_CheckedChanged(object sender, EventArgs e)
		{
			SettingsLoader.Current.StartWithWindows = StartWithWindowsChbox.Checked;
			SettingsLoader.Save();
			_viewModel.ToggleAutoStartWithWindows(StartWithWindowsChbox.Checked);
		}

		private async void CheckUpdateNow_Click(object sender, EventArgs e)
		{
			CheckUpdateNow.Enabled = false;
			string currentVersion = Application.ProductVersion;
			bool updateAvailable = await UpdateService.CheckForUpdateAsync(currentVersion);

			if (updateAvailable)
			{
				var result = MessageBox.Show(
					"A new version is available. Do you want to download it now?",
					"Update Available",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Information);

				if (result == DialogResult.Yes)
				{
					await UpdateService.DownloadAndRunUpdateAsync();
				}
			}
			else
			{
				MessageBox.Show("No updates available.", "Up to date", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			CheckUpdateNow.Enabled = true;
		}

		private void OpenCheckListBtn_Click(object sender, EventArgs e)
		{
			FileOpenerUtil.OpenFileInBaseDir(Constants.CheckListFileName);
		}

		private void ParamsBtn_Click(object sender, EventArgs e)
		{
			FileOpenerUtil.OpenFileInBaseDir(Constants.ParamsFileName);
		}

		private void ClearParamBtn_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show(
				"Are you sure you want to clear the chosen parameter?\n" +
				"This will delete the saved parameter. You need to re-do param selection workflow after this.",
				"Clear Chosen Parameter",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);
			if (result == DialogResult.Yes)
			{
				_viewModel.ClearChosenParam();
				MessageBox.Show(
					"Chosen parameter has been cleared.", "Clear Complete",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
		}

		private void ListBoxForMessages_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int index = ListBoxForMessages.IndexFromPoint(e.Location);
				if (index != ListBox.NoMatches)
				{
					ListBoxForMessages.SelectedIndex = index;
					copyToolStripMenuItem.Visible = true;
				}
				else
				{
					copyToolStripMenuItem.Visible = false;
					ListBoxForMessages.ClearSelected();
				}
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ListBoxForMessages.SelectedItem != null)
			{
				Clipboard.SetText(ListBoxForMessages.SelectedItem.ToString());
			}
		}

		private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (SettingsLoader.Current.HideToTray)
			{
				e.Cancel = true;
				this.WindowState = FormWindowState.Minimized;
				this.ShowInTaskbar = false;
				_trayIcon.Visible = true;
				HideToTrayChbox_CheckedChanged(HideToTrayChbox, EventArgs.Empty);
			}
			else
			{
				_viewModel.Dispose();
				_trayIcon.Dispose();
				await _viewModel.StopByeByeDPIAsync();
				base.OnFormClosing(e);
			}
		}
	}
}
