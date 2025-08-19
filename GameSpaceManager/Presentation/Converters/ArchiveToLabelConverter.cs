using Microsoft.UI.Xaml.Data;

namespace GameSpaceManager.Presentation.Converters;

/// <summary>
///     A converter that transforms an archive path into a label for UI representation
/// </summary>
public class ArchiveToLabelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var isArchived = !string.IsNullOrEmpty(value as string);
        return isArchived ? "Restore" : "Archive";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}