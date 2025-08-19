using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using DataAccessLayer.Entities;
using GameSpaceManager.Services.Interfaces;

namespace GameSpaceManager.Presentation;

public sealed partial class SettingsPage : Page
{
    private readonly IDestinationFolderService _destinationFolderService;

    private readonly ObservableCollection<SourceFolderEntity> _sourceFolders = [];
    private readonly ISourceFolderServices _sourceFolderService;

    public SettingsPage()
    {
        // Load the required Service
        _sourceFolderService = App.Container.GetRequiredService<ISourceFolderServices>();
        _destinationFolderService = App.Container.GetRequiredService<IDestinationFolderService>();

        InitializeComponent();
        SourceFoldersList.ItemsSource = _sourceFolders;
        DestinationFolderTextBox.Text = _destinationFolderService.GetDestinationFolder()?.Path ?? "No destination folder set";

        // Load existing settings from the services
        _sourceFolders.AddRange(_sourceFolderService.GetSourceFolders().ToList());
    }

    /// <summary>
    ///     Adds a new source folder to the list and database/service.
    /// </summary>
    private async void AddSourceFolder_Click(object sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync();

        if (string.IsNullOrEmpty(folder))
        {
            Console.WriteLine("Invalid folder path");
            return;
        }

        var sourceFolder = await _sourceFolderService.AddSourceFolder(folder);
        if (sourceFolder != null)
            _sourceFolders.Add(sourceFolder);
        else
            Console.WriteLine("Failed to add source folder. Please try again.");
    }

    /// <summary>
    ///     Removes a source folder from the list and database/service.
    /// </summary>
    private void RemoveSourceFolder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not SourceFolderEntity sourceFolder)
            return;

        _sourceFolderService.DeleteSourceFolder(sourceFolder.Id);
        _sourceFolders.Remove(sourceFolder);
    }

    /// <summary>
    ///     Lets a user browse for a destination folder
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void BrowseDestination_Click(object sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync();

        if (string.IsNullOrEmpty(folder))
        {
            Console.WriteLine("Invalid folder path");
            return;
        }

        var destinationFolder = await _destinationFolderService.SetDestinationFolder(folder);

        if (destinationFolder == null)
            Console.WriteLine("Failed to set destination folder. Please try again.");

        else
            DestinationFolderTextBox.Text = destinationFolder.Path;
    }

    // Simple folder picker
    private async Task<string?> PickFolderAsync()
    {
        var picker = new FolderPicker
        {
            SuggestedStartLocation = PickerLocationId.Desktop
        };
        picker.FileTypeFilter.Add("*");

        var folder = await picker.PickSingleFolderAsync();
        return folder?.Path;
    }
}