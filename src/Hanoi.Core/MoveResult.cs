namespace Hanoi.Core;

/// <summary>Resultado de intentar mover un disco.</summary>
public enum MoveResult
{
    Success,
    InvalidTower,
    SameTower,
    EmptySource,
    LargerOnSmaller,
}
