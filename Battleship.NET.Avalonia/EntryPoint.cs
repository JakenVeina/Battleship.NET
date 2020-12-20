using Avalonia;

namespace Battleship.NET.Avalonia
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
            => BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);

        // Required by visual designer
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<Application>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
