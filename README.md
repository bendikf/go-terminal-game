# Go Terminal Game

Go Terminal Game is a terminal application which lets two players engage in a game of Go using hotseat multiplayer procedures (i.e. taking turns at the the keyboard. It's not that fancy).

The aim of the project was to learn .NET/C# (I was ready for a more challenging exercise after tackling [Tic Tac Toe](https://github.com/bendikf/tic-tac-toe)).

The test projects were written using [MSTest](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-mstest-intro), [NUnit](https://nunit.org/) and [xUnit](https://xunit.net/) respectively (to be committed...). The aim of each of the test projects was to familiarise myself with a different unit-testing framework.

## Introduction to Go

Go is a strategy game which originated in China more than 2,500 years ago. Today it is played all around the world and is especially prevalent in East Asia.

The game is known by many names, such as Baduk (Korea), Wéiqí (China) and Cờ vây (Vietnam). I have chosen to use the Japanese name of the game (Go), as it is the most commonly used name for the game worldwide.

Play takes place on a grid, typically with the dimensions 19×19, but sometimes on smaller boards such as 13×13 or 9×9. Players take alternating turns placing stones on the intersections with the goal of capturing territory.

For a detailed overview of the rules, see this page from [The British Go Association](https://www.britgo.org/intro/intro2.html) (rules may vary slightly from this implementation, see below). If you are interested in learning to play the game, see e.g. https://www.pandanet.co.jp/English/learning_go/learning_go_1.html.

## Using the application

The applicatio has not been packaged in any way. To run, download and install the latest .NET SDK and run the project with the command `dotnet run`.

The game can be played with three board configurations, 19×19, 13×13 or 9×9.

When in-game, the active player may place a stone by specifying in which row and in which column the stone should be placed. The application accepts a combination of a letter (case insensitive) representing the column (e.g. A-S for 19×19) and a number representing the row (e.g. "a13" or "2e"). 

Alternatively, a player can pass by typing "pass" (case insensitive). The game is over when both players have consecutively passed, and points are awarded.

The application is terminated either by typing "exit" or by pressing <kbd>Ctrl</kbd> + <kbd>C</kbd>.  

## Rules variations

Go is played according to several different rulesets in different parts of the world and depending on the level of play (e.g. tournament rules). Below follows a runthrough of some rules which typically vary between rulesets and how they are implemented in this game:

* The game uses area scoring. There are currently no plans to implement other scoring methods, such as e.g. territory scoring.
* The game uses a komi value of 7 for all board sizes. This value is added to the second (white) player's score to compensate for first-player advantage.
* Draws result in victory for the white player.
* Any move which would result in self-capture is disallowed.
* Any move which would result in the repeat of a previous game state (incl. active player) is disallowed.

---

[GPLv3](https://www.gnu.org/licenses/gpl-3.0.html) licensed | Copyright © 2024 Bendik J. Ferkingstad, https://github.com/bendikf
