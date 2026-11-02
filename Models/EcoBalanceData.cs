using System.Collections.Generic;

namespace EcoBalanceLedger.Models;

public class SectorModel
{
    public string Nome { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public double Demanda { get; set; }
    public double[] Valores { get; set; } = Array.Empty<double>();
    public int Eficiencia { get; set; } = 75;
}

public class TransactionRecord
{
    public string Tipo { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public double Valor { get; set; }
    public string Unidade { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string Norma { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
}

public class KpiModel
{
    public string Valor { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string CorHex { get; set; } = "#10b981";
    public int Tendencia { get; set; }
}

public class EvolucaoDecenalModel
{
    public string Ano { get; set; } = string.Empty;
    public double Pue { get; set; }
    public int Co2 { get; set; }
    public double Roi { get; set; }
    public string Conformidade { get; set; } = string.Empty;
    public string Blockchain { get; set; } = string.Empty;
}

public static class EcoBalanceData
{
    public static readonly string[] MatrixCols = { "Hardware", "Cloud", "Auto", "Papel", "TI Verde" };

    public static List<SectorModel> GetDefaultMatrix() => new()
    {
        new SectorModel { Nome = "Hardware", Icon = "hardware_chip.png", Demanda = 2.0, Valores = new double[] { 0.15, 0.10, 0.20, 0.05, 0.10 }, Eficiencia = 78 },
        new SectorModel { Nome = "Cloud/Data", Icon = "cloud.png", Demanda = 1.5, Valores = new double[] { 0.10, 0.40, 0.15, 0.08, 0.12 }, Eficiencia = 82 },
        new SectorModel { Nome = "Automação", Icon = "cog.png", Demanda = 0.8, Valores = new double[] { 0.05, 0.15, 0.20, 0.03, 0.15 }, Eficiencia = 88 },
        new SectorModel { Nome = "Papel/Celulose", Icon = "leaf.png", Demanda = 1.2, Valores = new double[] { 0.02, 0.05, 0.03, 0.30, 0.08 }, Eficiencia = 75 },
        new SectorModel { Nome = "TI Verde", Icon = "flower.png", Demanda = 0.5, Valores = new double[] { 0.08, 0.12, 0.10, 0.04, 0.05 }, Eficiencia = 95 }
    };

    public static List<EvolucaoDecenalModel> EvolucaoDecenal => new()
    {
        new EvolucaoDecenalModel { Ano = "2015", Pue = 0.65, Co2 = 1200, Roi = 4.2, Conformidade = "Não Conforme", Blockchain = "Ausente" },
        new EvolucaoDecenalModel { Ano = "2018", Pue = 0.69, Co2 = 1120, Roi = 6.1, Conformidade = "Parcial", Blockchain = "Simulação Inicial" },
        new EvolucaoDecenalModel { Ano = "2021", Pue = 0.74, Co2 = 980, Roi = 9.8, Conformidade = "Parcial", Blockchain = "Verificado" },
        new EvolucaoDecenalModel { Ano = "2024", Pue = 0.79, Co2 = 860, Roi = 12.5, Conformidade = "Conforme", Blockchain = "Verificado" },
        new EvolucaoDecenalModel { Ano = "2025/26", Pue = 0.81, Co2 = 820, Roi = 14.2, Conformidade = "Conforme (Pleno)", Blockchain = "Ledger Ativo (SHA-256)" }
    };

    public static List<TransactionRecord> Transactions => new()
    {
        new TransactionRecord { Tipo = "Crédito de Carbono", Data = "2026-01-15 14:32", Valor = 150.5, Unidade = "tCO₂e", Hash = "0x8f2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e", Norma = "ISO 14064", Setor = "Cloud/Data" },
        new TransactionRecord { Tipo = "Resíduo Eletrônico", Data = "2026-01-15 12:18", Valor = -45.2, Unidade = "kg", Hash = "0x1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d", Norma = "GRI 306", Setor = "Hardware" },
        new TransactionRecord { Tipo = "Energia Renovável", Data = "2026-01-15 10:45", Valor = 1200.0, Unidade = "kWh", Hash = "0x9e8d7c6b5a4f3e2d1c0b9a8f7e6d5c4b", Norma = "ISO 50001", Setor = "TI Verde" },
        new TransactionRecord { Tipo = "Economia Circular", Data = "2026-01-14 16:22", Valor = 89.3, Unidade = "kg", Hash = "0x7f6e5d4c3b2a1f0e9d8c7b6a5f4e3d2c", Norma = "GRI 301", Setor = "Papel/Celulose" },
        new TransactionRecord { Tipo = "Automação Verde", Data = "2026-01-14 09:15", Valor = 234.7, Unidade = "kWh", Hash = "0x6a5b4c3d2e1f0a9b8c7d6e5f4a3b2c1d", Norma = "IFRS S2", Setor = "Automação" }
    };

    public static List<KpiModel> Kpis => new()
    {
        new KpiModel { Valor = "820 t", Label = "Emissões CO₂ Escopo 2", CorHex = "#10b981", Tendencia = -32 },
        new KpiModel { Valor = "14,2%", Label = "ROI Sustentável", CorHex = "#0ea5e9", Tendencia = 238 },
        new KpiModel { Valor = "0,81", Label = "Eficiência Energética (PUE)", CorHex = "#f59e0b", Tendencia = 25 },
        new KpiModel { Valor = "100%", Label = "Dados Auditáveis (SHA-256)", CorHex = "#8b5cf6", Tendencia = 100 }
    };
}
