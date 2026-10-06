namespace Hanoi.Core.Tests;

public class HanoiSolverTests
{
    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Solve_ProducesTwoToTheNMinusOneMoves(int diskCount)
    {
        var moves = HanoiSolver.Solve(diskCount).ToList();

        Assert.Equal((1 << diskCount) - 1, moves.Count);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(8)]
    public void Solve_EveryMoveIsValid_AndEndsWithGameWon(int diskCount)
    {
        var game = new HanoiGame(diskCount);

        foreach (var move in HanoiSolver.Solve(diskCount))
        {
            Assert.Equal(MoveResult.Success, game.TryMove(move.From, move.To));
        }

        Assert.True(game.IsWon);
        Assert.Equal(game.MinimumMoves, game.MoveCount);
    }

    [Fact]
    public void Solve_ReportsTheDiskThatActuallyMoves()
    {
        var game = new HanoiGame(3);

        foreach (var move in HanoiSolver.Solve(3))
        {
            var top = game.GetTower(move.From)[^1];
            Assert.Equal(top, move.Disk);
            game.TryMove(move.From, move.To);
        }
    }

    [Fact]
    public void Solve_ForThreeDisks_MatchesTheClassicSequence()
    {
        var expected = new (int Disk, int From, int To)[]
        {
            (1, 0, 2), (2, 0, 1), (1, 2, 1), (3, 0, 2), (1, 1, 0), (2, 1, 2), (1, 0, 2),
        };

        var actual = HanoiSolver.Solve(3).Select(m => (m.Disk, m.From, m.To));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Solve_WithLessThanOneDisk_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => HanoiSolver.Solve(0));
    }
}
