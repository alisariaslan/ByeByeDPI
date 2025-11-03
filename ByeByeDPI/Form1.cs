using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public partial class Form1 : Form , IDisposable
	{
		private readonly Form1ViewModel _viewModel;

		private NotifyIcon trayIcon;

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

			HideToTrayChbox.Checked = SettingsLoader.Current.HideToTray;
			CheckUpdatesChbox.Checked = SettingsLoader.Current.CheckUpdates;
			StartWithWindowsChbox.Checked = SettingsLoader.Current.StartWithWindows;

			if (_viewModel.IsByeByeDPIRunning)
			{
				ToggleDPIBtn.Text = "Stop Access";
				MessageWriteLine(Constants.GoodbyeDPIFileName+" is already running!");
				MessageWriteLine("Please kill the process via Task Manager.");
				MessageWriteLine("Or try to stop in here.");
			}
		}

		private void InitializeTray()
		{
			trayIcon = new NotifyIcon();
			trayIcon.Icon = Properties.Resources.so_so_64px_ico; 
			trayIcon.Visible = false;
			trayIcon.DoubleClick += (s, e) => {
				this.Show();
				this.WindowState = FormWindowState.Normal;
				trayIcon.Visible = false;
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

		private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			await _viewModel.StopByeByeDPIAsync();
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
			string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			MessageWriteLine("Checking for updates...");
			bool updateAvailable = await UpdateService.CheckForUpdateAsync(currentVersion);
			if (updateAvailable)
			{
				var result = MessageBox.Show("A new update is available. Do you want to download it now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if (result == DialogResult.Yes)
				{
					await UpdateService.DownloadUpdateAsync();
				}
			}
			else
			{
				MessageBox.Show("You are running the latest version.", "No Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (SettingsLoader.Current.HideToTray && this.WindowState == FormWindowState.Minimized)
			{
				trayIcon.Visible = true;
				this.Hide();
				trayIcon.ShowBalloonTip(1000, "ByeByeDPI", "Application minimized to tray.", ToolTipIcon.Info);
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			_viewModel.Dispose();
			base.OnHandleDestroyed(e);
		}
	
	}
}
