namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Extensions.Configuration;

/// <summary>
///     Helper class containing function and values that are repeated through the application.
///     Singleton Pattern.
/// </summary>
public sealed class Utilities
{
    //TODO: Investigate singleton pattern in .net core
    private static Utilities? Instance = null;
    private string? dbConectionString;

    /// <summary>
    ///     Function for settng the single instance
    /// </summary>
    public static Utilities GetInstance()
    {
        if (Instance == null)
        {
            Instance = new Utilities();
        }
        return Instance;
    }

    /// <summary>
    ///     Map all app config values to vars
    /// </summary>
    public Utilities()
    {
        // Build a config object, using env vars and JSON providers.
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Get values from the config given their key and their target type.
        IConfigurationSection settings = config.GetRequiredSection("Settings");
        dbConectionString = settings["dbConnectionString"];
    }

    /// <summary>
    ///     Get methods for dbConnectionString
    /// </summary>
    public string? DbConectionString
    {
        get { return dbConectionString; }
    }
}

