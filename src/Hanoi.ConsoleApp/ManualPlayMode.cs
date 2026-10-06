using Hanoi.Core;

namespace Hanoi.ConsoleApp;

public sealed class ManualPlayMode(ConsoleView view)
{
    private const string QuitHint = "(0 o Q para volver al menú)";

    public void Run()
    {
        if (DiskCountPrompt.Ask(view) is not { } diskCount)
        {
            return;
        }

        var game = new HanoiGame(diskCount);
        view.ShowBlankLine();
        view.ShowTowers(game);

        while (!game.IsWon)
        {
            if (!TryReadTower("Torre de origen (A/B/C) " + QuitHint + ": ", out var from) ||
                !TryReadTower("Torre de destino (A/B/C) " + QuitHint + ": ", out var to))
            {
                view.ShowMessage("Partida abandonada.");
                return;
            }

            var result = game.TryMove(from, to);
            if (result != MoveResult.Success)
            {
                view.ShowError(ConsoleView.DescribeError(result));
                continue;
            }

            view.ShowBlankLine();
            view.ShowTowers(game);
            view.ShowMessage($"Movimientos: {game.MoveCount}");
        }

        view.ShowBlankLine();
        view.ShowMessage(
            $"¡Ganaste! Resolviste el juego en {game.MoveCount} movimientos (mínimo posible: {game.MinimumMoves}).");
    }

    /// <summary>
    /// Pide una torre hasta recibir una letra. Devuelve false si el usuario abandona o la entrada se cierra.
    /// La existencia de la torre la valida el juego al mover.
    /// </summary>
    private bool TryReadTower(string prompt, out int tower)
    {
        tower = -1;
        while (true)
        {
            var text = view.Prompt(prompt);
            if (text is null || InputParser.IsQuit(text))
            {
                return false;
            }

            if (InputParser.ParseTower(text) is { } parsed)
            {
                tower = parsed;
                return true;
            }

            view.ShowError("ingresa una letra de torre (A, B o C).");
        }
    }
}
