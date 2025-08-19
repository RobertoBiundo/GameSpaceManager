namespace GameSpaceManager.Helpers;

public static class UnitHelpers
{
    /// <summary>
    ///     Formats the given bytes into a human-readable disk size string
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string FormatDiskSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        var order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}