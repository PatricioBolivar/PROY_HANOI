namespace Hanoi.Core.Tests;

public class HanoiGameTests
{
    [Fact]
    public void NewGame_PlacesAllDisksOnTowerA_LargestAtBottom()
    {
        var game = new HanoiGame(4);

        Assert.Equal(new[] { 4, 3, 2, 1 }, game.GetTower(0));
        Assert.Empty(game.GetTower(1));
        Assert.Empty(game.GetTower(2));
        Assert.Equal(0, game.MoveCount);
        Assert.False(game.IsWon);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(9)]
    [InlineData(0)]
    [InlineData(-1)]
    public void NewGame_WithDiskCountOutOfRange_Throws(int diskCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new HanoiGame(diskCount));
    }

    [Fact]
    public void TryMove_ValidMove_MovesDiskAndCountsIt()
    {
        var game = new HanoiGame(3);

        var result = game.TryMove(0, 1);

        Assert.Equal(MoveResult.Success, result);
        Assert.Equal(new[] { 3, 2 }, game.GetTower(0));
        Assert.Equal(new[] { 1 }, game.GetTower(1));
        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_FromEmptyTower_IsRejectedWithoutChangingState()
    {
        var game = new HanoiGame(3);

        var result = game.TryMove(1, 2);

        Assert.Equal(MoveResult.EmptySource, result);
        AssertInitialState(game);
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(0, 3)]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void TryMove_WithNonexistentTower_IsRejectedWithoutChangingState(int from, int to)
    {
        var game = new HanoiGame(3);

        var result = game.TryMove(from, to);

        Assert.Equal(MoveResult.InvalidTower, result);
        AssertInitialState(game);
    }

    [Fact]
    public void TryMove_ToSameTower_IsRejectedWithoutChangingState()
    {
        var game = new HanoiGame(3);

        var result = game.TryMove(0, 0);

        Assert.Equal(MoveResult.SameTower, result);
        AssertInitialState(game);
    }

    [Fact]
    public void TryMove_LargerDiskOnSmaller_IsRejectedWithoutChangingState()
    {
        var game = new HanoiGame(3);
        game.TryMove(0, 1); // disco 1 a B

        var result = game.TryMove(0, 1); // disco 2 sobre disco 1

        Assert.Equal(MoveResult.LargerOnSmaller, result);
        Assert.Equal(new[] { 3, 2 }, game.GetTower(0));
        Assert.Equal(new[] { 1 }, game.GetTower(1));
        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_SmallerDiskOnLarger_IsAllowed()
    {
        var game = new HanoiGame(3);
        game.TryMove(0, 1); // disco 1 a B
        game.TryMove(0, 2); // disco 2 a C

        var result = game.TryMove(1, 2); // disco 1 sobre disco 2

        Assert.Equal(MoveResult.Success, result);
        Assert.Equal(new[] { 2, 1 }, game.GetTower(2));
    }

    [Fact]
    public void IsWon_IsFalseUntilAllDisksAreOnTowerC()
    {
        var game = new HanoiGame(3);
        var moves = HanoiSolver.Solve(3).ToList();

        foreach (var move in moves.Take(moves.Count - 1))
        {
            game.TryMove(move.From, move.To);
            Assert.False(game.IsWon);
        }

        var last = moves[^1];
        game.TryMove(last.From, last.To);

        Assert.True(game.IsWon);
        Assert.Equal(new[] { 3, 2, 1 }, game.GetTower(2));
    }

    [Fact]
    public void IsWon_IsFalseWhenAllDisksAreOnTowerB()
    {
        var game = new HanoiGame(3);
        foreach (var move in HanoiSolver.Solve(3))
        {
            // Misma solución pero con destino B: se juega moviendo C <-> B.
            var to = move.To == 2 ? 1 : move.To == 1 ? 2 : move.To;
            var from = move.From == 2 ? 1 : move.From == 1 ? 2 : move.From;
            game.TryMove(from, to);
        }

        Assert.Equal(3, game.GetTower(1).Count);
        Assert.False(game.IsWon);
    }

    [Theory]
    [InlineData(3, 7)]
    [InlineData(4, 15)]
    [InlineData(8, 255)]
    public void MinimumMoves_IsTwoToTheNMinusOne(int diskCount, int expected)
    {
        Assert.Equal(expected, new HanoiGame(diskCount).MinimumMoves);
    }

    [Fact]
    public void GetTower_WithNonexistentTower_Throws()
    {
        var game = new HanoiGame(3);

        Assert.Throws<ArgumentOutOfRangeException>(() => game.GetTower(3));
    }

    private static void AssertInitialState(HanoiGame game)
    {
        Assert.Equal(new[] { 3, 2, 1 }, game.GetTower(0));
        Assert.Empty(game.GetTower(1));
        Assert.Empty(game.GetTower(2));
        Assert.Equal(0, game.MoveCount);
    }
}
