using SOSGame.Models;

namespace SOSGame.Tests
{
    public class PlayerControllerTests
    {
        [Fact]
        public void HumanPlayerController_IsHuman_ReturnsTrue()
        {
            var controller = new HumanPlayerController(Player.Blue);
            
            Assert.True(controller.IsHuman());
            Assert.False(controller.IsComputer());
        }

        [Fact]
        public void HumanPlayerController_GetMove_ReturnsNull()
        {
            var controller = new HumanPlayerController(Player.Blue);
            var board = new Board(3);
            var gameLogic = new SimpleGameLogic(board);
            
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            Assert.Null(move);
        }

        [Fact]
        public void ComputerPlayerController_IsComputer_ReturnsTrue()
        {
            var controller = new ComputerPlayerController(Player.Red);
            
            Assert.True(controller.IsComputer());
            Assert.False(controller.IsHuman());
        }

        [Fact]
        public void ComputerPlayerController_GetMove_ReturnsValidMove()
        {
            var controller = new ComputerPlayerController(Player.Blue);
            var board = new Board(3);
            var gameLogic = new SimpleGameLogic(board);
            
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            Assert.NotNull(move);
            Assert.True(move.Row >= 0 && move.Row < 3);
            Assert.True(move.Col >= 0 && move.Col < 3);
            Assert.True(move.Value == CellValue.S || move.Value == CellValue.O);
        }

        [Fact]
        public void ComputerPlayerController_GetMove_ReturnsNullWhenBoardFull()
        {
            var controller = new ComputerPlayerController(Player.Blue);
            var board = new Board(3);
            var gameLogic = new SimpleGameLogic(board);
            
            // Fill the board
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board.PlaceMove(row, col, CellValue.S);
                }
            }
            
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            Assert.Null(move);
        }

        [Fact]
        public void PlayerController_Player_Property_ReturnsCorrectPlayer()
        {
            var blueController = new HumanPlayerController(Player.Blue);
            var redController = new ComputerPlayerController(Player.Red);
            
            Assert.Equal(Player.Blue, blueController.Player);
            Assert.Equal(Player.Red, redController.Player);
        }

        [Fact]
        public void PlayerController_PlayerType_Property_ReturnsCorrectType()
        {
            var humanController = new HumanPlayerController(Player.Blue);
            var computerController = new ComputerPlayerController(Player.Red);
            
            Assert.Equal(PlayerType.Human, humanController.PlayerType);
            Assert.Equal(PlayerType.Computer, computerController.PlayerType);
        }
    }
}
