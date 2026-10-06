namespace Hanoi.Core;

/// <summary>Movimiento de un disco entre dos torres (índices 0 a 2).</summary>
public readonly record struct Move(int Disk, int From, int To);
