using ByeByeDPI.Constants;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ByeByeDPI.Services
{
    public class TaskService
    {
        public const string GoodbyeDPITaskName = "GoodbyeDPI_Runner";


        public event Action<string> OnMessage;

        public bool IsRunning
        {
            get
            {
                try
                {
                    return Process.GetProcesses()
                        .Any(p =>
                        {
                            try
                            {
                                return p.ProcessName.Equals("goodbyedpi", StringComparison.OrdinalIgnoreCase);
                            }
                            catch { return false; }
                        });
                }
                catch
                {
                    return false;
                }
            }
        }


        #region Public API

        public async System.Threading.Tasks.Task ToggleAsync(
            string exePath,
            string paramName,
            string paramValue)
        {
            if (IsRunning)
            {
                await StopAsync();
            }
            else
            {
                await StartAsync(exePath, paramName, paramValue);
            }
        }

        public async System.Threading.Tasks.Task StartAsync(string exePath, string paramName, string arguments)
        {
            if (IsRunning)
            {
                OnMessage?.Invoke("GoodbyeDPI is already running.");
                return;
            }

            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            try
            {
                using var ts = new Microsoft.Win32.TaskScheduler.TaskService();
                var task = ts.GetTask(GoodbyeDPITaskName);

                if (task == null)
                {
                    await CreateRunnerTaskAsync(
     ts,
     SettingsLoader.Current.AutoRunGoodbyeDPI
 );

                    task = ts.GetTask(GoodbyeDPITaskName);
                }

                if (task == null)
                {
                    OnMessage?.Invoke("Error: Cannot create GoodbyeDPI task.");
                    return;
                }

                var action = task.Definition.Actions.OfType<ExecAction>().FirstOrDefault();
                if (action == null)
                {
                    OnMessage?.Invoke("Error: Task action not found.");
                    return;
                }

                action.Path = exePath;
                action.Arguments = arguments;
                action.WorkingDirectory = AppConstants.AppBaseDir;

                ts.RootFolder.RegisterTaskDefinition(
     GoodbyeDPITaskName,
     task.Definition,
     TaskCreation.CreateOrUpdate,
     "SYSTEM",
     null,
     TaskLogonType.ServiceAccount
 );

                task.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start GoodbyeDPI: {ex.Message}");
            }
        }

        public async System.Threading.Tasks.Task StopAsync()
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            try
            {
                using var ts = new Microsoft.Win32.TaskScheduler.TaskService();
                var task = ts.GetTask(GoodbyeDPITaskName);

                if (task?.State == TaskState.Running)
                {
                    task.Stop();
                    Console.WriteLine("GoodbyeDPI task stopped.");
                }

                foreach (var p in Process.GetProcessesByName("goodbyedpi"))
                {
                    try { p.Kill(); }
                    catch { }
                    finally { p.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop GoodbyeDPI: {ex.Message}");
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync()
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            try
            {
                using var ts = new Microsoft.Win32.TaskScheduler.TaskService();
                if (ts.GetTask(GoodbyeDPITaskName) != null)
                {
                    ts.RootFolder.DeleteTask(GoodbyeDPITaskName);
                    Debug.WriteLine($"{GoodbyeDPITaskName} task deleted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete task: {ex.Message}");
            }
        }

        #endregion

        public async System.Threading.Tasks.Task SetStartWithWindowsAsync(bool enable)
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            string appName = AppConstants.AppName;
            string exePath = $"\"{Application.ExecutablePath}\"";

            using RegistryKey key = Registry.CurrentUser.OpenSubKey(
                AppConstants.RunKeyPath,
                writable: true
            );

            if (key == null)
                throw new InvalidOperationException("Startup registry key not found.");

            if (enable)
            {
                key.SetValue(appName, exePath);
            }
            else
            {
                if (key.GetValue(appName) != null)
                    key.DeleteValue(appName);
            }

        }

        public async System.Threading.Tasks.Task SetAutoRunOnLoginAsync(
    bool enable)
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            using var ts = new Microsoft.Win32.TaskScheduler.TaskService();
            var task = ts.GetTask(GoodbyeDPITaskName);

            if (task == null)
            {
                await CreateRunnerTaskAsync(ts, enable);
                return;
            }

            var td = task.Definition;

            td.Triggers.Clear();

            if (enable)
            {
                td.Triggers.Add(new LogonTrigger
                {
                    Enabled = true
                });
            }

            ts.RootFolder.RegisterTaskDefinition(
      GoodbyeDPITaskName,
      td,
      TaskCreation.CreateOrUpdate,
      "SYSTEM",
      null,
      TaskLogonType.ServiceAccount
  );

        }


        private async System.Threading.Tasks.Task CreateRunnerTaskAsync(
    Microsoft.Win32.TaskScheduler.TaskService ts,
    bool autoRunOnLogin)
        {
            if (!await PrivilegesHelper.EnsureAdministrator())
                return;

            var td = ts.NewTask();
            td.RegistrationInfo.Description = "Runs GoodbyeDPI with SYSTEM privileges";

            if (autoRunOnLogin)
            {
                td.Triggers.Add(new LogonTrigger
                {
                    Enabled = true
                });
            }

            td.Principal.RunLevel = TaskRunLevel.Highest;
            td.Principal.LogonType = TaskLogonType.ServiceAccount;
            td.Principal.UserId = "SYSTEM";

            string exePath = AppConstants.GoodbyeDPIPath;
            string workingDir = AppConstants.AppBaseDir;

            td.Actions.Add(new ExecAction(exePath, "", workingDir));

            td.Settings.AllowDemandStart = true;
            td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;

            ts.RootFolder.RegisterTaskDefinition(
      GoodbyeDPITaskName,
      td,
      TaskCreation.CreateOrUpdate,
      "SYSTEM",
      null,
      TaskLogonType.ServiceAccount
  );

        }

    }
}
