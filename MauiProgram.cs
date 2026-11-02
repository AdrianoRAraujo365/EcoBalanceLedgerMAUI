using EcoBalanceLedger.Views;
using Microsoft.Extensions.Logging;

namespace EcoBalanceLedger;

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

        // Registro das páginas no Container de Injeção de Dependências
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<MatrixPage>();
        builder.Services.AddTransient<BlockchainPage>();
        builder.Services.AddTransient<SimulatorPage>();
        builder.Services.AddTransient<DashboardPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
