using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class PrivilegesHelper
	{
		public static bool IsAdministrator()
		{
			try
			{
				var identity = WindowsIdentity.GetCurrent();
				var principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch
			{
				return false;
			}
		}

		public static Task<bool> EnsureAdministrator(Action<string> onMessage)
		{
			if (IsAdministrator())
			{
				Task.Delay(1000);
				return Task.FromResult(true);
			}

			onMessage?.Invoke("Administrator privileges are required to continue.");

			try
			{
				TempConfigLoader.Current.AdminPriviligesRequested = true;
				TempConfigLoader.Save();

				var psi = new ProcessStartInfo
				{
					FileName = Application.ExecutablePath,
					UseShellExecute = true,
					Verb = "runas"
				};

				Process.Start(psi);
				onMessage?.Invoke("Application is restarting with administrator privileges...");
				Application.Exit();
			}
			catch
			{
				onMessage?.Invoke("User declined to grant administrator privileges.");
				TempConfigLoader.Reset_AdminPriviligesRequested();
			}

			return Task.FromResult(false);
		}
	}
}
