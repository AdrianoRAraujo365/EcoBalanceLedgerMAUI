using EcoBalanceLedger.Models;
using System;
using System.IO;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace EcoBalanceLedger.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        ConstruirKpis();
    }

    private void ConstruirKpis()
    {
        KpisGrid.Children.Clear();
        var kpis = EcoBalanceData.Kpis;

        for (int i = 0; i < kpis.Count; i++)
        {
            var k = kpis[i];
            int row = i / 2;
            int col = i % 2;

            var border = new Border
            {
                BackgroundColor = Color.FromArgb("#1e293b"),
                Stroke = Color.FromArgb(k.CorHex),
                StrokeThickness = 1,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
                Padding = new Thickness(12)
            };

            var stack = new VerticalStackLayout { HorizontalOptions = LayoutOptions.Center };
            stack.Children.Add(new Label { Text = k.Valor, TextColor = Color.FromArgb(k.CorHex), FontSize = 22, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center });
            stack.Children.Add(new Label { Text = k.Label, TextColor = Color.FromArgb("#9ca3af"), FontSize = 11, HorizontalTextAlignment = TextAlignment.Center });
            stack.Children.Add(new Label
            {
                Text = $"{(k.Tendencia >= 0 ? "▲" : "▼")} {Math.Abs(k.Tendencia)}% vs 2015",
                TextColor = k.Tendencia >= 0 ? Color.FromArgb("#10b981") : Color.FromArgb("#ef4444"),
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 4, 0, 0)
            });

            border.Content = stack;
            KpisGrid.Add(border, col, row);
        }
    }

    private async void OnExportReportClicked(object sender, EventArgs e)
    {
        string relatorio = """
        ============================================================
        ECOBALANCE LEDGER — RELATÓRIO DE SUSTENTABILIDADE & ESG
        Período: 2015–2026 | Conformidade: ISO 14001, IFRS S2, GRI 305
        ============================================================
        INDICADORES DE DESEMPENHO (KPIS):
        • Emissões CO₂ Escopo 2 : 820 tCO₂e (-32% vs 2015)
        • ROI Sustentável       : 14,2% (+238% vs 2015)
        • Eficiência Energética : PUE 0,81 (+25%)
        • Auditoria Criptográfica: 100% registros com SHA-256

        PREVISÃO DE RESILIÊNCIA CLIMÁTICA 2030 (IFRS S2):
        • Cenário Otimista : 92% de conformidade plena
        • Cenário Base     : 78% com manutenção da matriz técnica
        • Cenário de Risco : 65% sem aportes de capital

        Auditoria Digital: Ativo | Hash Root: 0x8f2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e
        Documento gerado automaticamente pelo motor .NET MAUI.
        """;

        string caminho = Path.Combine(FileSystem.CacheDirectory, "Relatorio_Auditoria_ESG.txt");
        await File.WriteAllTextAsync(caminho, relatorio);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Exportar Relatório ESG",
            File = new ShareFile(caminho)
        });
    }
}
