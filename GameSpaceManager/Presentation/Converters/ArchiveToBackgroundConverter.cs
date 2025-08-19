using Microsoft.UI.Xaml.Data;

namespace GameSpaceManager.Presentation.Converters;

public class ArchiveToBackgroundConverter : IValueConverter
{
    public Brush ActiveBrush { get; set; } = null!;
    public Brush ArchivedBrush { get; set; } = null!;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var archivePath = value as string;
        return string.IsNullOrEmpty(archivePath)
            ? ActiveBrush
            : ArchivedBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}