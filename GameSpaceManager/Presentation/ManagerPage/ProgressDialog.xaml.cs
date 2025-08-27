using System.Windows;
using Windows.UI.Core;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;

namespace GameSpaceManager.Presentation.ManagerPage
{
    public partial class ProgressDialog : ContentDialog
    {
        public ProgressDialog(string initialStatus)
        {
            InitializeComponent();
            StatusText.Text = initialStatus;
            ProgressBar.Value = 0;
        }

        public async void UpdateProgress(double percent, string? status = null)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressBar.Value = percent;
                if (!string.IsNullOrEmpty(status))
                    StatusText.Text = status;
            });
        }
    }
}