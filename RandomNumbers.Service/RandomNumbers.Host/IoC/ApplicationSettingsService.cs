namespace RandomNumbers.Utilities;

public static class ApplicationSettingsService
{
    public static IServiceCollection RegisterApplicationSettings(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        var appSettings = new ApplicationSettings();
        configuration.Bind(appSettings);
        services.AddSingleton(appSettings);

        return services;
    }
}
