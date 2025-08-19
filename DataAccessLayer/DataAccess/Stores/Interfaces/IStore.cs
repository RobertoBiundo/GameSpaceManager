using DataAccessLayer.DataAccess.Repositories.Interfaces;
using DataAccessLayer.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccessLayer.DataAccess.Stores.Interfaces;

public interface IStore
{
    ISourceFolderRepository SourceFolderRepository { get; }
    IDestinationFolderRepository DestinationFolderRepository { get; }
    ITrackedFolderRepository TrackedFolderRepository { get; }

    #region ----------------------------------------- Shared -----------------------------------------

    /// <summary>
    ///     Add an entity to the database
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The added entity including auto generated fields</returns>
    Task<IEntity> AddAsync(IEntity entity);

    /// <summary>
    ///     Remove an entity from the database
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    /// <returns>The entity entry of the removed entity</returns>
    EntityEntry<IEntity> Remove(IEntity entity);

    /// <summary>
    ///     Save the changes to the database
    /// </summary>
    /// <returns>True if the changes were saved successfully</returns>
    Task<bool> SaveAsync();

    #endregion
}