using System.Text;
using Hanoi.Core;

namespace Hanoi.ConsoleApp;

/// <summary>Toda la entrada y salida de texto del programa.</summary>
public sealed class ConsoleView(TextReader input, TextWriter output)
{
    // Ancho de una columna: el disco más grande (8) mide 2 * 8 + 1 = 17 caracteres.
    private const int ColumnWidth = 2 * HanoiGame.MaxDisks + 3;

    /// <summary>Lee una línea; devuelve null si la entrada se cerró (fin de archivo).</summary>
    public string? Prompt(string message)
    {
        output.Write(message);
        return input.ReadLine();
    }

    public void ShowMessage(string message) => output.WriteLine(message);

    public void ShowError(string message) => output.WriteLine($"Error: {message}");

    public void ShowBlankLine() => output.WriteLine();

    public void ShowMainMenu()
    {
        output.WriteLine();
        output.WriteLine("=== TORRES DE HANÓI ===");
        output.WriteLine("1. Solución automática");
        output.WriteLine("2. Jugar paso a paso");
        output.WriteLine("3. Salir");
    }

    public void ShowAutoMove(int number, Move move) =>
        output.WriteLine(
            $"Movimiento {number}: disco {move.Disk} de {TowerName(move.From)} a {TowerName(move.To)}");

    public void ShowTowers(HanoiGame game)
    {
        var towers = Enumerable.Range(0, HanoiGame.TowerCount).Select(game.GetTower).ToArray();
        var text = new StringBuilder();

        for (var level = game.DiskCount - 1; level >= 0; level--)
        {
            foreach (var tower in towers)
            {
                text.Append(level < tower.Count ? DiskSprite(tower[level]) : DiskSprite(0));
            }

            text.AppendLine();
        }

        foreach (var tower in towers.Select((_, i) => i))
        {
            text.Append(CenterInColumn(TowerName(tower).ToString()));
        }

        output.WriteLine(text.ToString());
    }

    public static string DescribeError(MoveResult result) => result switch
    {
        MoveResult.InvalidTower => "la torre no existe. Usa A, B o C.",
        MoveResult.SameTower => "la torre de origen y la de destino son la misma.",
        MoveResult.EmptySource => "la torre de origen está vacía.",
        MoveResult.LargerOnSmaller => "no se puede poner un disco grande sobre uno más pequeño.",
        _ => "movimiento no válido.",
    };

    public static char TowerName(int tower) => InputParser.TowerNames[tower];

    private static string DiskSprite(int disk) =>
        CenterInColumn(disk == 0 ? "|" : new string('=', 2 * disk + 1));

    private static string CenterInColumn(string text)
    {
        var left = (ColumnWidth - text.Length) / 2;
        return new string(' ', left) + text + new string(' ', ColumnWidth - text.Length - left);
    }
}
