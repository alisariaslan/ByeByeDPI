using ByeByeDPI.Core;
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

		public static Task<bool> EnsureAdministrator()
		{
			if (IsAdministrator())
			{
				Task.Delay(1000);
				return Task.FromResult(true);
			}

            Debug.WriteLine("Administrator privileges are required to continue.");

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
                Debug.WriteLine("Application is restarting with administrator privileges...");
				TrayApplicationContext.Instance?.ExitApplication();
			}
			catch
			{
                Debug.WriteLine("User declined to grant administrator privileges.");
				TempConfigLoader.Current.AdminPriviligesRequested = false;
				TempConfigLoader.Save();
			}

			return Task.FromResult(false);
		}
	}
}
