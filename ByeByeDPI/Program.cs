using System;
using System.Threading;
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
					MessageBox.Show("App is already running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				var viewModel = new Form1ViewModel();
				var view = new MainForm(viewModel);
				Application.Run(view);
			}
		}
	}
}
