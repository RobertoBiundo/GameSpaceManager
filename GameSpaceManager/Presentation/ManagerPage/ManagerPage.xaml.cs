using GameSpaceManager.Services.Interfaces;

namespace GameSpaceManager.Presentation.ManagerPage;

public sealed partial class ManagerPage : Page
{
    private readonly IDestinationFolderService _destinationFolderService;
    private readonly ISourceFolderServices _sourceFolderService;
    private readonly ITrackedFolderServices _trackedFolderService;

    public ManagerPage()
    {
        InitializeComponent();

        _sourceFolderService = App.Container.GetRequiredService<ISourceFolderServices>();
        _trackedFolderService = App.Container.GetRequiredService<ITrackedFolderServices>();
        _destinationFolderService = App.Container.GetRequiredService<IDestinationFolderService>();

        LoadFolders();
        LoadDiskInfo();
    }

    public ManagerPageViewModel ViewModel { get; set; } = new();

    /// <summary>
    ///     Asynchronously loads the folders from the source and tracked folder services
    /// </summary>
    private void LoadFolders()
    {
        ViewModel.FolderItems.Clear();

        var allFolders = new List<FolderItemModel>();

        // Load the required Service
        var sourceFolders = _sourceFolderService.GetSourceFolders();

        foreach (var sourceFolder in sourceFolders)
        {
            var subfolders = Directory.GetDirectories(sourceFolder.Path);

            allFolders.AddRange(subfolders.Select(s => new FolderItemModel
            {
                Name = Path.GetFileName(s),
                FullPath = s
            }));
        }

        // Load all of the archived folders
        var archivedFolders = _trackedFolderService.GetTrackedFolders();
        allFolders.AddRange(archivedFolders.Select(s => new FolderItemModel
        {
            Name = Path.GetFileName(s.OriginalPath),
            FullPath = s.OriginalPath,
            ArchivePath = s.ArchivePath
        }));

        // Sort and update the observable collection
        allFolders = allFolders.OrderBy(o => o.Name).ToList();
        ViewModel.FolderItems.AddRange(allFolders);

        // Get the sizes of the folders asynchronously
        _ = Task.WhenAll(ViewModel.FolderItems.Select(s => s.CalculateSizeAsync()));
    }

    /// <summary>
    ///     Get the disk information of the disks involved in the source folders and the destination folder.
    /// </summary>
    private void LoadDiskInfo()
    {
        ViewModel.DiskInfoModel.Clear();

        // Load the required Service
        var sourceFolders = _sourceFolderService.GetSourceFolders();
        var destinationFolder = _destinationFolderService.GetDestinationFolder()?.Path;

        // Figure out the disk path from the source folders
        var diskPaths = sourceFolders.Select(s => Path.GetPathRoot(s.Path)).Distinct().ToList();
        if (!string.IsNullOrEmpty(destinationFolder))
        {
            var destinationDiskPath = Path.GetPathRoot(destinationFolder);
            if (!diskPaths.Contains(destinationDiskPath))
                diskPaths.Add(destinationDiskPath);
        }

        diskPaths = diskPaths.Distinct().ToList();

        foreach (var diskPath in diskPaths)
        {
            // Skip if the disk path is null or empty
            if (string.IsNullOrEmpty(diskPath))
                continue;

            var driveInfo = new DriveInfo(diskPath);
            if (driveInfo.IsReady)
                ViewModel.DiskInfoModel.Add(new DiskInfoModel
                {
                    DisplayName = driveInfo.Name,
                    FreeBytes = driveInfo.AvailableFreeSpace,
                    TotalBytes = driveInfo.TotalSize
                });
        }
    }

    /// <summary>
    ///     Archives or restores a folder based on its current state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ArchiveOrRestore_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: FolderItemModel folderItem })
            return;

        var action = folderItem.IsArchived ? "Restoring" : "Archiving";
        var progressDialog = new ProgressDialog($"{action} '{folderItem.Name}'...")
        {
            XamlRoot = this.XamlRoot
        };
        progressDialog.ShowAsync();

        bool success;
        try
        {
            if (folderItem.IsArchived)
            {
                success = await Task.Run(() =>
                    _trackedFolderService.RestoreFolder(folderItem, (percent) =>
                        progressDialog.UpdateProgress(percent, $"Restoring '{folderItem.Name}'... {percent:F0}%")));
            }
            else
            {
                var archivedFolder = await Task.Run(() =>
                    _trackedFolderService.ArchiveFolder(folderItem, (percent) =>
                        progressDialog.UpdateProgress(percent, $"Archiving '{folderItem.Name}'... {percent:F0}%")));
                success = archivedFolder != null;
            }
        }
        finally
        {
            progressDialog.Hide();
        }

        var resultMsg = success
            ? $"{action} of folder '{folderItem.Name}' completed successfully."
            : $"Failed to {action.ToLower()} folder '{folderItem.Name}'.";
        await ShowMessageDialogAsync(resultMsg, action);
        LoadFolders();
    }

    private async Task ShowMessageDialogAsync(string message, string title)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = XamlRoot
        };
        await dialog.ShowAsync();
    }
}