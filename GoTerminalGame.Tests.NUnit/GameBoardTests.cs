using GoTerminalGame.Core;

namespace GoTerminalGame.Tests.NUnit;

[TestFixture]
public class GameBoardTests
{
    [Test]
    public void CreateNewGameOn19x19Board()
    {
        Game game= new Game(19);

        Assert.That(game.BoardSize, Is.EqualTo(19));
        Assert.That(game.Board.EmptyBoardLayout[18,18] is not null);
        Assert.That(game.Board.PopulatedBoard[18,18] is not null);
        Assert.That(game.Board.EmptyBoardLayout[9,3], Is.EqualTo("┿"));
    }

    [Test]
    public void CreateNewGameOn13x13Board()
    {
        Game game = new Game(13);

        Assert.That(game.BoardSize, Is.EqualTo(13));
    }

    [Test]
    public void CreateNewGameOn9x9Board()
    {
        Game game = new Game(9);

        Assert.That(game.BoardSize, Is.EqualTo(9));
    }
}