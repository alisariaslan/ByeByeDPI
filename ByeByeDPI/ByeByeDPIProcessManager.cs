using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ByeByeDPI
{
	public class ByeByeDPIProcessManager : IDisposable
	{
		private Process _process;
		public event Action<string> OnMessage;

		/// <summary>
		/// Checks if the process is running either via local reference or via process list
		/// </summary>
		public bool IsRunning
		{
			get
			{
				// If local process exists and not exited
				if (_process != null && !_process.HasExited)
					return true;

				// Check system process list
				var running = Process.GetProcessesByName("goodbyedpi").Any();
				return running;
			}
		}

		public async Task StartAsync(string exePath, string arguments = "")
		{
			if (IsRunning)
			{
				OnMessage?.Invoke("ByeByeDPI is already running.");
				return;
			}

			await Task.Run(() =>
			{
				try
				{
					var psi = new ProcessStartInfo
					{
						FileName = exePath,
						Arguments = arguments,
						WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						CreateNoWindow = true
					};

					_process = new Process
					{
						StartInfo = psi,
						EnableRaisingEvents = true
					};

					_process.Exited += (s, e) =>
					{
						OnMessage?.Invoke("ByeByeDPI exited.");
						try { _process?.Dispose(); } catch { }
						_process = null;
					};

					bool started = _process.Start();
					if (started)
					{
						_process.BeginOutputReadLine();
						_process.BeginErrorReadLine();
						_process.OutputDataReceived += (s, e) => { /* silent */ };
						_process.ErrorDataReceived += (s, e) => { /* silent */ };

						OnMessage?.Invoke($"ByeByeDPI started (hidden): {arguments}");
					}
					else
					{
						OnMessage?.Invoke("Failed to start ByeByeDPI process.");
					}
				}
				catch (Exception ex)
				{
					OnMessage?.Invoke("ByeByeDPI couldn't run: " + ex.Message);
				}
			});
		}

		public async Task StopAsync()
		{
			await Task.Run(() =>
			{
				try
				{
					// Stop local process if exists
					if (_process != null && !_process.HasExited)
					{
						try { _process.CloseMainWindow(); } catch { }
						if (!_process.WaitForExit(2000))
						{
							_process.Kill();
							_process.WaitForExit(3000);
						}
						OnMessage?.Invoke("ByeByeDPI stopped.");
						try { _process?.Dispose(); } catch { }
						_process = null;
					}

					// Also stop any other running GoodbyeDPI processes from system
					var others = Process.GetProcessesByName("goodbyedpi");
					foreach (var p in others)
					{
						try
						{
							p.Kill();
							p.WaitForExit(3000);
							OnMessage?.Invoke("ByeByeDPI process from system stopped.");
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
					OnMessage?.Invoke("Failed to stop ByeByeDPI: " + ex.Message);
				}
			});
		}

		public void Dispose()
		{
			try
			{
				if (_process != null && !_process.HasExited)
					_process.Kill();
			}
			catch { }
			finally
			{
				try { _process?.Dispose(); } catch { }
				_process = null;
			}
		}
	}
}
