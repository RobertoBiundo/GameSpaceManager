using DataAccessLayer.DataAccess.Repositories;
using DataAccessLayer.DataAccess.Repositories.Interfaces;
using DataAccessLayer.DataAccess.Stores.Interfaces;
using DataAccessLayer.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccessLayer.DataAccess.Stores;

public class Store : IStore
{
    private readonly IDatabaseContext _context;

    public Store(IDatabaseContext context)
    {
        _context = context;
    }

    #region ----------------------------------------- Repositories -----------------------------------

    public ISourceFolderRepository SourceFolderRepository => new SourceFolderRepository(_context);
    public IDestinationFolderRepository DestinationFolderRepository => new DestinationFolderRepository(_context);
    public ITrackedFolderRepository TrackedFolderRepository => new TrackedFolderRepository(_context);

    #endregion

    #region ----------------------------------------- Shared -----------------------------------------

    /// <inheritdoc />
    public async Task<IEntity> AddAsync(IEntity entity)
    {
        var result = await _context.AddAsync(entity);

        return result.Entity;
    }

    /// <inheritdoc />
    public EntityEntry<IEntity> Remove(IEntity entity)
    {
        return _context.Remove(entity);
    }

    /// <inheritdoc />
    public async Task<bool> SaveAsync()
    {
        var succeeded = await _context.SaveChangesAsync() > 0;

        return succeeded;
    }

    #endregion
}