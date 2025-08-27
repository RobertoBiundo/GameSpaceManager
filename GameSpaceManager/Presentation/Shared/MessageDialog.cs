namespace GameSpaceManager.Presentation.Shared;

public static class MessageDialog
{
    /// <summary>
    ///     A helper extension method to show a simple message dialog
    /// </summary>
    /// <param name="xamlRoot">The XamlRoot to attach the dialog to</param>
    /// <param name="message">The message to display</param>
    /// <param name="title">The title of the dialog</param>
    public static async Task ShowMessageDialogAsync(this XamlRoot xamlRoot, string message, string title)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = xamlRoot
        };
        await dialog.ShowAsync();
    }
}