using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace ByeByeDPI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			if (!IsAdministrator())
			{
				// Admin olarak yeniden başlat
				var proc = new ProcessStartInfo
				{
					FileName = Application.ExecutablePath,
					UseShellExecute = true,
					Verb = "runas" // admin yetkisi ister
				};
				try
				{
					Process.Start(proc);
				}
				catch
				{
					MessageBox.Show("Uygulama admin olarak başlatılmadı!");
				}
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var viewModel = new Form1ViewModel();
			var view = new Form1(viewModel);
			Application.Run(view);
		}

		static bool IsAdministrator()
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
