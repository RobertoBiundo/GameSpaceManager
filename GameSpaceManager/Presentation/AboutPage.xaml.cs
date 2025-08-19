// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using Windows.System;

namespace GameSpaceManager.Presentation;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AboutPage : Page
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void DonateButton_Click(object sender, RoutedEventArgs e)
    {
        // Open the donation link in the default web browser
        var uri = new Uri("https://paypal.com/donate/?hosted_button_id=VLJA62KL2TGR4");
        Launcher.LaunchUriAsync(uri).GetAwaiter().GetResult();
    }
}