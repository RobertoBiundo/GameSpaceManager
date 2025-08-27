using DataAccessLayer.DataAccess.Stores.Interfaces;
using DataAccessLayer.Entities;
using GameSpaceManager.Presentation.ManagerPage;
using GameSpaceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameSpaceManager.Services;

public class TrackedFolderServices(IStore store, IDestinationFolderService destinationFolderService) : ITrackedFolderServices
{
    /// <inheritDoc />
    public async Task<TrackedFolderEntity?> ArchiveFolder(FolderItemModel folderToArchive, ITrackedFolderServices.CopyProgressCallback? progressCallback = null)
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
            if (Directory.Exists(folderToArchive.FullPath))
            {
                // If the source and destination are on the same volume, use Move. Otherwise, copy and delete.
                var sourceRoot = Path.GetPathRoot(folderToArchive.FullPath);
                var destRoot = Path.GetPathRoot(archivePath);
                if (string.Equals(sourceRoot, destRoot, StringComparison.OrdinalIgnoreCase))
                {
                    Directory.Move(folderToArchive.FullPath, archivePath);
                    progressCallback?.Invoke(100.0);
                }
                else
                {
                    CopyDirectoryRecursively(folderToArchive.FullPath, archivePath, progressCallback);
                    Directory.Delete(folderToArchive.FullPath, true);
                }
            }
                }
                else
                {
                    CopyDirectoryRecursively(folderToArchive.FullPath, archivePath, progressCallback);
                    Directory.Delete(folderToArchive.FullPath, true);
                }
            }
<<<<<<< HEAD
            }
=======
>>>>>>> 7955348 (Feature: Added a progress bar when moving files)
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            // Delete the tracked folder entry if the move fails
            Directory.Delete(archivePath, true);
            store.Remove(trackedFolder);
            await store.SaveAsync();
            progressCallback?.Invoke(100.0);
            return null;
        }
        progressCallback?.Invoke(100.0);
        return trackedFolder;
    }

    /// <inheritDoc />
    public async Task<bool> RestoreFolder(FolderItemModel folderToRestore, ITrackedFolderServices.CopyProgressCallback? progressCallback = null)
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

            // If the source and destination are on the same volume, use Move. Otherwise, copy and delete.
            var sourceRoot = Path.GetPathRoot(trackedFolder.ArchivePath);
            var destRoot = Path.GetPathRoot(trackedFolder.OriginalPath);
            if (string.Equals(sourceRoot, destRoot, StringComparison.OrdinalIgnoreCase))
            {
                Directory.Move(trackedFolder.ArchivePath, trackedFolder.OriginalPath);
                progressCallback?.Invoke(100.0);
            }
            else
            {
                CopyDirectoryRecursively(trackedFolder.ArchivePath, trackedFolder.OriginalPath, progressCallback);
                Directory.Delete(trackedFolder.ArchivePath, true);
            }
        }
        catch (Exception)
        {
            progressCallback?.Invoke(100.0);
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
    ///     A method to copy a directory recursively with progress reporting.
    /// </summary>
    /// <param name="sourceDir">The source directory path</param>
    /// <param name="destDir">The destination directory path</param>
    /// <param name="progressCallback">The progress callback to report progress percentage</param>
    private static void CopyDirectoryRecursively(string sourceDir, string destDir, ITrackedFolderServices.CopyProgressCallback? progressCallback = null)
    {
        var allFiles = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        var totalFiles = allFiles.Length;
        var copiedFiles = 0;

        void CopyRecursive(string src, string dst)
        {
            Directory.CreateDirectory(dst);
            foreach (var file in Directory.GetFiles(src))
            {
                var destFile = Path.Combine(dst, Path.GetFileName(file));
                File.Copy(file, destFile, true);
                copiedFiles++;
                progressCallback?.Invoke((double) copiedFiles / totalFiles * 100.0);
            }
            foreach (var dir in Directory.GetDirectories(src))
            {
                var destSubDir = Path.Combine(dst, Path.GetFileName(dir));
                CopyRecursive(dir, destSubDir);
            }
        }
        CopyRecursive(sourceDir, destDir);
    }
}