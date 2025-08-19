using DataAccessLayer.Entities;

namespace GameSpaceManager.Services.Interfaces;

public interface IDestinationFolderService
{
    /// <summary>
    ///     Sets the destination folder
    /// </summary>
    /// <param name="path">The path of the destination folder to set</param>
    /// <returns>The new destination folder entity if set successfully, otherwise null</returns>
    Task<DestinationFolderEntity?> SetDestinationFolder(string path);

    /// <summary>
    ///     Gets the destination folder
    /// </summary>
    /// <returns>The destination folder entity if it exists, otherwise null</returns>
    DestinationFolderEntity? GetDestinationFolder();
}