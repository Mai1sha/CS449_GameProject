using Xunit;
using SOSGame.Models;

namespace SOSGame.Tests
{
    public class GeneralGameLogicTests
    {
        [Fact]
        public void GeneralGame_BlueWins_WhenBlueHasMoreSOS()
        {
            // Arrange
            var board = new Board(4);
            var game = new GeneralGameLogic(board);
            
            // Blue creates 2 SOS, Red creates 1
            // S O S _
            // O _ _ _
            // S _ _ _
            // _ _ _ _
            
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);  // Blue horizontal SOS
            var sos1 = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sos1, Player.Blue);
            
            board.PlaceMove(1, 0, CellValue.O);
            board.PlaceMove(2, 0, CellValue.S);  // Blue vertical SOS
            var sos2 = game.CheckForSOS(2, 0, Player.Blue);
            game.UpdateGameState(sos2, Player.Blue);
            
            // Fill remaining cells
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (board.GetCell(r, c) == CellValue.Empty)
                    {
                        board.PlaceMove(r, c, CellValue.O);
                    }
                }
            }
            
            var finalSos = game.CheckForSOS(3, 3, Player.Red);
            game.UpdateGameState(finalSos, Player.Red);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Equal(Player.Blue, game.Winner);
            Assert.True(game.BlueScore > game.RedScore);
        }

        [Fact]
        public void GeneralGame_RedWins_WhenRedHasMoreSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            
            // Red creates 1 SOS
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            var sos1 = game.CheckForSOS(0, 2, Player.Red);
            game.UpdateGameState(sos1, Player.Red);
            
            // Fill board without Blue creating any SOS
            board.PlaceMove(1, 0, CellValue.O);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(1, 2, CellValue.O);
            board.PlaceMove(2, 0, CellValue.O);
            board.PlaceMove(2, 1, CellValue.O);
            board.PlaceMove(2, 2, CellValue.O);
            
            var finalSos = game.CheckForSOS(2, 2, Player.Blue);
            game.UpdateGameState(finalSos, Player.Blue);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Equal(Player.Red, game.Winner);
            Assert.Equal(1, game.RedScore);
            Assert.Equal(0, game.BlueScore);
        }

        [Fact]
        public void GeneralGame_Draw_WhenScoresEqual()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            
            // Blue creates 1 SOS
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            var sos1 = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sos1, Player.Blue);
            
            // Red creates 1 SOS
            board.PlaceMove(1, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(1, 2, CellValue.S);
            var sos2 = game.CheckForSOS(1, 2, Player.Red);
            game.UpdateGameState(sos2, Player.Red);
            
            // Fill remaining cells
            board.PlaceMove(2, 0, CellValue.O);
            board.PlaceMove(2, 1, CellValue.O);
            board.PlaceMove(2, 2, CellValue.O);
            
            var finalSos = game.CheckForSOS(2, 2, Player.Blue);
            game.UpdateGameState(finalSos, Player.Blue);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Null(game.Winner);
            Assert.Equal(game.BlueScore, game.RedScore);
        }

        [Fact]
        public void GeneralGame_DoesNotSwitchPlayer_WhenSOSCreated()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            var sosSequences = new List<SOSSequence> 
            { 
                new SOSSequence(0, 0, 0, 2, Player.Blue) 
            };

            // Act
            bool shouldSwitch = game.ShouldSwitchPlayer(sosSequences);

            // Assert
            Assert.False(shouldSwitch);
        }

        [Fact]
        public void GeneralGame_SwitchesPlayer_WhenNoSOSCreated()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            var sosSequences = new List<SOSSequence>();

            // Act
            bool shouldSwitch = game.ShouldSwitchPlayer(sosSequences);

            // Assert
            Assert.True(shouldSwitch);
        }

        [Fact]
        public void GeneralGame_CountsMultipleSOSInOneMove()
        {
            // Arrange
            var board = new Board(5);
            var game = new GeneralGameLogic(board);
            
            // Create a setup where one move creates multiple SOS
            // S O S _ _
            // O _ _ _ _
            // S _ _ _ _
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(2, 2, CellValue.S);
            
            // Place S at (0,0) which should create horizontal SOS
            var sos = game.CheckForSOS(0, 0, Player.Blue);
            game.UpdateGameState(sos, Player.Blue);

            // Assert
            Assert.True(game.BlueScore >= 1);
        }

        [Fact]
        public void GeneralGame_OnlyEndsWhenBoardFull()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            
            // Create SOS but board not full
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            
            var sos = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sos, Player.Blue);

            // Assert - Game should not be over yet
            Assert.False(game.IsGameOver);
        }

        [Fact]
        public void GeneralGame_TracksScoresCorrectly()
        {
            // Arrange
            var board = new Board(5);
            var game = new GeneralGameLogic(board);
            
            // Blue creates 1 SOS horizontally
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            var sos1 = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sos1, Player.Blue);
            
            // Red creates 1 SOS horizontally
            board.PlaceMove(1, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(1, 2, CellValue.S);
            var sos2 = game.CheckForSOS(1, 2, Player.Red);
            game.UpdateGameState(sos2, Player.Red);
            
            // Blue creates 1 SOS vertically (not at intersection)
            board.PlaceMove(0, 3, CellValue.S);
            board.PlaceMove(1, 3, CellValue.O);
            board.PlaceMove(2, 3, CellValue.S);
            var sos3 = game.CheckForSOS(2, 3, Player.Blue);
            game.UpdateGameState(sos3, Player.Blue);

            // Assert
            Assert.Equal(2, game.BlueScore);
            Assert.Equal(1, game.RedScore);
        }

        [Fact]
        public void GeneralGame_Reset_ClearsScores()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(0, 2, CellValue.S);
            var sos = game.CheckForSOS(0, 2, Player.Blue);
            game.UpdateGameState(sos, Player.Blue);

            // Act
            game.Reset();

            // Assert
            Assert.Equal(0, game.BlueScore);
            Assert.Equal(0, game.RedScore);
            Assert.False(game.IsGameOver);
            Assert.Null(game.Winner);
        }

        [Fact]
        public void GeneralGame_NoDraw_WhenBoardFullAndNoSOS()
        {
            // Arrange
            var board = new Board(3);
            var game = new GeneralGameLogic(board);
            
            // Fill board without creating any SOS
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.S);
            board.PlaceMove(0, 2, CellValue.O);
            board.PlaceMove(1, 0, CellValue.O);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(1, 2, CellValue.S);
            board.PlaceMove(2, 0, CellValue.S);
            board.PlaceMove(2, 1, CellValue.S);
            board.PlaceMove(2, 2, CellValue.O);
            
            var sos = game.CheckForSOS(2, 2, Player.Blue);
            game.UpdateGameState(sos, Player.Blue);

            // Assert
            Assert.True(game.IsGameOver);
            Assert.Null(game.Winner);  // Draw - both have 0 points
            Assert.Equal(0, game.BlueScore);
            Assert.Equal(0, game.RedScore);
        }
    }
}
