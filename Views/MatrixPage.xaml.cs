using EcoBalanceLedger.Models;
using EcoBalanceLedger.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcoBalanceLedger.Views;

public partial class MatrixPage : ContentPage
{
    private List<SectorModel> _matrix = new();
    private readonly Entry[,] _coefficientEntries = new Entry[5, 5];
    private readonly Entry[] _demandEntries = new Entry[5];

    public MatrixPage()
    {
        InitializeComponent();
        CarregarMatriz();
    }

    private void CarregarMatriz()
    {
        _matrix = EcoBalanceData.GetDefaultMatrix();
        ConstruirTabelaVisual();
        Calcular();
    }

    private void ConstruirTabelaVisual()
    {
        MatrixContainer.Children.Clear();

        // Cabeçalho
        var headerGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(110) },
                new ColumnDefinition { Width = new GridLength(45) },
                new ColumnDefinition { Width = new GridLength(45) },
                new ColumnDefinition { Width = new GridLength(45) },
                new ColumnDefinition { Width = new GridLength(45) },
                new ColumnDefinition { Width = new GridLength(45) },
                new ColumnDefinition { Width = new GridLength(65) }
            }
        };

        headerGrid.Add(new Label { Text = "Setor", TextColor = Color.FromArgb("#9ca3af"), FontSize = 11, FontAttributes = FontAttributes.Bold }, 0, 0);
        for (int c = 0; c < EcoBalanceData.MatrixCols.Length; c++)
        {
            headerGrid.Add(new Label { Text = EcoBalanceData.MatrixCols[c].Substring(0, Math.Min(4, EcoBalanceData.MatrixCols[c].Length)), 
                                       TextColor = Color.FromArgb("#10b981"), FontSize = 10, HorizontalTextAlignment = TextAlignment.Center }, c + 1, 0);
        }
        headerGrid.Add(new Label { Text = "Dem (d)", TextColor = Color.FromArgb("#9ca3af"), FontSize = 10, HorizontalTextAlignment = TextAlignment.End }, 6, 0);
        MatrixContainer.Children.Add(headerGrid);

        // Linhas dos Setores
        for (int i = 0; i < _matrix.Count; i++)
        {
            var rowGrid = new Grid
            {
                ColumnDefinitions = headerGrid.ColumnDefinitions,
                Margin = new Thickness(0, 4)
            };

            rowGrid.Add(new Label { Text = _matrix[i].Nome, TextColor = Color.FromArgb("#f8fafc"), FontSize = 12, VerticalOptions = LayoutOptions.Center }, 0, 0);

            for (int j = 0; j < _matrix[i].Valores.Length; j++)
            {
                int rowIdx = i, colIdx = j;
                var entry = new Entry
                {
                    Text = _matrix[i].Valores[j].ToString("F2"),
                    Keyboard = Keyboard.Numeric,
                    FontSize = 11,
                    TextColor = Color.FromArgb("#10b981"),
                    BackgroundColor = Color.FromArgb("#0f172a"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    HeightRequest = 35
                };
                entry.TextChanged += (s, e) =>
                {
                    if (double.TryParse(entry.Text, out double val))
                    {
                        _matrix[rowIdx].Valores[colIdx] = Math.Clamp(val, 0.0, 0.99);
                        Calcular();
                    }
                };
                _coefficientEntries[i, j] = entry;
                rowGrid.Add(entry, j + 1, 0);
            }

            int currentI = i;
            var demandEntry = new Entry
            {
                Text = _matrix[i].Demanda.ToString("F1"),
                Keyboard = Keyboard.Numeric,
                FontSize = 11,
                TextColor = Color.FromArgb("#0ea5e9"),
                BackgroundColor = Color.FromArgb("#0f172a"),
                HorizontalTextAlignment = TextAlignment.End,
                HeightRequest = 35
            };
            demandEntry.TextChanged += (s, e) =>
            {
                if (double.TryParse(demandEntry.Text, out double val))
                {
                    _matrix[currentI].Demanda = Math.Max(0.0, val);
                    Calcular();
                }
            };
            _demandEntries[i] = demandEntry;
            rowGrid.Add(demandEntry, 6, 0);

            MatrixContainer.Children.Add(rowGrid);
        }
    }

    private void Calcular()
    {
        double[][] C = _matrix.Select(r => (double[])r.Valores.Clone()).ToArray();
        double[] d = _matrix.Select(r => r.Demanda).ToArray();

        var res = LeontiefService.CalcularLeontief(C, d);

        if (res.Sucesso)
        {
            double somaX = res.ProducaoTotal.Sum();
            LblProducaoTotal.Text = somaX.ToString("N2");
            LblFormulaInfo.Text = $"Fórmula: X = (I − C)⁻¹ · d | Det(I-C) = {res.Determinante:F4}";
            LblHawkinsStatus.Text = "✓ Teorema de Hawkins-Simons atendido (Viável)";
            LblHawkinsStatus.TextColor = Color.FromArgb("#10b981");
        }
        else
        {
            LblProducaoTotal.Text = "INDETERMINADO";
            LblFormulaInfo.Text = $"Det(I-C) = {res.Determinante:F4}";
            LblHawkinsStatus.Text = "⚠️ " + res.Mensagem;
            LblHawkinsStatus.TextColor = Color.FromArgb("#ef4444");
        }
    }

    private void OnResetClicked(object sender, EventArgs e) => CarregarMatriz();

    private void OnOptimizeClicked(object sender, EventArgs e)
    {
        for (int i = 0; i < _matrix.Count; i++)
        {
            for (int j = 0; j < _matrix[i].Valores.Length; j++)
            {
                _matrix[i].Valores[j] = Math.Round(_matrix[i].Valores[j] * 0.90, 2);
                _coefficientEntries[i, j].Text = _matrix[i].Valores[j].ToString("F2");
            }
        }
        Calcular();
        DisplayAlert("Otimização Concluída", "Coeficientes reduzidos em 10% para ganhos de ecoeficiência.", "OK");
    }
}
