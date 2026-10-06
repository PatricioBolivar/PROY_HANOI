using Hanoi.Core;

namespace Hanoi.ConsoleApp;

public static class DiskCountPrompt
{
    /// <summary>Pide el número de discos hasta obtener un valor válido. Devuelve null si la entrada se cerró.</summary>
    public static int? Ask(ConsoleView view)
    {
        while (true)
        {
            var text = view.Prompt($"Número de discos ({HanoiGame.MinDisks}-{HanoiGame.MaxDisks}): ");
            if (text is null)
            {
                return null;
            }

            if (InputParser.ParseDiskCount(text) is { } count)
            {
                return count;
            }

            view.ShowError($"ingresa un número entero entre {HanoiGame.MinDisks} y {HanoiGame.MaxDisks}.");
        }
    }
}
