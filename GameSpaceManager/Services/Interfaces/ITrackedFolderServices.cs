using DataAccessLayer.Entities;
using GameSpaceManager.Presentation.ManagerPage;

namespace GameSpaceManager.Services.Interfaces;

public interface ITrackedFolderServices
{
    /// <summary>
    ///     Archives a folder by moving it to the archive location
    /// </summary>
    /// <param name="folderToArchive">The folder to archive</param>
    /// <returns>The archived folder entity or null if the operation failed</returns>
    Task<TrackedFolderEntity?> ArchiveFolder(FolderItemModel folderToArchive);

    /// <summary>
    ///     Restores a folder by moving it back to its original location
    /// </summary>
    /// <param name="folderToRestore">The folder to restore</param>
    /// <returns>The success status of the restore operation</returns>
    Task<bool> RestoreFolder(FolderItemModel folderToRestore);

    /// <summary>
    ///     Gets all tracked folders from the database
    /// </summary>
    /// <returns>A queryable of tracked folders</returns>
    IQueryable<TrackedFolderEntity> GetTrackedFolders();
}