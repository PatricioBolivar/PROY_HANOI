using Hanoi.Core;

namespace Hanoi.ConsoleApp;

public sealed class AutoSolveMode(ConsoleView view)
{
    public void Run()
    {
        if (DiskCountPrompt.Ask(view) is not { } diskCount)
        {
            return;
        }

        var game = new HanoiGame(diskCount);
        view.ShowBlankLine();
        view.ShowMessage("Estado inicial:");
        view.ShowTowers(game);

        foreach (var move in HanoiSolver.Solve(diskCount))
        {
            game.TryMove(move.From, move.To);
            view.ShowBlankLine();
            view.ShowAutoMove(game.MoveCount, move);
            view.ShowTowers(game);
        }

        view.ShowBlankLine();
        view.ShowMessage($"Solución completada en {game.MoveCount} movimientos (2^{diskCount} - 1 = {game.MinimumMoves}).");
    }
}
