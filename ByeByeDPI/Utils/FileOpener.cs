using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public static class FileOpener
	{
		/// <summary>
		/// Opens the specified file with the default application.
		/// </summary>
		/// <param name="filePath">Full path of the file to open</param>
		public static void OpenFile(string filePath)
		{
			try
			{
				if (!File.Exists(filePath))
				{
					MessageBox.Show("File not found: " + filePath);
					return;
				}

				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = filePath,
					UseShellExecute = true
				};

				Process.Start(psi);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to open file: " + ex.Message);
			}
		}

		/// <summary>
		/// Opens a file located in the application's base directory.
		/// </summary>
		/// <param name="fileName">File name</param>
		public static void OpenFileInBaseDir(string fileName)
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
			OpenFile(filePath);
		}
	}
}
