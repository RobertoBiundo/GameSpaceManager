using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDatabaseContext
{
    public DbSet<SourceFolderEntity> SourceFolders { get; set; } = null!;
    public DbSet<DestinationFolderEntity> DestinationFolders { get; set; } = null!;
    public DbSet<TrackedFolderEntity> TrackedFolders { get; set; } = null!;
}