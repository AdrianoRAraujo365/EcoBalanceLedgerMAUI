using System;
using System.Linq;

namespace EcoBalanceLedger.Services;

public class LeontiefResult
{
    public bool Sucesso { get; set; }
    public double[] ProducaoTotal { get; set; } = Array.Empty<double>();
    public double Determinante { get; set; }
    public double[][] Inversa { get; set; } = Array.Empty<double[]>();
    public string Mensagem { get; set; } = string.Empty;
}

public static class LeontiefService
{
    public static double[][] Identity(int n)
    {
        var id = new double[n][];
        for (int i = 0; i < n; i++)
        {
            id[i] = new double[n];
            id[i][i] = 1.0;
        }
        return id;
    }

    public static double[][] Subtract(double[][] a, double[][] b)
    {
        int n = a.Length;
        var r = new double[n][];
        for (int i = 0; i < n; i++)
        {
            r[i] = new double[n];
            for (int j = 0; j < n; j++)
                r[i][j] = a[i][j] - b[i][j];
        }
        return r;
    }

    public static double[] Multiply(double[][] m, double[] v)
    {
        int n = m.Length;
        var r = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < n; j++) sum += m[i][j] * v[j];
            r[i] = sum;
        }
        return r;
    }

    public static double Determinant(double[][] matrix)
    {
        int n = matrix.Length;
        var a = matrix.Select(r => (double[])r.Clone()).ToArray();
        double det = 1.0;

        for (int col = 0; col < n; col++)
        {
            int maxRow = col;
            for (int row = col + 1; row < n; row++)
                if (Math.Abs(a[row][col]) > Math.Abs(a[maxRow][col]))
                    maxRow = row;

            if (maxRow != col)
            {
                (a[col], a[maxRow]) = (a[maxRow], a[col]);
                det *= -1.0;
            }

            det *= a[col][col];
            if (Math.Abs(a[col][col]) < 1e-12) return 0.0;

            for (int row = col + 1; row < n; row++)
            {
                double factor = a[row][col] / a[col][col];
                for (int j = col; j < n; j++)
                    a[row][j] -= factor * a[col][j];
            }
        }
        return det;
    }

    public static double[][] Invert(double[][] matrix)
    {
        int n = matrix.Length;
        var a = new double[n][];
        for (int i = 0; i < n; i++)
        {
            a[i] = new double[2 * n];
            Array.Copy(matrix[i], 0, a[i], 0, n);
            a[i][n + i] = 1.0;
        }

        for (int col = 0; col < n; col++)
        {
            int maxRow = col;
            for (int row = col + 1; row < n; row++)
                if (Math.Abs(a[row][col]) > Math.Abs(a[maxRow][col]))
                    maxRow = row;

            (a[col], a[maxRow]) = (a[maxRow], a[col]);

            double pivot = a[col][col];
            if (Math.Abs(pivot) < 1e-12)
                throw new InvalidOperationException("Matriz singular ou quase-singular.");

            for (int j = 0; j < 2 * n; j++) a[col][j] /= pivot;

            for (int row = 0; row < n; row++)
            {
                if (row == col) continue;
                double factor = a[row][col];
                for (int j = 0; j < 2 * n; j++)
                    a[row][j] -= factor * a[col][j];
            }
        }

        var inv = new double[n][];
        for (int i = 0; i < n; i++)
        {
            inv[i] = new double[n];
            Array.Copy(a[i], n, inv[i], 0, n);
        }
        return inv;
    }

    public static LeontiefResult CalcularLeontief(double[][] C, double[] d)
    {
        int n = C.Length;
        var I = Identity(n);
        var L = Subtract(I, C);
        double det = Determinant(L);

        if (det <= 0)
        {
            return new LeontiefResult
            {
                Sucesso = false,
                Determinante = Math.Round(det, 6),
                Mensagem = "Sistema Improdutivo: Det(I-C) <= 0 (Violação de Hawkins-Simons)"
            };
        }

        var inv = Invert(L);
        var x = Multiply(inv, d);

        return new LeontiefResult
        {
            Sucesso = true,
            ProducaoTotal = x.Select(v => Math.Round(v, 4)).ToArray(),
            Determinante = Math.Round(det, 6),
            Inversa = inv,
            Mensagem = "Hawkins-Simons satisfeito. Solução viável encontrada."
        };
    }
}
