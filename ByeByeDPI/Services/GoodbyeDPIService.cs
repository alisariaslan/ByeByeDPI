using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ByeByeDPI.Services
{
    public class GoodbyeDPIService
    {
        private const string GoodbyeDPITaskName = "GoodbyeDPI_Runner";

        public event Action<string> OnMessage;

        public bool IsRunning =>
            Process.GetProcessesByName("goodbyedpi").Any();

        #region Public API

        public async System.Threading.Tasks.Task ToggleAsync(
            string exePath,
            string? chosenParamName,
            Func<string, string?> paramResolver,
            Func<System.Threading.Tasks.Task> runParamSelectionWorkflow)
        {
            if (IsRunning)
            {
                await StopAsync();
                return;
            }

            if (!string.IsNullOrWhiteSpace(chosenParamName))
            {
                var param = paramResolver(chosenParamName);
                await StartAsync(exePath, param);
            }
            else
            {
                await runParamSelectionWorkflow();
            }
        }

        public async System.Threading.Tasks.Task StartAsync(string exePath, string arguments = "")
        {
            if (IsRunning)
            {
                OnMessage?.Invoke("GoodbyeDPI is already running.");
                return;
            }

            if (!await PrivilegesHelper.EnsureAdministrator(OnMessage))
                return;

            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(GoodbyeDPITaskName);

                if (task == null)
                {
                    await CreateRunnerTaskAsync(ts, exePath);
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

                ts.RootFolder.RegisterTaskDefinition(GoodbyeDPITaskName, task.Definition);
                task.Run();

                OnMessage?.Invoke($"GoodbyeDPI started with parameters: {arguments}");
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Failed to start GoodbyeDPI: {ex.Message}");
            }
        }

        public async System.Threading.Tasks.Task StopAsync()
        {
            if (!await PrivilegesHelper.EnsureAdministrator(OnMessage))
                return;

            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(GoodbyeDPITaskName);

                if (task?.State == TaskState.Running)
                {
                    task.Stop();
                    OnMessage?.Invoke("GoodbyeDPI task stopped.");
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
                OnMessage?.Invoke($"Failed to stop GoodbyeDPI: {ex.Message}");
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync()
        {
            if (!await PrivilegesHelper.EnsureAdministrator(OnMessage))
                return;

            try
            {
                using var ts = new TaskService();
                if (ts.GetTask(GoodbyeDPITaskName) != null)
                {
                    ts.RootFolder.DeleteTask(GoodbyeDPITaskName);
                    OnMessage?.Invoke("GoodbyeDPI task deleted.");
                }
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Failed to delete task: {ex.Message}");
            }
        }

        #endregion

        #region Internal

        private async System.Threading.Tasks.Task CreateRunnerTaskAsync(TaskService ts, string exePath)
        {
            if (!await PrivilegesHelper.EnsureAdministrator(OnMessage))
                return;

            var td = ts.NewTask();
            td.RegistrationInfo.Description = "Runs GoodbyeDPI with SYSTEM privileges";

            td.Triggers.Add(new LogonTrigger());

            td.Principal.RunLevel = TaskRunLevel.Highest;
            td.Principal.LogonType = TaskLogonType.ServiceAccount;
            td.Principal.UserId = "SYSTEM";

            string workingDir = Path.GetDirectoryName(exePath)
                ?? AppDomain.CurrentDomain.BaseDirectory;

            td.Actions.Add(new ExecAction(exePath, "", workingDir));
            td.Settings.AllowDemandStart = true;
            td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;

            ts.RootFolder.RegisterTaskDefinition(GoodbyeDPITaskName, td);
        }

        #endregion
    }
}
