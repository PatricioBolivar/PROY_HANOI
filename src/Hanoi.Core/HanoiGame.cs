namespace Hanoi.Core;

/// <summary>
/// Estado y reglas de las Torres de Hanói. Las torres se identifican con índices
/// 0 (A), 1 (B) y 2 (C); los discos se numeran de 1 (el menor) a N (el mayor).
/// </summary>
public sealed class HanoiGame
{
    public const int TowerCount = 3;
    public const int MinDisks = 3;
    public const int MaxDisks = 8;
    public const int InitialTower = 0;
    public const int TargetTower = TowerCount - 1;

    private readonly Stack<int>[] _towers;

    public HanoiGame(int diskCount)
    {
        if (diskCount is < MinDisks or > MaxDisks)
        {
            throw new ArgumentOutOfRangeException(
                nameof(diskCount), diskCount, $"El número de discos debe estar entre {MinDisks} y {MaxDisks}.");
        }

        DiskCount = diskCount;
        _towers = new Stack<int>[TowerCount];
        for (var i = 0; i < TowerCount; i++)
        {
            _towers[i] = new Stack<int>();
        }

        for (var disk = diskCount; disk >= 1; disk--)
        {
            _towers[InitialTower].Push(disk);
        }
    }

    public int DiskCount { get; }

    public int MoveCount { get; private set; }

    /// <summary>Mínimo de movimientos para resolver el juego: 2^N − 1.</summary>
    public int MinimumMoves => MinimumMovesFor(DiskCount);

    /// <summary>Todos los discos están en la torre final (el orden lo garantizan las reglas).</summary>
    public bool IsWon => _towers[TargetTower].Count == DiskCount;

    public static int MinimumMovesFor(int diskCount) => (1 << diskCount) - 1;

    /// <summary>Discos de una torre, de abajo hacia arriba.</summary>
    public IReadOnlyList<int> GetTower(int tower)
    {
        if (!IsValidTower(tower))
        {
            throw new ArgumentOutOfRangeException(nameof(tower), tower, "Torre inexistente.");
        }

        return _towers[tower].Reverse().ToArray();
    }

    /// <summary>Valida y aplica un movimiento. Si no es válido, el estado no cambia.</summary>
    public MoveResult TryMove(int from, int to)
    {
        if (!IsValidTower(from) || !IsValidTower(to))
        {
            return MoveResult.InvalidTower;
        }

        if (from == to)
        {
            return MoveResult.SameTower;
        }

        if (_towers[from].Count == 0)
        {
            return MoveResult.EmptySource;
        }

        if (_towers[to].Count > 0 && _towers[to].Peek() < _towers[from].Peek())
        {
            return MoveResult.LargerOnSmaller;
        }

        _towers[to].Push(_towers[from].Pop());
        MoveCount++;
        return MoveResult.Success;
    }

    private static bool IsValidTower(int tower) => tower is >= 0 and < TowerCount;
}
