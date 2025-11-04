using System;
using System.Windows.Forms;

namespace ByeByeDPI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var viewModel = new Form1ViewModel();
			var view = new MainForm(viewModel);
			Application.Run(view);
		}
	}
}
