using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace EcoBalanceLedger.Views;

public partial class SimulatorPage : ContentPage
{
    private readonly CultureInfo _ptBr = new("pt-BR");

    public SimulatorPage()
    {
        InitializeComponent();
        AtualizarSimulacao();
    }

    private void OnParamChanged(object sender, ValueChangedEventArgs e)
    {
        AtualizarSimulacao();
    }

    private void AtualizarSimulacao()
    {
        double hw = SldHardware.Value;
        double eng = SldEnergia.Value;
        double aut = SldAutomacao.Value;
        double invest = Math.Round(SldInvestimento.Value / 10000.0) * 10000.0;

        LblHwVal.Text = $"{hw:F0}%";
        LblEnergiaVal.Text = $"{eng:F0}%";
        LblAutoVal.Text = $"{aut:F0}%";
        LblInvestVal.Text = invest.ToString("C0", _ptBr);

        double eficienciaTotal = (hw * 0.3) + (eng * 0.4) + (aut * 0.3);
        double economiaAnual = invest * (eficienciaTotal / 100.0) * 0.5;
        double roi = Math.Max(0.0, (((economiaAnual * 3.0) - invest) / invest) * 100.0);
        int payback = Math.Max(6, (int)Math.Ceiling((invest / Math.Max(economiaAnual, 1.0)) * 12.0));

        LblRoi.Text = $"{roi:F1}%";
        LblPayback.Text = payback.ToString();
        LblDeltaX.Text = $"+{(eficienciaTotal * 0.8):F1}%";
        LblEconomia.Text = economiaAnual.ToString("C0", _ptBr);
        LblCo2Reducao.Text = $"-{(eficienciaTotal * 0.6):F1}%";
    }

    private async void OnExportClicked(object sender, EventArgs e)
    {
        string conteudo = $"""
        ============================================================
        ECOBALANCE LEDGER - PROPOSTA DE INVESTIMENTO EM TI VERDE
        Modelo de Leontief & Diretrizes IFRS S2 / GRI 305
        ============================================================
        Investimento Inicial : {LblInvestVal.Text}
        ROI Estimado (3 anos): {LblRoi.Text}
        Payback Estimado     : {LblPayback.Text} meses
        Economia Anual       : {LblEconomia.Text}
        Redução de CO₂       : {LblCo2Reducao.Text}
        Impacto Produtivo ΔX : {LblDeltaX.Text}
        ============================================================
        Gerado automaticamente via EcoBalance Ledger C# .NET MAUI
        """;

        string filePath = Path.Combine(FileSystem.CacheDirectory, "Proposta_Investimento_TI_Verde.txt");
        await File.WriteAllTextAsync(filePath, conteudo);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Compartilhar Proposta de Investimento",
            File = new ShareFile(filePath)
        });
    }
}
