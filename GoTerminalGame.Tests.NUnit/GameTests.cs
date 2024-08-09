using GoTerminalGame.Core;

namespace GoTerminalGame.Tests.NUnit;

[TestFixture]
public class GameTests
{
    [Test]
    public void CreateNewGameOn19x19Board()
    {
        Game game = new Game(19);

        Assert.That(game.BoardSize, Is.EqualTo(19));
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