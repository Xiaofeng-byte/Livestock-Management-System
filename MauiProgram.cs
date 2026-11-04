using COMP609A2.Pages;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace COMP609A2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.ConfigureSyncfusionCore();
            builder.Services.AddSingleton<DatabaseService>();//Dependency injection
            builder.Services.AddTransient<ChartPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
    public static class GlobalVariables
    {
        public static string? CurrentUser { get; set; }
        public static int UserID { get; set; } = 0;
        public static bool IsLoggedIn { get; set; } = false;
        public static bool IsAdmin { get; set; } = false;
    }
    //public class AdminToVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        return value is bool isAdmin && isAdmin;
    //    }

    //    public object ConvertBack(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
