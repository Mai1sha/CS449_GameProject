using Xunit;
using SOSGame.Models;

namespace SOSGame.Tests
{
    public class SimpleGameLogicTests
    {
        [Fact]
        public void SimpleGame_BlueWins_WhenBlueCreatesFirstSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            // Blue places S at (0,0)
            board.PlaceMove(0, 0, CellValue.S);
            // Red places O at (0,1)
            board.PlaceMove(0, 1, CellValue.O);
            // Blue places S at (0,2) - creates SOS horizontally
            board.PlaceMove(0, 2, CellValue.S);

            // Act
            var sosSequences = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sosSequences, Player.Blue);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Equal(Player.Blue, game.Winner);
            Assert.Equal(1, game.BlueScore);
            Assert.Equal(0, game.RedScore);
        }

        [Fact]
        public void SimpleGame_RedWins_WhenRedCreatesFirstSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            // Setup: S _ _
            //        O _ _
            //        _ _ _
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(1, 0, CellValue.O);
            // Red places S at (2,0) - creates vertical SOS
            board.PlaceMove(2, 0, CellValue.S);

            // Act
            var sosSequences = game.CheckForSOS(2, 0, Player.Red);
            game.UpdateGameState(sosSequences, Player.Red);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Equal(Player.Red, game.Winner);
            Assert.Equal(0, game.BlueScore);
            Assert.Equal(1, game.RedScore);
        }

        [Fact]
        public void SimpleGame_EndsInDraw_WhenBoardFullWithoutSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            // Fill board without creating SOS:
            // S S O
            // O O S
            // S S O
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.S);
            board.PlaceMove(0, 2, CellValue.O);
            board.PlaceMove(1, 0, CellValue.O);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(1, 2, CellValue.S);
            board.PlaceMove(2, 0, CellValue.S);
            board.PlaceMove(2, 1, CellValue.S);
            board.PlaceMove(2, 2, CellValue.O);

            // Act
            var sosSequences = game.CheckForSOS(2, 2, Player.Blue);
            game.UpdateGameState(sosSequences, Player.Blue);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Null(game.Winner);
            Assert.Equal(0, game.BlueScore);
            Assert.Equal(0, game.RedScore);
        }

        [Fact]
        public void SimpleGame_DetectsDiagonalSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            // S _ _
            // _ O _
            // _ _ S  <- Blue creates diagonal SOS
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(2, 2, CellValue.S);

            // Act
            var sosSequences = game.CheckForSOS(2, 2, Player.Blue);

            // Assert
            Assert.Single(sosSequences);
            Assert.Equal(0, sosSequences[0].StartRow);
            Assert.Equal(0, sosSequences[0].StartCol);
            Assert.Equal(2, sosSequences[0].EndRow);
            Assert.Equal(2, sosSequences[0].EndCol);
        }

        [Fact]
        public void SimpleGame_ShouldAlwaysSwitchPlayer()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            var sosSequences = new List<SOSSequence>();

            // Act & Assert - should switch even if SOS found
            Assert.True(game.ShouldSwitchPlayer(sosSequences));
            
            sosSequences.Add(new SOSSequence(0, 0, 0, 2, Player.Blue));
            Assert.True(game.ShouldSwitchPlayer(sosSequences));
        }

        [Fact]
        public void SimpleGame_GameNotOver_WhenSOSNotFoundAndBoardNotFull()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);

            // Act
            var sosSequences = game.CheckForSOS(0, 1, Player.Red);
            game.UpdateGameState(sosSequences, Player.Red);

            // Assert
            Assert.False(game.IsGameOver);
            Assert.Null(game.Winner);
        }

        [Fact]
        public void SimpleGame_Reset_ClearsGameState()
        {
            // Arrange
            var board = new Board(3);
            var game = new SimpleGameLogic(board);
            
            // Create a game over state
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            var sosSequences = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sosSequences, Player.Blue);

            // Act
            game.Reset();

            // Assert
            Assert.False(game.IsGameOver);
            Assert.Null(game.Winner);
            Assert.Equal(0, game.BlueScore);
            Assert.Equal(0, game.RedScore);
        }

        [Fact]
        public void SimpleGame_DetectsMultipleSOSDirections_ButEndsImmediately()
        {
            // Arrange
            var board = new Board(5);
            var game = new SimpleGameLogic(board);
            
            // Create a situation where placing one S creates SOS pattern
            // S O _ _ _
            // _ _ _ _ _
            // _ _ _ _ _
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            
            // Blue places S at (0,2) - creates horizontal SOS
            board.PlaceMove(0, 2, CellValue.S);

            // Act
            var sosSequences = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sosSequences, Player.Blue);

            // Assert - Game should end immediately after first SOS in simple mode
            Assert.True(game.IsGameOver);
            Assert.Equal(Player.Blue, game.Winner);
        }
    }
}
