using ByeByeDPI.Core;
using ByeByeDPI.Services;
using ReaLTaiizor.Forms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
    public partial class MainForm : PoisonForm
    {
        private readonly TrayApplicationContext _trayApplicationContext;
        private readonly MainFormViewModel _viewModel;
        private FormLayoutManager _layoutManager;

        public MainForm(TrayApplicationContext trayContext)
        {
            InitializeComponent();

            _viewModel = new MainFormViewModel(trayContext);
            _viewModel.OnMessage += MessageWriteLine;

            Load += async (_, _) =>
            {
                _viewModel.LoadCheckList();
                _viewModel.LoadParams();
                await Task.Delay(500);
                MessageWriteLine(_viewModel.IsGoodbyeDPIRunning
                    ? "GoodbyeDPI is running."
                    : "GoodbyeDPI is stopped.");
            };
        }

        private void MessageWriteLine(string msg)
        {
            MessagesListBox.Items.Add(msg);
            MessagesListBox.TopIndex = MessagesListBox.Items.Count - 1;
        }

        private async void ToggleDPIBtn_Click(object sender, EventArgs e)
        {
            MessagesListBox.Items.Clear();
            await _viewModel.ToggleGoodbyeDPIAsync();
        }

        private async void CheckDomainsBtn_Click(object sender, EventArgs e)
        {
            MessagesListBox.Items.Clear();
            await _viewModel.BeginCheckDomainListAsync();
        }

        private async void ResetBtn_Click(object sender, EventArgs e)
        {
            await _viewModel.ClearSelectedParam();
        }

        public void ResetFormPositionAndSize() => _layoutManager.ResetToDefault();

    }
}
