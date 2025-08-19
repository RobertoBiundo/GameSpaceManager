namespace GameSpaceManager.Presentation;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string? _name;

    public MainViewModel(IStringLocalizer localizer, IOptions<AppConfig> appInfo)
    {
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
    }

    private string? Title { get; }
}