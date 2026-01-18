using System;
using System.IO;

namespace ByeByeDPI.Constants
{
	public static class AppConstants
	{

		public static readonly string AppName = "ByeByeDPI";

		public static readonly string AppBaseDir = AppDomain.CurrentDomain.BaseDirectory;

		public static readonly string UserDataDir =
	   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

        public static readonly TimeSpan ApplicationUpdateInterval = TimeSpan.FromHours(2);


        private const string _goodbyeDPIFileName = "goodbyedpi.exe";
		private const string _domainListFileName = "checklist.json";
		private const string _paramsFileName = "params.json";
		private const string _appSettingsFileName = "settings.json";
		private const string _tempConfigsFileName = "tempconfigs.json";


		public static string GoodbyeDPIPath => Path.Combine(AppBaseDir, _goodbyeDPIFileName);
		public static string DomainListPath => Path.Combine(UserDataDir, _domainListFileName);
		public static string ParamsPath => Path.Combine(UserDataDir, _paramsFileName);
		public static string AppSettingsPath => Path.Combine(UserDataDir, _appSettingsFileName);
		public static string TempConfigsPath => Path.Combine(UserDataDir, _tempConfigsFileName);


	}
}
