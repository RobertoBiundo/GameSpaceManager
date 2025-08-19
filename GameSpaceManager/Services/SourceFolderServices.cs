using DataAccessLayer.DataAccess.Stores.Interfaces;
using DataAccessLayer.Entities;
using GameSpaceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameSpaceManager.Services;

public class SourceFolderServices(IStore store) : ISourceFolderServices
{
    /// <inheritdoc />
    public async Task<SourceFolderEntity?> AddSourceFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var sourceFolder = new SourceFolderEntity { Path = path };
        await store.AddAsync(sourceFolder);
        await store.SaveAsync();

        return sourceFolder;
    }

    /// <inheritdoc />
    public IQueryable<SourceFolderEntity> GetSourceFolders()
    {
        return store.SourceFolderRepository.QueryMany();
    }

    /// <inheritdoc />
    public async Task DeleteSourceFolder(Guid id)
    {
        var sourceFolder = await store.SourceFolderRepository.QueryMany().Where(w => w.Id == id).FirstOrDefaultAsync();
        if (sourceFolder != null)
        {
            store.Remove(sourceFolder);
            await store.SaveAsync();
        }
    }
}