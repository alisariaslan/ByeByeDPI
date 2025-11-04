using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public partial class InfoDialog : Form
	{
		public InfoDialog(string title, string htmlContent)
		{
			InitializeComponent();
			this.Text = title;

			// WebBrowser kontrolü temiz şekilde html yükleme
			webBrowser1.DocumentText = htmlContent;
		}

		private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			// Herhangi bir linke tıklanınca tarayıcıda aç ve gömülü gezgiyi kullanma
			if (e.Url != null && e.Url.Scheme != "about")
			{
				e.Cancel = true;
				try
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = e.Url.ToString(),
						UseShellExecute = true
					});
				}
				catch { }
			}
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
