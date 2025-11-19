using SOSGame.Models;

namespace SOSGame.Tests
{
    public class GameStateTests
    {
        [Fact]
        public void GameState_InitializesWithCorrectDefaults()
        {
            var gameState = new GameState(5, GameMode.Simple);
            
            Assert.Equal(5, gameState.Board.Size);
            Assert.Equal(GameMode.Simple, gameState.Mode);
            Assert.Equal(Player.Blue, gameState.CurrentPlayer);
        }

        [Fact]
        public void GameState_DefaultsToSimpleMode()
        {
            var gameState = new GameState(5);
            Assert.Equal(GameMode.Simple, gameState.Mode);
        }

        [Fact]
        public void MakeMove_SwitchesPlayer_AfterSuccessfulMove()
        {
            var gameState = new GameState(3, GameMode.Simple);
            
            Assert.Equal(Player.Blue, gameState.CurrentPlayer);
            
            gameState.MakeMove(0, 0, CellValue.S);
            
            Assert.Equal(Player.Red, gameState.CurrentPlayer);
        }

        [Fact]
        public void MakeMove_DoesNotSwitchPlayer_WhenMoveInvalid()
        {
            var gameState = new GameState(3, GameMode.Simple);
            
            gameState.MakeMove(0, 0, CellValue.S);
            Player currentPlayer = gameState.CurrentPlayer;
            
            // Try to place on occupied cell
            gameState.MakeMove(0, 0, CellValue.O);
            
            Assert.Equal(currentPlayer, gameState.CurrentPlayer);
        }

        [Fact]
        public void MakeMove_ReturnsTrue_ForValidMove()
        {
            var gameState = new GameState(3, GameMode.Simple);
            bool result = gameState.MakeMove(0, 0, CellValue.S);
            
            Assert.True(result);
        }

        [Fact]
        public void MakeMove_ReturnsFalse_ForInvalidMove()
        {
            var gameState = new GameState(3, GameMode.Simple);
            gameState.MakeMove(0, 0, CellValue.S);
            bool result = gameState.MakeMove(0, 0, CellValue.O);
            
            Assert.False(result);
        }

        [Fact]
        public void MakeMove_AlternatesPlayers_ForMultipleMoves()
        {
            var gameState = new GameState(3, GameMode.Simple);
            
            Assert.Equal(Player.Blue, gameState.CurrentPlayer);
            
            gameState.MakeMove(0, 0, CellValue.S);
            Assert.Equal(Player.Red, gameState.CurrentPlayer);
            
            gameState.MakeMove(0, 1, CellValue.O);
            Assert.Equal(Player.Blue, gameState.CurrentPlayer);
            
            gameState.MakeMove(1, 0, CellValue.S);
            Assert.Equal(Player.Red, gameState.CurrentPlayer);
        }

        [Fact]
        public void Reset_ResetsToInitialState()
        {
            var gameState = new GameState(3, GameMode.General);
            
            // Make some moves
            gameState.MakeMove(0, 0, CellValue.S);
            gameState.MakeMove(0, 1, CellValue.O);
            gameState.MakeMove(1, 0, CellValue.S);
            
            // Reset
            gameState.Reset();
            
            // Check that board is empty
            Assert.True(gameState.Board.IsCellEmpty(0, 0));
            Assert.True(gameState.Board.IsCellEmpty(0, 1));
            Assert.True(gameState.Board.IsCellEmpty(1, 0));
            
            // Check that player is reset to Blue
            Assert.Equal(Player.Blue, gameState.CurrentPlayer);
        }

        [Fact]
        public void GameState_WorksWithDifferentBoardSizes()
        {
            var smallGame = new GameState(3, GameMode.Simple);
            var largeGame = new GameState(10, GameMode.General);
            
            Assert.Equal(3, smallGame.Board.Size);
            Assert.Equal(10, largeGame.Board.Size);
        }

        [Fact]
        public void GameState_SupportsSimpleMode()
        {
            var gameState = new GameState(5, GameMode.Simple);
            Assert.Equal(GameMode.Simple, gameState.Mode);
        }

        [Fact]
        public void GameState_SupportsGeneralMode()
        {
            var gameState = new GameState(5, GameMode.General);
            Assert.Equal(GameMode.General, gameState.Mode);
        }

        [Fact]
        public void GameState_InitializesWithHumanPlayers_ByDefault()
        {
            var gameState = new GameState(5, GameMode.Simple);
            
            Assert.NotNull(gameState.BluePlayerController);
            Assert.NotNull(gameState.RedPlayerController);
            Assert.True(gameState.BluePlayerController.IsHuman());
            Assert.True(gameState.RedPlayerController.IsHuman());
        }

        [Fact]
        public void GameState_InitializesWithComputerPlayers()
        {
            var gameState = new GameState(5, GameMode.Simple, PlayerType.Computer, PlayerType.Computer);
            
            Assert.True(gameState.BluePlayerController.IsComputer());
            Assert.True(gameState.RedPlayerController.IsComputer());
        }

        [Fact]
        public void GameState_InitializesWithMixedPlayers()
        {
            var gameState = new GameState(5, GameMode.General, PlayerType.Human, PlayerType.Computer);
            
            Assert.True(gameState.BluePlayerController.IsHuman());
            Assert.True(gameState.RedPlayerController.IsComputer());
        }

        [Fact]
        public void GameState_GetComputerMove_ReturnsNullForHumanPlayer()
        {
            var gameState = new GameState(5, GameMode.Simple, PlayerType.Human, PlayerType.Human);
            
            var move = gameState.GetComputerMove();
            
            Assert.Null(move);
        }

        [Fact]
        public void GameState_GetComputerMove_ReturnsValidMoveForComputerPlayer()
        {
            var gameState = new GameState(5, GameMode.Simple, PlayerType.Computer, PlayerType.Human);
            
            var move = gameState.GetComputerMove();
            
            Assert.NotNull(move);
            Assert.True(move.Row >= 0 && move.Row < 5);
            Assert.True(move.Col >= 0 && move.Col < 5);
        }

        [Fact]
        public void GameState_CurrentPlayerController_ReturnsCorrectController()
        {
            var gameState = new GameState(3, GameMode.Simple, PlayerType.Computer, PlayerType.Human);
            
            // Blue's turn (computer)
            Assert.Equal(gameState.BluePlayerController, gameState.CurrentPlayerController);
            Assert.True(gameState.CurrentPlayerController.IsComputer());
            
            // Make a move to switch to Red
            gameState.MakeMove(0, 0, CellValue.S);
            
            // Red's turn (human)
            Assert.Equal(gameState.RedPlayerController, gameState.CurrentPlayerController);
            Assert.True(gameState.CurrentPlayerController.IsHuman());
        }

        [Fact]
        public void GameState_GetComputerMove_ReturnsNullWhenGameOver()
        {
            var gameState = new GameState(3, GameMode.Simple, PlayerType.Computer, PlayerType.Computer);
            
            // Create winning condition
            gameState.MakeMove(0, 0, CellValue.S);
            gameState.MakeMove(0, 1, CellValue.O);
            gameState.MakeMove(0, 2, CellValue.S);
            
            // Game should be over
            if (gameState.IsGameOver)
            {
                var move = gameState.GetComputerMove();
                Assert.Null(move);
            }
        }
    }
}
