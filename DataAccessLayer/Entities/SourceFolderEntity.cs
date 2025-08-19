using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class SourceFolderEntity : BaseEntity
{
    private const int PathMaximumLength = 4096;

    /// <summary>
    ///     The path of the folder
    /// </summary>
    [Required]
    [MaxLength(PathMaximumLength)]
    public string Path { get; set; } = null!;
}