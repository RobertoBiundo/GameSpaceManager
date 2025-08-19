using DataAccessLayer.DataAccess.Repositories.Interfaces;
using DataAccessLayer.Entities;

namespace DataAccessLayer.DataAccess.Repositories;

public class DestinationFolderRepository(IDatabaseContext databaseContext) : IDestinationFolderRepository
{
    public IQueryable<DestinationFolderEntity> QueryMany()
    {
        return databaseContext.DestinationFolders;
    }
}