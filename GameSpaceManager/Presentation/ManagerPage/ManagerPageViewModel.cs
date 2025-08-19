using System.Collections.ObjectModel;

namespace GameSpaceManager.Presentation.ManagerPage;

public class ManagerPageViewModel
{
    /// <summary>
    ///     A model representing the folder items
    /// </summary>
    public ObservableCollection<FolderItemModel> FolderItems { get; set; } = [];

    /// <summary>
    ///     A model representing the disk information
    /// </summary>
    public ObservableCollection<DiskInfoModel> DiskInfoModel { get; set; } = [];
}