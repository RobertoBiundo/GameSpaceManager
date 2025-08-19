using DataAccessLayer.DataAccess.Repositories.Interfaces;
using DataAccessLayer.Entities;

namespace DataAccessLayer.DataAccess.Repositories;

public class TrackedFolderRepository(IDatabaseContext context) : ITrackedFolderRepository
{
    public IQueryable<TrackedFolderEntity> QueryMany()
    {
        return context.TrackedFolders;
    }
}