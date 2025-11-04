using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public partial class MainForm : Form, IDisposable
	{
		private readonly Form1ViewModel _viewModel;
		private NotifyIcon _trayIcon;
		private bool _updateChecksStarted;
		private bool _closeWithoutTray;

		public MainForm(Form1ViewModel viewModel)
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

			this.Text = Application.ProductName + " v" + Application.ProductVersion;

			_viewModel.LoadSettings();
			_viewModel.LoadCheckList();
			_viewModel.LoadParams();

			HideToTrayChbox.Checked = SettingsLoader.Current.HideToTray;
			CheckUpdatesChbox.Checked = SettingsLoader.Current.CheckUpdates;
			StartWithWindowsChbox.Checked = SettingsLoader.Current.StartWithWindows;

			if (SettingsLoader.Current.HideToTray && !TempConfigLoader.Current.AdminPriviligesRequested)
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

			ToggleDPIBtnTextSync();

			if (_viewModel.IsGoodbyeDPIRunning)
			{
				MessageWriteLine("GoodbyeDPI is already running!");
			}

			StartStateSync();
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
					}
					else
					{
						CheckUpdateNow.Text = "Check Now";
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

			var trayMenu = new ContextMenuStrip();
			trayMenu.Items.Add("Show", null, (s, e) => ShowMainWindow());
			trayMenu.Items.Add("Close", null, (s, e) =>
			{
				_closeWithoutTray = true;
				this.Close();
			});
			_trayIcon.ContextMenuStrip = trayMenu;

			_trayIcon.DoubleClick += (s, e) =>
			{
				if (this.Visible && this.WindowState != FormWindowState.Minimized)
				{
					HideMainWindow();
				}
				else
				{
					ShowMainWindow();
				}
			};
		}

		private void ShowMainWindow()
		{
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.ShowInTaskbar = true;
			_trayIcon.Visible = false;
		}

		private void HideMainWindow()
		{
			this.Hide();
			this.ShowInTaskbar = false;
			_trayIcon.Visible = true;
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

		private void LockProcessButtons()
		{
			RunBtn.Enabled = false;
			ToggleDPIBtn.Enabled = false;
			ResetBtn.Enabled = false;
		}

		private void UnlockProcessButtons()
		{
			RunBtn.Enabled = true;
			ToggleDPIBtn.Enabled = true;
			ResetBtn.Enabled = true;
		}

		private async void RunBtn_Click(object sender, EventArgs e)
		{
			LockProcessButtons();
			ClearMessages();
			MessageWriteLine("CheckList started to check domains...");
			await _viewModel.StartCheckingCheckListAsync();
			UnlockProcessButtons();
		}

		private async void ToggleDPIBtn_Click(object sender, EventArgs e)
		{
			LockProcessButtons();
			ClearMessages();

			if (ToggleDPIBtn.Text != GetToggleDPIBtnExpectedText())
			{
				MessageBox.Show(
					"The state has changed and is not in sync with the UI. Please wait a moment and try again.",
					"Warning",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}
			else
			{
				await _viewModel.ToggleByeByeDPIAsync();
			}

			ToggleDPIBtnTextSync();
			MessageWriteLine(_viewModel.IsGoodbyeDPIRunning ? "GoodbyeDPI is running." : "GoodbyeDPI is stopped.");

			await Task.Delay(3000);
			UnlockProcessButtons();
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
			FileOpenerUtil.OpenFileInBaseDir(Constants.CheckListPath);
		}

		private void ParamsBtn_Click(object sender, EventArgs e)
		{
			FileOpenerUtil.OpenFileInBaseDir(Constants.ParamsPath);
		}

		private async void ResetBtn_Click(object sender, EventArgs e)
		{
			LockProcessButtons();
			var result = MessageBox.Show(
				"Are you sure you want to clear the profile?\n" +
				"This will delete the saved profile. You need to re-do profile(parameter) selection workflow after this.",
				"Clear Profile",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);
			if (result == DialogResult.Yes)
			{
				await _viewModel.ClearChosenParam();
				MessageBox.Show(
					"Chosen profile has been cleared.", "Clear Complete",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
			ToggleDPIBtnTextSync();
			UnlockProcessButtons();
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


		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string aboutHtml = $@"
    <!doctype html>
    <html>
    <head>
      <meta charset='utf-8'/>
      <title>About ByeByeDPI</title>
      <style>
        body {{ font-family: Segoe UI, Tahoma, Arial; padding: 16px; color:#222; }}
        h1 {{ margin-top:0; }}
        a {{ color:#1a73e8; text-decoration:none; }}
        a:hover {{ text-decoration:underline; }}
        .meta {{ margin-top:12px; color:#555; }}
        .footer {{ margin-top:20px; font-size:90%; color:#666; }}
      </style>
    </head>
    <body>
      <h1>ByeByeDPI GUI</h1>
      <div class='meta'>Version: {Application.ProductVersion}</div>
      <p>A lightweight graphical interface for controlling GoodbyeDPI.</p>
      <p>Developed by <strong>Ali SARIASLAN</strong></p>
      <p>Contact: <a href='mailto:dev@alisariaslan.com'>dev@alisariaslan.com</a></p>
      <div class='footer'>
        GitHub: <a href='https://github.com/alisariaslan/ByeByeDPI' target='_blank'>https://github.com/alisariaslan/ByeByeDPI</a>
      </div>
    </body>
    </html>";

			using (var dlg = new InfoDialog("About ByeByeDPI", aboutHtml))
			{
				dlg.ShowDialog(this);
			}
		}

		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string helpHtml = @"
    <!doctype html>
    <html>
    <head>
      <meta charset='utf-8'/>
      <title>Help - ByeByeDPI</title>
      <style>
        body { font-family: Segoe UI, Tahoma, Arial; padding:16px; color:#222; }
        h1 { margin-top:0; }
        ul { line-height:1.6; }
        a { color:#1a73e8; text-decoration:none; }
        .note { margin-top:12px; color:#555; }
      </style>
    </head>
    <body>
      <h1>Help - ByeByeDPI GUI</h1>
      <ul>
        <li><strong>Start Access</strong> — Starts the DPI bypass process (goodbyedpi.exe).</li>
        <li><strong>Stop Access</strong> — Stops goodbyedpi.exe process.</li>
        <li><strong>Params</strong> — Open and edit parameter file for launching goodbyedpi with arguments.</li>
        <li><strong>Checklist</strong> — Open checklist file to add or remove domains.</li>
        <li><strong>Run</strong> — Begins to check domains from checklist.</li>
        <li><strong>Update</strong> — Checks GitHub for newer versions.</li>
        <li><strong>Reset</strong> — Clears chosen profile.</li>
      </ul>
      <p class='note'>If something goes wrong, open an issue on the GitHub page or contact <a href='mailto:dev@alisariaslan.com'>dev@alisariaslan.com</a>.</p>
      <p>GitHub: <a href='https://github.com/alisariaslan/ByeByeDPI' target='_blank'>https://github.com/alisariaslan/ByeByeDPI</a></p>
    </body>
    </html>";

			using (var dlg = new InfoDialog("Help - ByeByeDPI", helpHtml))
			{
				dlg.ShowDialog(this);
			}
		}

		private string GetToggleDPIBtnExpectedText()
		{
			return _viewModel.IsGoodbyeDPIRunning ? "Stop Access" : "Start Access";
		}

		private void ToggleDPIBtnTextSync()
		{
			string expectedText = GetToggleDPIBtnExpectedText();
			if (ToggleDPIBtn.Text != expectedText)
			{
				ToggleDPIBtn.Text = expectedText;
			}
		}

		private async void StartStateSync()
		{
			while (!this.IsDisposed)
			{
				try
				{
					ToggleDPIBtnTextSync();
				}
				catch
				{
					break;
				}
				await Task.Delay(2000);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (SettingsLoader.Current.HideToTray && !_closeWithoutTray && !TempConfigLoader.Current.AdminPriviligesRequested)
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
			}
		}

	}
}
