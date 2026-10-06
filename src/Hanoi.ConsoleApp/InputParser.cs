using System.Globalization;
using Hanoi.Core;

namespace Hanoi.ConsoleApp;

/// <summary>Interpreta el texto escrito por el usuario. No lanza excepciones ante entradas inesperadas.</summary>
public static class InputParser
{
    public const string TowerNames = "ABC";

    /// <summary>Opción de menú (1 a <paramref name="optionCount"/>), o null si no es válida.</summary>
    public static int? ParseMenuOption(string? input, int optionCount) =>
        TryParseInt(input, out var option) && option >= 1 && option <= optionCount ? option : null;

    /// <summary>Cantidad de discos entre <see cref="HanoiGame.MinDisks"/> y <see cref="HanoiGame.MaxDisks"/>, o null.</summary>
    public static int? ParseDiskCount(string? input) =>
        TryParseInt(input, out var count) && count is >= HanoiGame.MinDisks and <= HanoiGame.MaxDisks ? count : null;

    /// <summary>
    /// Índice de torre para una letra (A = 0, B = 1, ...). Cualquier letra se acepta aquí;
    /// la existencia de la torre la valida el juego. Devuelve null si no es una sola letra.
    /// </summary>
    public static int? ParseTower(string? input)
    {
        var text = input?.Trim();
        if (text is not { Length: 1 } || !char.IsAsciiLetter(text[0]))
        {
            return null;
        }

        return char.ToUpperInvariant(text[0]) - 'A';
    }

    /// <summary>El usuario quiere abandonar la partida (<c>0</c> o <c>Q</c>).</summary>
    public static bool IsQuit(string? input)
    {
        var text = input?.Trim();
        return text is "0" || string.Equals(text, "Q", StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryParseInt(string? input, out int value) =>
        int.TryParse(input?.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out value);
}
