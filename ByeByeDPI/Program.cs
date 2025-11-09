using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ByeByeDPI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			TempConfigLoader.LoadSettings();
			bool skipMutexCheck = TempConfigLoader.Current.AdminPriviligesRequested;
			using (Mutex mutex = new Mutex(true, "ByeByeDPI_SingleInstance", out bool isNewInstance))
			{
				if (!isNewInstance && !skipMutexCheck)
				{
					using (var client = new NamedPipeClientStream(".", "ByeByeDPI_ShowWindow", PipeDirection.Out))
					{
						try
						{
							client.Connect(500);
							using (var writer = new System.IO.StreamWriter(client))
							{ writer.WriteLine("SHOW"); writer.Flush(); }
						}
						catch { }
					}
					return;
				}
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Task.Run(() => StartPipeServer());
				Application.Run(new TrayApplicationContext());
			}
		}
		private static void StartPipeServer()
		{
			while (true)
			{
				using (
					var server = new NamedPipeServerStream("ByeByeDPI_ShowWindow", PipeDirection.In))
				{
					server.WaitForConnection();
					using (var reader = new System.IO.StreamReader(server))
					{
						var command = reader.ReadLine(); if (command == "SHOW")
						{
							TrayApplicationContext.Instance?.ShowMainWindow();
						}
					}
				}
			}
		}
	}
}