namespace Hanoi.Core;

/// <summary>Solución recursiva clásica de las Torres de Hanói.</summary>
public static class HanoiSolver
{
    /// <summary>Movimientos que llevan todos los discos de la torre A a la torre C.</summary>
    public static IEnumerable<Move> Solve(int diskCount)
    {
        if (diskCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(diskCount), diskCount, "Debe haber al menos un disco.");
        }

        return SolveRecursive(diskCount, HanoiGame.InitialTower, HanoiGame.TargetTower, 1);
    }

    private static IEnumerable<Move> SolveRecursive(int disks, int from, int to, int via)
    {
        if (disks == 0)
        {
            yield break;
        }

        foreach (var move in SolveRecursive(disks - 1, from, via, to))
        {
            yield return move;
        }

        yield return new Move(disks, from, to);

        foreach (var move in SolveRecursive(disks - 1, via, to, from))
        {
            yield return move;
        }
    }
}
