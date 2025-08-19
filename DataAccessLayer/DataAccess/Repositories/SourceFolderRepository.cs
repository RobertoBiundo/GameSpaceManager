using DataAccessLayer.DataAccess.Repositories.Interfaces;
using DataAccessLayer.Entities;

namespace DataAccessLayer.DataAccess.Repositories;

public class SourceFolderRepository(IDatabaseContext databaseContext) : ISourceFolderRepository
{
    public IQueryable<SourceFolderEntity> QueryMany()
    {
        return databaseContext.SourceFolders;
    }
}