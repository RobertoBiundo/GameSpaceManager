using Windows.UI.Core;

namespace GameSpaceManager.Presentation.Shared
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