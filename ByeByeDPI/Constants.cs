using System;
using System.IO;

namespace ByeByeDPI
{
	public static class Constants
	{

		public static readonly string RegistryAppName = "ByeByeDPI";


		public static readonly string AppBaseDir = AppDomain.CurrentDomain.BaseDirectory;


		private const string _goodbyeDPIFileName = "goodbyedpi.exe";
		private const string _checkListFileName = "checklist.json";
		private const string _paramsFileName = "params.json";
		private const string _appSettingsFileName = "settings.json";


		public static string GoodbyeDPIPath => Path.Combine(AppBaseDir, _goodbyeDPIFileName);
		public static string CheckListPath => Path.Combine(AppBaseDir, _checkListFileName);
		public static string ParamsPath => Path.Combine(AppBaseDir, _paramsFileName);
		public static string AppSettingsPath => Path.Combine(AppBaseDir, _appSettingsFileName);


		public static string GoodbyeDPIFileName => _goodbyeDPIFileName;
		public static string CheckListFileName => _checkListFileName;
		public static string ParamsFileName => _paramsFileName;
		public static string AppSettingsFileName => _appSettingsFileName;
	}
}
