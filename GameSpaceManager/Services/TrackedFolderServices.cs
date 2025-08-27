using DataAccessLayer.DataAccess.Stores.Interfaces;
using DataAccessLayer.Entities;
using GameSpaceManager.Presentation.ManagerPage;
using GameSpaceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameSpaceManager.Services;

public class TrackedFolderServices(IStore store, IDestinationFolderService destinationFolderService) : ITrackedFolderServices
{
    /// <inheritDoc />
    public async Task<TrackedFolderEntity?> ArchiveFolder(FolderItemModel folderToArchive)
    {
        var destinationFolder = destinationFolderService.GetDestinationFolder();

        if (destinationFolder == null)
            return null;

        // We need to create a unique name for the destination folder using the last path + tracked folder ID
        var archiveId = Guid.NewGuid();
        var destinationFolderName = $"{folderToArchive.Name}_{archiveId.ToString()}";
        var archivePath = Path.Combine(destinationFolder.Path, destinationFolderName);

        // Move folder to archive location
        var trackedFolder = new TrackedFolderEntity
        {
            Id = archiveId,
            OriginalPath = folderToArchive.FullPath,
            ArchivePath = archivePath
        };

        await store.AddAsync(trackedFolder);
        await store.SaveAsync();

        try
        {
            // Move the folder to the archive location
            if (Directory.Exists(folderToArchive.FullPath))
            {
                CopyDirectory(folderToArchive.FullPath, archivePath);
                Directory.Delete(folderToArchive.FullPath, true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            // Delete the tracked folder entry if the move fails
            Directory.Delete(archivePath, true);
            store.Remove(trackedFolder);
            await store.SaveAsync();
            return null;
        }

        return trackedFolder;
    }

    /// <inheritDoc />
    public async Task<bool> RestoreFolder(FolderItemModel folderToRestore)
    {
        // Find the tracked folder entity by its ID
        var trackedFolder = await store.TrackedFolderRepository.QueryMany()
            .FirstOrDefaultAsync(f => string.Equals(f.OriginalPath, folderToRestore.FullPath));

        if (trackedFolder == null)
            return false;

        // Restore the folder by moving it back to its original location
        try
        {
            // Check if the archive path exists
            if (!Directory.Exists(trackedFolder.ArchivePath))
                return false;

            // If the original Path exists. Clean it up and put the folder back
            if (Directory.Exists(trackedFolder.OriginalPath))
                Directory.Delete(trackedFolder.OriginalPath, true);

            // Move the folder back to its original location
            if (!Directory.Exists(trackedFolder.OriginalPath))
                Directory.CreateDirectory(trackedFolder.OriginalPath);

            CopyDirectory(trackedFolder.ArchivePath, trackedFolder.OriginalPath);
            Directory.Delete(trackedFolder.ArchivePath, true);
        }
        catch (Exception)
        {
            return false;
        }

        // Remove the tracked folder entry from the database
        store.Remove(trackedFolder);
        await store.SaveAsync();

        return true;
    }

    /// <inheritDoc />
    public IQueryable<TrackedFolderEntity> GetTrackedFolders()
    {
        return store.TrackedFolderRepository.QueryMany();
    }

    /// <summary>
    /// Copies all files and subdirectories from the source directory to the destination directory.
    /// </summary>
    /// <param name="sourceDir">The source directory to copy from.</param>
    /// <param name="destDir">The destination directory to copy to.</param>
    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
            CopyDirectory(dir, destSubDir);
        }
    }
}