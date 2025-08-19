using DataAccessLayer.Entities;

namespace GameSpaceManager.Services.Interfaces;

public interface ISourceFolderServices
{
    /// <summary>
    ///     Adds a new source folder to the database
    ///     If the path is invalid no source folder is added and null is returned
    /// </summary>
    /// <param name="path">The path of the source folder to add</param>
    /// <returns>A source folder entity if added successfully, otherwise null</returns>
    Task<SourceFolderEntity?> AddSourceFolder(string path);

    /// <summary>
    ///     Gets a list of all the source folders
    /// </summary>
    /// <returns>A queryable of all source folders</returns>
    IQueryable<SourceFolderEntity> GetSourceFolders();

    /// <summary>
    ///     Deletes a source folder by its id
    /// </summary>
    /// <param name="id">The id of the source folder to delete</param>
    Task DeleteSourceFolder(Guid id);
}