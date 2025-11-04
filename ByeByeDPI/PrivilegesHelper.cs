using System.Security.Principal;

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
	}
}
