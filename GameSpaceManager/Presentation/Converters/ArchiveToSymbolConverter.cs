using Microsoft.UI.Xaml.Data;

namespace GameSpaceManager.Presentation.Converters;

/// <summary>
///     A converter that transforms an archive path into a symbol for UI representation
/// </summary>
public class ArchiveToSymbolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var isArchived = !string.IsNullOrEmpty(value as string);
        return isArchived ? Symbol.Undo : Symbol.SaveLocal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}