using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class TrackedFolderEntity : BaseEntity
{
    [Required] public string OriginalPath { get; set; } = null!;

    [Required] public string ArchivePath { get; set; } = null!;
}