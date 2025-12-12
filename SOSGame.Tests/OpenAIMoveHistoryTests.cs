using Xunit;
using SOSGame.Models;
using System.Collections.Generic;

namespace SOSGame.Tests
{
    public class OpenAIMoveHistoryTests
    {
        [Fact]
        public void GameState_TracksMoveHistory()
        {
            // Arrange
            var gameState = new GameState(3, GameMode.Simple, PlayerType.Human, PlayerType.Human);

            // Act
            gameState.MakeMove(0, 0, CellValue.S);
            gameState.MakeMove(0, 1, CellValue.O);
            gameState.MakeMove(0, 2, CellValue.S);

            // Assert
            Assert.Equal(3, gameState.MoveHistory.Count);
            Assert.Equal((0, 0, CellValue.S, Player.Blue), gameState.MoveHistory[0]);
            Assert.Equal((0, 1, CellValue.O, Player.Red), gameState.MoveHistory[1]);
            Assert.Equal((0, 2, CellValue.S, Player.Blue), gameState.MoveHistory[2]);
        }

        [Fact]
        public void GameState_ClearsMoveHistoryOnReset()
        {
            // Arrange
            var gameState = new GameState(3, GameMode.Simple, PlayerType.Human, PlayerType.Human);
            gameState.MakeMove(0, 0, CellValue.S);
            gameState.MakeMove(0, 1, CellValue.O);

            // Act
            gameState.Reset();

            // Assert
            Assert.Empty(gameState.MoveHistory);
        }

        [Fact]
        public void GameState_MoveHistoryIsReadOnly()
        {
            // Arrange
            var gameState = new GameState(3, GameMode.Simple, PlayerType.Human, PlayerType.Human);
            gameState.MakeMove(0, 0, CellValue.S);

            // Assert
            Assert.IsAssignableFrom<IReadOnlyList<(int row, int col, CellValue value, Player player)>>(gameState.MoveHistory);
        }
    }
}
