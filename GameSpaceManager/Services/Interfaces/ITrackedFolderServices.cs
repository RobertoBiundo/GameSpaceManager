using DataAccessLayer.Entities;
using GameSpaceManager.Presentation.ManagerPage;

namespace GameSpaceManager.Services.Interfaces;

public interface ITrackedFolderServices
{
    delegate void CopyProgressCallback(double percentComplete);

    /// <summary>
    ///     Archives a folder by moving it to the archive location
    /// </summary>
    /// <param name="folderToArchive">The folder to archive</param>
    /// <param name="progressCallback">The callback to report progress updates</param>
    /// <returns>The archived folder entity or null if the operation failed</returns>
    Task<TrackedFolderEntity?> ArchiveFolder(FolderItemModel folderToArchive, CopyProgressCallback? progressCallback = null);

    /// <summary>
    ///     Restores a folder by moving it back to its original location
    /// </summary>
    /// <param name="folderToRestore">The folder to restore</param>
    /// <param name="progressCallback">The callback to report progress updates</param>
    /// <returns>The success status of the restore operation</returns>
    Task<bool> RestoreFolder(FolderItemModel folderToRestore, CopyProgressCallback? progressCallback = null);

    /// <summary>
    ///     Gets all tracked folders from the database
    /// </summary>
    /// <returns>A queryable of tracked folders</returns>
    IQueryable<TrackedFolderEntity> GetTrackedFolders();
}