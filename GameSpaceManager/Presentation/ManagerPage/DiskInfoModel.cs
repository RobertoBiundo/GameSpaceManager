using System.ComponentModel.DataAnnotations;
using GameSpaceManager.Helpers;

namespace GameSpaceManager.Presentation.ManagerPage;

public class DiskInfoModel
{
    [Required]
    public string DisplayName { get; set; } = null!;

    public long TotalBytes { get; set; }
    public long FreeBytes { get; set; }

    public double UsedPercentage =>
        TotalBytes == 0 ? 0 : (double) (TotalBytes - FreeBytes) / TotalBytes * 100;

    public string FreeSpaceText =>
        $"{UnitHelpers.FormatDiskSize(FreeBytes)} free";

    public string TotalSpaceText =>
        $"{UnitHelpers.FormatDiskSize(TotalBytes)} total";
}