using ByeByeDPI.Constants;
using ByeByeDPI.Models;
using ByeByeDPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI.Core
{
    public class MainFormViewModel : IDisposable
    {
        public event Action<string> OnMessage;

        public bool IsGoodbyeDPIRunning => _goodbyeDPIService.IsRunning;
        public bool IsLocked { get; private set; }

        private readonly GoodbyeDPIService _goodbyeDPIService;
        private readonly HttpClient _httpClient;

        private List<CheckListWrapperModel> _checkList = new();
        private List<ParamModel> _paramList = new();

        private bool _isWorkflowRunning;
        private bool _isCheckListRunning;
        private bool _isDisposed;

        public MainFormViewModel(TrayApplicationContext trayApplicationContext)
        {
            _goodbyeDPIService = new GoodbyeDPIService();
            _goodbyeDPIService.OnMessage += msg => OnMessage?.Invoke(msg);

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        #region Loaders

        public void LoadCheckList()
        {
            _checkList = CheckListLoader.LoadCheckList()
                .Select(x => new CheckListWrapperModel { Item = x })
                .ToList();

            OnMessage?.Invoke("Check list loaded.");
        }

        public void LoadParams()
        {
            _paramList = ParamsLoader.LoadParams();
            OnMessage?.Invoke("Parameters loaded.");
        }

        #endregion

        private string? GetParamValue(string paramName) =>
            _paramList.FirstOrDefault(x => x.Name == paramName)?.Param;

        #region Domain Check

        public async Task BeginCheckDomainListAsync()
        {
            if (_isCheckListRunning) return;
            _isCheckListRunning = true;

            foreach (var wrapper in _checkList)
            {
                if (_isDisposed) break;

                bool accessible = false;
                var url = wrapper.Item.Url.StartsWith("www.")
                    ? wrapper.Item.Url
                    : "www." + wrapper.Item.Url;

                try
                {
                    var response = await _httpClient.GetAsync("https://" + url);
                    accessible = response.IsSuccessStatusCode;
                }
                catch { }

                wrapper.IsAccesible = accessible;
                OnMessage?.Invoke($"Is {wrapper.Item.Name} accessible? {(accessible ? "✅" : "❌")}");
            }

            _isCheckListRunning = false;
        }

        #endregion

        #region DPI Control

        public async Task ToggleGoodbyeDPIAsync()
        {
            await _goodbyeDPIService.ToggleAsync(
                AppConstants.GoodbyeDPIPath,
                SettingsLoader.Current.ChosenParam,
                GetParamValue,
                RunParamSelectionWorkflowAsync);
        }

        public async Task ClearSelectedParam()
        {
            await _goodbyeDPIService.StopAsync();
            await _goodbyeDPIService.DeleteTaskAsync();

            SettingsLoader.Current.ChosenParam = "";
            SettingsLoader.Save();

            OnMessage?.Invoke("Selected parameter cleared.");
        }

        private async Task RunParamSelectionWorkflowAsync()
        {
            if (_isWorkflowRunning || _isCheckListRunning)
                return;

            _isWorkflowRunning = true;

            foreach (var item in _paramList)
            {
                OnMessage?.Invoke($"Trying parameter '{item.Name}'...");
                await _goodbyeDPIService.StartAsync(AppConstants.GoodbyeDPIPath, item.Param);
                await BeginCheckDomainListAsync();

                var result = MessageBox.Show(
                    $"Parameter '{item.Name}' applied.\nIs everything accessible?",
                    "Test Parameter",
                    MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SettingsLoader.Current.ChosenParam = item.Name;
                    SettingsLoader.Save();
                    break;
                }

                await _goodbyeDPIService.StopAsync();
                if (result == DialogResult.Cancel)
                    break;
            }

            _isWorkflowRunning = false;
            OnMessage?.Invoke("Parameter selection workflow completed.");
        }

        #endregion

        public void Dispose()
        {
            _httpClient.Dispose();
            _isDisposed = true;
        }
    }
}
