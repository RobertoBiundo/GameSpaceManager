namespace GameSpaceManager.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();

        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        // Set Manager menu item as selected on load
        NavView.SelectedItem = ManagerMenuItem;

        // Navigate to ManagerPage
        ContentFrame.Navigate(typeof(ManagerPage.ManagerPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (!args.IsSettingsSelected)
        {
            if (args.SelectedItem is NavigationViewItem item)
                switch (item.Tag)
                {
                    case "manager":
                        ContentFrame.Navigate(typeof(ManagerPage.ManagerPage));
                        break;

                    case "about":
                        ContentFrame.Navigate(typeof(AboutPage));
                        break;
                }
        }
        else
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
    }
}