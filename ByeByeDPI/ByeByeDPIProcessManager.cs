using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Task = Microsoft.Win32.TaskScheduler.Task;

namespace ByeByeDPI
{
	public class GoodbyeDPIProcessManager
	{
		private const string GoodbyeDPITaskName = "GoodbyeDPI_Runner";

		public event Action<string> OnMessage;

		public bool IsRunning
		{
			get
			{
				return Process.GetProcessesByName("goodbyedpi").Any();
			}
		}

		public async System.Threading.Tasks.Task StartAsync(string exePath, string arguments = "")
		{
			if (IsRunning)
			{
				OnMessage?.Invoke("GoodbyeDPI is already running.");
				return;
			}

			try
			{
				using (TaskService ts = new TaskService())
				{
					TaskDefinition td;
					Task task = ts.GetTask(GoodbyeDPITaskName);
					if (task == null)
					{
						CreateGoodbyeDPIRunnerTask(ts, exePath);
						task = ts.GetTask(GoodbyeDPITaskName);
					}
					if (task == null)
					{
						OnMessage?.Invoke("Error: Cannot find or create the GoodbyeDPI task.");
						return;
					}
					td = task.Definition;
					if (td.Actions.OfType<ExecAction>().FirstOrDefault() is ExecAction action)
					{
						action.Path = exePath;
						action.Arguments = arguments;
						var logonTriggers = td.Triggers.OfType<LogonTrigger>().ToList();
						if (logonTriggers.Any())
						{
							foreach (var trigger in logonTriggers)
							{
								trigger.Enabled = SettingsLoader.Current.StartWithWindows;
							}
							if(SettingsLoader.Current.StartWithWindows)
							{
								OnMessage?.Invoke($"GoodbyeDPI windows startup enabled.");
							} else
							{
								OnMessage?.Invoke($"GoodbyeDPI windows startup disabled.");
							}
						}
						else if (SettingsLoader.Current.StartWithWindows)
						{
							td.Triggers.Add(new LogonTrigger()
							{
								UserId = null,
								//Delay = TimeSpan.FromSeconds(5),
								Enabled = true
							});
							OnMessage?.Invoke("GoodbyeDPI logon trigger added.");
						}
						ts.RootFolder.RegisterTaskDefinition(GoodbyeDPITaskName, td);
						OnMessage?.Invoke($"GoodbyeDPI parameters updated: {arguments}");
					}
					else
					{
						OnMessage?.Invoke("Error: Task action not found.");
						return;
					}
					task.Run();
					OnMessage?.Invoke($"GoodbyeDPI task started: {arguments}");
				}
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke("Failed to start GoodbyeDPI (Task Scheduler error): " + ex.Message);
			}

			await System.Threading.Tasks.Task.CompletedTask;
		}

		public async System.Threading.Tasks.Task StopAsync()
		{
			if (!PrivilegesHelper.EnsureAdministrator(OnMessage))
				return;

			try
			{
				using (TaskService ts = new TaskService())
				{
					Task task = ts.GetTask(GoodbyeDPITaskName);

					if (task != null)
					{
						var td = task.Definition;
						var logonTriggers = td.Triggers.OfType<LogonTrigger>().ToList();
						if (logonTriggers.Any())
						{
							foreach (var trigger in logonTriggers)
							{
								trigger.Enabled = SettingsLoader.Current.StartWithWindows;
							}
							ts.RootFolder.RegisterTaskDefinition(GoodbyeDPITaskName, td);
							if (SettingsLoader.Current.StartWithWindows)
							{
								OnMessage?.Invoke($"GoodbyeDPI windows startup enabled.");
							}
							else
							{
								OnMessage?.Invoke($"GoodbyeDPI windows startup disabled.");
							}
						}

						if (task.State == TaskState.Running)
						{
							task.Stop();
							OnMessage?.Invoke("GoodbyeDPI task stopped.");
						}
						else
						{
							OnMessage?.Invoke("GoodbyeDPI task is not running. Checking for any running processes...");
						}
					}
					else
					{
						OnMessage?.Invoke("GoodbyeDPI task not found. Checking for any running processes...");
					}
				}

				var others = Process.GetProcessesByName("goodbyedpi");
				foreach (var p in others)
				{
					try
					{
						p.Kill();
						OnMessage?.Invoke("GoodbyeDPI remaining processes terminated.");
					}
					catch { }
					finally
					{
						try { p.Dispose(); } catch { }
					}
				}
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke("Failed to stop GoodbyeDPI: " + ex.Message);
			}

			await System.Threading.Tasks.Task.CompletedTask;
		}

		public bool DeleteTask()
		{
			if (!PrivilegesHelper.IsAdministrator())
			{
				OnMessage?.Invoke("Error: Administrator privileges are required to delete the task.");
				return false;
			}
			try
			{
				using (TaskService ts = new TaskService())
				{
					Task task = ts.GetTask(GoodbyeDPITaskName);
					if (task != null)
					{
						ts.RootFolder.DeleteTask(GoodbyeDPITaskName);
						OnMessage?.Invoke($"'{GoodbyeDPITaskName}' task successfully deleted from Task Scheduler.");
						return true;
					}
					else
					{
						OnMessage?.Invoke($"'{GoodbyeDPITaskName}' task does not exist in Task Scheduler.");
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				OnMessage?.Invoke($"Error: Failed to delete the task: {ex.Message}");
				return false;
			}
		}

		private void CreateGoodbyeDPIRunnerTask(TaskService ts, string exePath)
		{
			if (!PrivilegesHelper.IsAdministrator())
			{
				OnMessage?.Invoke("Error: Administrator privileges are required to create the task.");
				return;
			}
			TaskDefinition td = ts.NewTask();
			td.RegistrationInfo.Description = "Used to run GoodbyeDPI with administrator privileges.";
			td.Triggers.Add(new LogonTrigger()
			{
				UserId = null,
				//Delay = TimeSpan.FromSeconds(5)
			});
			td.Principal.RunLevel = TaskRunLevel.Highest;
			td.Principal.LogonType = TaskLogonType.ServiceAccount;
			td.Principal.UserId = "SYSTEM";
			string workingDir = Path.GetDirectoryName(exePath) ?? AppDomain.CurrentDomain.BaseDirectory;
			td.Actions.Add(new ExecAction(exePath, "", workingDir));
			td.Settings.AllowDemandStart = true;
			td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
			td.Settings.Hidden = false;
			ts.RootFolder.RegisterTaskDefinition(GoodbyeDPITaskName, td);
		}

	}
}
