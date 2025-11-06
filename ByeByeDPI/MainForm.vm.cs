using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class Form1ViewModel : IDisposable
	{
		public event Action<string> OnMessage;
		public bool IsGoodbyeDPIRunning => _dpiManager.IsRunning;

		private readonly GoodbyeDPIProcessManager _dpiManager = new GoodbyeDPIProcessManager();
		private readonly HttpClient _httpClient = new HttpClient();
		private List<CheckListModel> _checkList { get; set; } = new List<CheckListModel>();
		private List<ParamModel> _paramList { get; set; } = new List<ParamModel>();
		private bool _isWorkflowRunning = false;
		private bool _isCheckListRunnig = false;
		private bool _isDisposed = false;

		public Form1ViewModel(MainForm mainForm)
		{
			_dpiManager.OnMessage += (msg) => OnMessage?.Invoke(msg);
			_httpClient.Timeout = TimeSpan.FromSeconds(5);
			_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36");
			_httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
			_httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
			_httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
			_httpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
		}

		public void LoadSettings()
		{
			SettingsLoader.LoadSettings();
			OnMessage?.Invoke("Settings loaded.");
		}

		public void LoadCheckList()
		{
			_checkList = CheckListLoader.LoadCheckList(Constants.CheckListPath);
			OnMessage?.Invoke("Check list loaded.");
		}

		public void LoadParams()
		{
			_paramList = ParamsLoader.LoadParams(Constants.ParamsPath);
			OnMessage?.Invoke("Parameters loaded.");
		}

		public async Task ClearSelectedParam()
		{
			try
			{
				await _dpiManager.StopAsync();
				#region Prevent secondary admin request
				if (!PrivilegesHelper.IsAdministrator())
					return;
				#endregion
				await _dpiManager.DeleteTask();
				SettingsLoader.Current.ChosenParam = "";
				SettingsLoader.Save();
				OnMessage?.Invoke($"Selected parameter cleared successfully.");
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke($"Failed to clear selected parameter.\nError: {ex.Message}");
			}
		}

		public async Task BeginCheckDomainListAsync()
		{
			if (_isCheckListRunnig) return;
			_isCheckListRunnig = true;
			foreach (var item in _checkList)
			{
				if (_isDisposed)
					break;
				try
				{
					var url = item.Url.StartsWith("www.") ? item.Url : "www." + item.Url;
					bool accessible = false;
					try
					{
						var headRequest = new HttpRequestMessage(HttpMethod.Head, "https://" + url);
						var headResponse = await _httpClient.SendAsync(headRequest);
						accessible = headResponse.IsSuccessStatusCode;
					}
					catch { accessible = false; }
					if (!accessible)
					{
						try
						{
							var getResponse = await _httpClient.GetAsync("https://" + url);
							accessible = getResponse.IsSuccessStatusCode;
						}
						catch { accessible = false; }
					}
					item.Accessible = accessible;
				}
				catch { item.Accessible = false; }
				string statusEmoji = item.Accessible ? "✅" : "❌";
				OnMessage?.Invoke($"Is {item.Name} accessible? {statusEmoji}");
			}
			_isCheckListRunnig = false;
		}

		public async Task ToggleGoodbyeDPIAsync()
		{
			if (IsGoodbyeDPIRunning)
			{
				await _dpiManager.StopAsync();
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(SettingsLoader.Current.ChosenParam))
				{
					await _dpiManager.StartAsync(Constants.GoodbyeDPIPath, SettingsLoader.Current.ChosenParam);
				}
				else
				{
					await RunParamSelectionWorkflowAsync();
				}
			}
		}

		public async Task RunParamSelectionWorkflowAsync()
		{
			if (_isWorkflowRunning || _isCheckListRunnig)
				return;
			_isWorkflowRunning = true;
			foreach (var item in _paramList)
			{
				OnMessage?.Invoke($"Trying parameter '{item.Name}'...");

				await _dpiManager.StartAsync(Constants.GoodbyeDPIPath, item.Value);
				await BeginCheckDomainListAsync();

				var result = MessageBox.Show(
					$"Parameter set '{item.Name}' applied.\n\nCan you access the desired content correctly? \n\n'Yes' for OK, 'No' for next parameter.",
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
			_isWorkflowRunning = false;
		}

		public void ToggleStartWithWindows(bool newState)
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
			_httpClient.Dispose();
			_isDisposed = true;
		}
	}

}
