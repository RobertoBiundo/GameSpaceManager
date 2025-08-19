using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccessLayer;

/// <summary>
///     The database context used to access the sqlite database
/// </summary>
public interface IDatabaseContext : IDisposable
{
    DbSet<SourceFolderEntity> SourceFolders { get; set; }
    DbSet<DestinationFolderEntity> DestinationFolders { get; set; }
    DbSet<TrackedFolderEntity> TrackedFolders { get; set; }

    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class;

    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}