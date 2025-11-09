using ByeByeDPI.Loaders;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class MainFormViewModel : IDisposable
	{
		public event Action<string> OnMessage;
		public bool IsGoodbyeDPIRunning => _dpiManager.IsRunning;

		private readonly GoodbyeDPIProcessManager _dpiManager = new GoodbyeDPIProcessManager();
		private readonly HttpClient _httpClient = new HttpClient();
		private List<CheckListWrapperModel> _checkList { get; set; } = new List<CheckListWrapperModel>();
		private List<ParamModel> _paramList { get; set; } = new List<ParamModel>();
		private bool _isWorkflowRunning = false;
		private bool _isCheckListRunnig = false;
		private bool _isDisposed = false;

		public MainFormViewModel(MainForm mainForm)
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
			var rawList = CheckListLoader.LoadCheckList();
			_checkList = rawList.Select(x => new CheckListWrapperModel { Item = x, IsAccesible = false }).ToList();
			OnMessage?.Invoke("Check list loaded.");
		}

		public void LoadParams()
		{
			_paramList = ParamsLoader.LoadParams();
			OnMessage?.Invoke("Parameters loaded.");
		}

		public async System.Threading.Tasks.Task ClearSelectedParam()
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

		public async System.Threading.Tasks.Task BeginCheckDomainListAsync()
		{
			if (_isCheckListRunnig) return;
			_isCheckListRunnig = true;

			foreach (var wrapper in _checkList)
			{
				if (_isDisposed)
					break;

				var item = wrapper.Item;

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

					wrapper.IsAccesible = accessible;
				}
				catch
				{
					wrapper.IsAccesible = false;
				}

				string statusEmoji = wrapper.IsAccesible ? "✅" : "❌";
				OnMessage?.Invoke($"Is {item.Name} accessible? {statusEmoji}");
			}

			_isCheckListRunnig = false;
		}


		public async System.Threading.Tasks.Task ToggleGoodbyeDPIAsync()
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

		public async System.Threading.Tasks.Task RunParamSelectionWorkflowAsync()
		{
			if (_isWorkflowRunning || _isCheckListRunnig)
				return;
			_isWorkflowRunning = true;
			foreach (var item in _paramList)
			{
				OnMessage?.Invoke($"Trying parameter '{item.Name}'...");

				await _dpiManager.StartAsync(Constants.GoodbyeDPIPath, item.Param);
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

		public async System.Threading.Tasks.Task ToggleStartWithWindows(bool newState)
		{
			if (!await PrivilegesHelper.EnsureAdministrator(onMessage: OnMessage))
				return;
			SettingsLoader.Current.StartWithWindows = newState;
			SettingsLoader.Save();
			string appName = Constants.AppName;
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
				OnMessage?.Invoke(newState
					? "Application will start with Windows (Registry updated)."
					: "Application will not start with Windows (Registry updated).");
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke($"Failed to update application startup (Registry): {ex.Message}");
			}

			try
			{
				using (TaskService ts = new TaskService())
				{
					var task = ts.GetTask("GoodbyeDPI_Runner");
					if (task == null)
					{
						_dpiManager.CreateGoodbyeDPIRunnerTask(ts, Constants.GoodbyeDPIPath).Wait();
						task = ts.GetTask("GoodbyeDPI_Runner");
					}
					if (task != null)
					{
						var td = task.Definition;
						var logonTriggers = td.Triggers.OfType<LogonTrigger>();
						foreach (var trigger in logonTriggers)
							trigger.Enabled = newState;

						ts.RootFolder.RegisterTaskDefinition("GoodbyeDPI_Runner", td);
						OnMessage?.Invoke(newState
							? "GoodbyeDPI will start with Windows (Task Scheduler updated)."
							: "GoodbyeDPI will not start with Windows (Task Scheduler updated).");
					}
				}
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke($"Failed to update GoodbyeDPI startup (Task Scheduler): {ex.Message}");
			}
		}


		public void Dispose()
		{
			_httpClient.Dispose();
			_isDisposed = true;
		}
	}

}
