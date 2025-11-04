using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class Form1ViewModel : IDisposable
	{
		private MainForm _view;

		public event Action<string> OnMessage;
		private readonly ByeByeDPIProcessManager _dpiManager = new ByeByeDPIProcessManager();
		public List<CheckListModel> CheckList { get; private set; } = new List<CheckListModel>();
		public List<ParamModel> ParamList { get; private set; } = new List<ParamModel>();
		public bool IsGoodbyeDPIRunning => _dpiManager.IsRunning;

		public void SetFormView(MainForm view)
		{
			_view = view;
		}

		public void LoadSettings()
		{
			SettingsLoader.LoadSettings();
			OnMessage?.Invoke("Settings loaded.");
		}

		public void LoadCheckList()
		{
			CheckList = CheckListLoader.LoadCheckList(Constants.CheckListPath);
			OnMessage?.Invoke("Check list loaded.");
		}

		public void LoadParams()
		{
			ParamList = ParamsLoader.LoadParams(Constants.ParamsPath);
			OnMessage?.Invoke("Parameters loaded.");
		}

		public void ClearChosenParam()
		{
			try
			{
				SettingsLoader.Current.ChosenParam = "";
				OnMessage?.Invoke($"Chosen profile cleared successfully.");
				SettingsLoader.Save();
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke($"Failed to clear chosen profile file.\nError: {ex.Message}");
			}
		}

		public async Task StartCheckingCheckListAsync()
		{
			 HttpClient client = new HttpClient();
			foreach (var item in CheckList)
			{
				try
				{
					var response = await client.GetAsync("https://" + item.Url);
					item.Accessible = response.IsSuccessStatusCode;
				}
				catch
				{
					item.Accessible = false;
				}

				string statusEmoji = item.Accessible ? "✅" : "❌";
				OnMessage?.Invoke($"Is {item.Name} accessible? {statusEmoji}");
			}
			client.Dispose();
		}


		public async Task ToggleByeByeDPIAsync()
		{
			if (IsGoodbyeDPIRunning)
				await _dpiManager.StopAsync();
			else
			{
				if (!String.IsNullOrWhiteSpace(SettingsLoader.Current.ChosenParam))
				{
					await _dpiManager.StartAsync(Constants.GoodbyeDPIPath, SettingsLoader.Current.ChosenParam);
				} else
				{
					await RunParamSelectionWorkflowAsync();
				}
			}
				
		}
	

		public async Task RunParamSelectionWorkflowAsync()
		{
			foreach (var item in ParamList)
			{
				OnMessage?.Invoke($"Trying parameter '{item.Name}'...");

				await _dpiManager.StartAsync(Constants.GoodbyeDPIPath, item.Value);
				await StartCheckingCheckListAsync();

				var result = MessageBox.Show(
					$"Parameter set '{item.Name}' applied.\n\nCan you access the desired content correctly?",
					"Test Parameter",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					SettingsLoader.Current.ChosenParam = item.Name;
					SettingsLoader.Save();
					OnMessage?.Invoke($"Parameter '{item.Name}' accepted and saved.");
					break;
				}
				else if (result == DialogResult.No)
				{
					OnMessage?.Invoke($"Parameter '{item.Name}' rejected by user, trying next...");
					await _dpiManager.StopAsync();
				}
				else if (result == DialogResult.Cancel)
				{
					OnMessage?.Invoke("Parameter selection workflow cancelled by user.");
					await _dpiManager.StopAsync();
					break;
				}
			}

			OnMessage?.Invoke("Parameter selection workflow completed.");
		}

		public void ToggleAutoStartWithWindows(bool newState)
		{
			string appName = Constants.RegistryAppName;
			string exePath = $"\"{Application.ExecutablePath}\"";
			try
			{
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
						   @"Software\Microsoft\Windows\CurrentVersion\Run", writable: true))
				{
					if (newState)
					{
						key.SetValue(appName, exePath);
					}
					else
					{
						if (key.GetValue(appName) != null)
							key.DeleteValue(appName);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to update Windows startup.\nError: {ex.Message}",
								"Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Dispose()
		{
			_dpiManager.Dispose();
		}
	}

}
