using Windows.Graphics;
using DataAccessLayer;
using DataAccessLayer.DataAccess.Stores;
using DataAccessLayer.DataAccess.Stores.Interfaces;
using GameSpaceManager.Services;
using GameSpaceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameSpaceManager;

public partial class App : Application
{
    /// <summary>
    ///     Initializes the singleton application object. This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection();

        // Register the database
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GameSpaceManager.db");

        // Register database context as singleton
        services.AddDbContext<DatabaseContext>(dbContextOptionsBuilder =>
            dbContextOptionsBuilder.UseSqlite($"Data Source={dbPath}",
                sqliteDbContextOptionsBuilder =>
                    sqliteDbContextOptionsBuilder.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name)));

        services.AddScoped<IDatabaseContext, DatabaseContext>();
        services.AddSingleton<IStore, Store>();

        // Migrate the database
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();
        }

        // Register your services
        services.AddSingleton<ISourceFolderServices, SourceFolderServices>();
        services.AddSingleton<IDestinationFolderService, DestinationFolderService>();
        services.AddSingleton<ITrackedFolderServices, TrackedFolderServices>();

        Container = services.BuildServiceProvider();
    }

    public static IServiceProvider Container { get; private set; } = null!;

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging((context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);
                }, true)
                .UseSerilog(true, true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                .UseHttp((context, services) =>
                {
#if DEBUG
                    // DelegatingHandler will be automatically injected
                    services.AddTransient<DelegatingHandler, DebugHttpHandler>();
#endif
                })
                .UseNavigation(RegisterRoutes)
            );
        MainWindow = builder.Window;
        MainWindow.AppWindow.Resize(new SizeInt32 { Width = 1280, Height = 1024 });

#if DEBUG
        MainWindow.UseStudio();
#endif
#if WINDOWS
        MainWindow.SetWindowIcon();
#endif
        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<MainPage, MainViewModel>()
        );

        routes.Register(
            new RouteMap("", views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new RouteMap("Main", views.FindByViewModel<MainViewModel>(), true)
                ]
            )
        );
    }
}