using DataAccessLayer.DataAccess.Stores.Interfaces;
using DataAccessLayer.Entities;
using GameSpaceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameSpaceManager.Services;

public class DestinationFolderService(IStore store) : IDestinationFolderService
{
    /// <inheritdoc />
    public async Task<DestinationFolderEntity?> SetDestinationFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        // If there is already a destination fodler in the databse, we remove it
        var existingDestinationFolder = await store.DestinationFolderRepository.QueryMany().FirstOrDefaultAsync();
        if (existingDestinationFolder != null)
            store.Remove(existingDestinationFolder);

        var destinationFolder = new DestinationFolderEntity { Path = path };
        await store.AddAsync(destinationFolder);
        await store.SaveAsync();

        return destinationFolder;
    }

    /// <inheritdoc />
    public DestinationFolderEntity? GetDestinationFolder()
    {
        return store.DestinationFolderRepository.QueryMany().FirstOrDefault();
    }
}