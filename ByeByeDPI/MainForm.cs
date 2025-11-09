using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public partial class MainForm : Form, IDisposable
	{
		private readonly TrayApplicationContext _trayApplicationContext;
		private readonly MainFormViewModel _viewModel;

		public MainForm(TrayApplicationContext trayApplicationContext)
		{
			InitializeComponent();

			_trayApplicationContext = trayApplicationContext;
			_viewModel = new MainFormViewModel(this);

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

			HideToTrayChbox.CheckedChanged -= HideToTrayChbox_CheckedChanged;
			CheckUpdatesChbox.CheckedChanged -= CheckUpdatesChbox_CheckedChanged;
			StartWithWindowsChbox.CheckedChanged -= StartWithWindowsChbox_CheckedChanged;

			HideToTrayChbox.Checked = SettingsLoader.Current.HideToTray;
			CheckUpdatesChbox.Checked = SettingsLoader.Current.CheckUpdates;
			StartWithWindowsChbox.Checked = SettingsLoader.Current.StartWithWindows;

			HideToTrayChbox.CheckedChanged += HideToTrayChbox_CheckedChanged;
			CheckUpdatesChbox.CheckedChanged += CheckUpdatesChbox_CheckedChanged;
			StartWithWindowsChbox.CheckedChanged += StartWithWindowsChbox_CheckedChanged;

			StartStateSync();

			if (_viewModel.IsGoodbyeDPIRunning)
			{
				if (!string.IsNullOrEmpty(SettingsLoader.Current.ChosenParam))
					MessageWriteLine("Selected parameter: " + SettingsLoader.Current.ChosenParam);
				else
					MessageWriteLine("No parameter selected. It seems your settings are not synced.");
				MessageWriteLine("GoodbyeDPI is running!");
			}
			else
			{
				MessageWriteLine("GoodbyeDPI is stopped!");
			}
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

		public void UpdateCheckUpdateNowBtnText(string newString)
		{
			CheckUpdateNowBtn.Text = newString;
		}


		private void ClearMessages()
		{
			ListBoxForMessages.Items.Clear();
		}

		private void LockProcessButtons()
		{
			CheckDomainsBtn.Enabled = false;
			ToggleDPIBtn.Enabled = false;
			ResetBtn.Enabled = false;
		}

		private void UnlockProcessButtons()
		{
			CheckDomainsBtn.Enabled = true;
			ToggleDPIBtn.Enabled = true;
			ResetBtn.Enabled = true;
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
				await Task.Delay(3000);
			}
		}

		private async void CheckDomainsBtn_Click(object sender, EventArgs e)
		{
			LockProcessButtons();
			ClearMessages();
			MessageWriteLine("Starting to check domains from domain list...");
			await _viewModel.BeginCheckDomainListAsync();
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
				await _viewModel.ToggleGoodbyeDPIAsync();
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
			MessageWriteLine(HideToTrayChbox.Checked
		? "The application will now run in the system tray when closed."
		: "The application will no longer run in the system tray when closed.");
		}

		private void CheckUpdatesChbox_CheckedChanged(object sender, EventArgs e)
		{
			SettingsLoader.Current.CheckUpdates = CheckUpdatesChbox.Checked;
			SettingsLoader.Save();
			if (CheckUpdatesChbox.Checked)
			{
				_trayApplicationContext.StartAutoUpdateCheck();
			}
			MessageWriteLine(CheckUpdatesChbox.Checked
	  ? "Automatic updates have been enabled."
	  : "Automatic updates have been disabled.");
		}

		private async void StartWithWindowsChbox_CheckedChanged(object sender, EventArgs e)
		{
			StartWithWindowsChbox.Enabled = false;
			await _viewModel.ToggleStartWithWindows(StartWithWindowsChbox.Checked);
			StartWithWindowsChbox.Enabled = true;
		}

		private async void CheckUpdateNow_Click(object sender, EventArgs e)
		{
			CheckUpdateNowBtn.Enabled = false;
			string currentVersion = Application.ProductVersion;
			var update = await UpdateService.CheckForUpdateAsync(currentVersion);
			if (update != null)
			{
				var result = MessageBox.Show(
					"A new version is available. Do you want to download it now? New update features: " + update.Notes,
					"Update Available v" + update.Version,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Information);
				if (result == DialogResult.Yes)
				{
					if (_viewModel.IsGoodbyeDPIRunning)
					{
						await _viewModel.ToggleGoodbyeDPIAsync();
					}
					await UpdateService.DownloadAndRunUpdateAsync();
				}
			}
			else
			{
				MessageBox.Show("No updates available.", "Up to date", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			CheckUpdateNowBtn.Enabled = true;
		}

		private void OpenDomainListBtn_Click(object sender, EventArgs e)
		{
			FileOpener.OpenFileInBaseDir(Constants.CheckListPath);
		}

		private void OpenParamsBtn_Click(object sender, EventArgs e)
		{
			FileOpener.OpenFileInBaseDir(Constants.ParamsPath);
		}

		private async void ResetBtn_Click(object sender, EventArgs e)
		{
			LockProcessButtons();
			var result = MessageBox.Show(
				"Are you sure you want to reset?\n" +
				"This will reset the selected parameter and stops the GoodbyeDPI. After that if you want to run again, you need to re-do parameter selection workflow.",
				"Clear Selected Parameter",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);
			if (result == DialogResult.Yes)
			{
				await _viewModel.ClearSelectedParam();
				MessageBox.Show(
					"Selected parameter has been cleared. GoodbyeDPI stopped.", "Clear Complete",
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

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (_trayApplicationContext.ApplicationExiting)
			{
				base.OnFormClosing(e);
				return;
			}
			if (!SettingsLoader.Current.HideToTray || TempConfigLoader.Current.AdminPriviligesRequested)
			{
				_trayApplicationContext.ExitApplication();
			}
			else
			{
				e.Cancel = true;
				_trayApplicationContext.HideMainWindow();
			}
		}

	}
}
