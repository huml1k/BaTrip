using Avalonia;
using System;

namespace BaTrip.Client;

sealed class Program
{
    public static string AuthServerAddress { get; private set; } = "https://localhost:7170";

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        AuthServerAddress = ResolveServerAddress(args);

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    private static string ResolveServerAddress(string[] args)
    {
        const string argumentPrefix = "--server-url=";

        foreach (var arg in args)
        {
            if (arg.StartsWith(argumentPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var value = arg[argumentPrefix.Length..].Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
        }

        return AuthServerAddress;
    }
}
