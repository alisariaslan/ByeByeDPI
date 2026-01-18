using ByeByeDPI.Constants;
using ByeByeDPI.Models;
using ByeByeDPI.Services;
using ByeByeDPI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI.Core
{

    public class MainFormViewModel : IDisposable
    {
        public event Action<string> OnStatusChanged;
        public event Action<string> OnMessage;
        public event Action<FormStage> OnStageChanged;
        public event Action<int, int> OnProgressChanged;
        public event Action OnClearRequested;
        public bool IsGoodbyeDPIRunning => _goodbyeDPIService.IsRunning;
        private FormStage _currentStage = FormStage.Toggle;
        public FormStage CurrentStage
        {
            get => _currentStage;
            private set { _currentStage = value; OnStageChanged?.Invoke(value); }
        }
        private readonly GoodbyeDPIService _goodbyeDPIService;
        private List<CheckListWrapperModel> _checkList = new();
        private List<ParamModel> _paramList = new();
        private int _currentParamIndex = -1;

        public MainFormViewModel(TrayApplicationContext trayApplicationContext)
        {
            _goodbyeDPIService = new GoodbyeDPIService();
            _goodbyeDPIService.OnMessage += msg => OnMessage?.Invoke(msg);
        }

        /// <summary>
        /// Loads checklist and parameter list
        /// </summary>
        public void LoadData()
        {
            _checkList = DomainLoader.LoadDomains().Select(x => new CheckListWrapperModel { Item = x }).ToList();
            _paramList = ParamsLoader.LoadParams();
        }

        /// <summary>
        /// Sets the current form stage
        /// </summary>
        /// <param name="newStage"></param>
        public void SetCurrentStage(FormStage newStage)
        {
            CurrentStage = newStage;
        }

        /// <summary>
        /// Toggles GoodbyeDPI with either the saved parameter or starts parameter testing workflow automatically
        /// </summary>
        public async Task<int> ToggleGoodbyeDPIAsync()
        {
            var savedParam = SettingsLoader.Current.ChosenParam;
            var param = _paramList.FirstOrDefault(x => x.Name == savedParam);
            if (param != null)
            {
                await _goodbyeDPIService.ToggleAsync(
                    AppConstants.GoodbyeDPIPath,
                    param.Name,
                    param.Value
                );
                return 0;
            }
            else if (param == null && IsGoodbyeDPIRunning)
            {
                await CleanupAsync();
                return 2;
            }
            if (_paramList == null || _paramList.Count == 0)
            {
                throw new Exception("No parameter list available for testing!");
            }
            Debug.WriteLine("🔍 Scanning for a suitable parameter automatically...");
            _currentParamIndex = 0;
            bool foundWorking = false;
            while (_currentParamIndex < _paramList.Count)
            {
                var currentParam = _paramList[_currentParamIndex];
                Debug.WriteLine($"⚡ Trying ({_currentParamIndex + 1}/{_paramList.Count}): {currentParam.Name}");
                bool isSuccess = await TestCurrentParameterAsync();
                if (isSuccess)
                {
                    foundWorking = true;
                    Debug.WriteLine($"✅ Working parameter found: {currentParam.Name}");
                    break;
                }
                _currentParamIndex++;
            }
            if (foundWorking)
                return 1;
            else
            {
                _currentParamIndex = -1;
                return -1;
            }
        }

        /// <summary>
        /// Tests the current parameter in the list
        /// </summary>
        /// <returns></returns>
        private async Task<bool> TestCurrentParameterAsync()
        {
            CurrentStage = FormStage.Loading;
            if (_currentParamIndex < 0 || _currentParamIndex >= _paramList.Count)
                return false;
            var item = _paramList[_currentParamIndex];
            try
            {
                await _goodbyeDPIService.StartAsync(
                    AppConstants.GoodbyeDPIPath,
                    item.Name,
                    item.Value
                );
                foreach (var citem in _checkList)
                    citem.IsAccesible = false;
                using var httpClient = HttpClientFactory.CreateNoRedirectClient();
                Debug.WriteLine($"Warmup started: {item.Name}");
                bool ready = await NetworkWarmupHelper.WaitAsync(httpClient, OnStatusChanged);
                if (!ready)
                {
                    Debug.WriteLine($"Warmup failed: {item.Name}");
                    await CleanupAsync();
                    return false;
                }
                Debug.WriteLine($"Warmup succeeded: {item.Name}");
                OnClearRequested?.Invoke();
                OnMessage?.Invoke($"--> {item.Name} <--");
                await AccessibilityChecker.CheckAsync(
                    httpClient,
                    _checkList,
                    OnMessage,
                    OnProgressChanged,
                    OnStatusChanged
                );
                await CleanupAsync();
                OnProgressChanged?.Invoke(0, 1);
                CurrentStage = FormStage.Result;
                Debug.WriteLine($"Test completed: {item.Name}");
                return true;
            }
            catch (TaskCanceledException )
            {
                Debug.WriteLine($"⚠ Warmup or network request timed out: {item.Name}");
                await CleanupAsync();
                CurrentStage = FormStage.Result;
                return false;
            }
            catch (Exception)
            {
                await CleanupAsync();
                CurrentStage = FormStage.Result;
                throw;
            }
        }

        /// <summary>
        /// Cleans up after testing a parameter
        /// </summary>
        /// <returns></returns>
        private async Task CleanupAsync()
        {
            await _goodbyeDPIService.StopAsync();
            await _goodbyeDPIService.DeleteTaskAsync();
        }

        /// <summary>
        /// Returns true if no more parameters to test
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestNextParameterAsync()
        {
            _currentParamIndex++;
            bool foundWorking = false;
            while (_currentParamIndex < _paramList.Count)
            {
                var currentParam = _paramList[_currentParamIndex];
                bool isSuccess = await TestCurrentParameterAsync();
                if (isSuccess)
                {
                    foundWorking = true;
                    break;
                }
                _currentParamIndex++;
            }
            if (!foundWorking)
                _currentParamIndex = -1;
            return !foundWorking;
        }

        /// <summary>
        /// Gets the statistics of reachable and unreachable checklist items
        /// </summary>
        /// <returns></returns>
        public (int reachable, int unreachable) GetAccessibilityStats()
        {
            int reachable = 0;
            int unreachable = 0;
            foreach (var item in _checkList)
            {
                if (item.IsAccesible)
                    reachable++;
                else
                    unreachable++;
            }
            return (reachable, unreachable);
        }

        /// <summary>
        /// Applies the current parameter as chosen parameter in settings
        /// </summary>
        /// <returns></returns>
        public async Task ApplyCurrentParameter()
        {
            var item = _paramList[_currentParamIndex];
            SettingsLoader.Current.ChosenParam = item.Name;
            SettingsLoader.Save();
            await ToggleGoodbyeDPIAsync();
        }

        /// <summary>
        /// Resets the workflow to initial state
        /// </summary>
        /// <returns></returns>
        public async Task ResetWorkflowAsync()
        {
            await _goodbyeDPIService.StopAsync();
            await _goodbyeDPIService.DeleteTaskAsync();
            SettingsLoader.Current.ChosenParam = "";
            SettingsLoader.Save();
            _currentParamIndex = -1;
            OnProgressChanged?.Invoke(0, 1);
        }

        public ParamModel GetParamByName(string name)
        {
            return _paramList.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task ToggleGoodbyeDPIAsync(string paramName, string paramValue)
        {
            await _goodbyeDPIService.StartAsync(AppConstants.GoodbyeDPIPath, paramName, paramValue);
        }
        public async Task<bool> TestParamByNameAsync(string paramName)
        {
            var index = _paramList.FindIndex(x => x.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));
            if (index < 0)
                return false;

            _currentParamIndex = index;
            return await TestCurrentParameterAsync();
        }


        public void Dispose()
        {
        }
    }
}
