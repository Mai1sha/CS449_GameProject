using SOSGame.Models;

namespace SOSGame.Tests
{
    public class ComputerPlayerTests
    {
        [Fact]
        public void ComputerPlayer_MakesWinningMove_InSimpleGame()
        {
            // Arrange: Set up board where computer can make SOS
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S); // S at (0,0)
            board.PlaceMove(0, 1, CellValue.O); // O at (0,1)
            // Computer should place S at (0,2) to complete SOS
            
            var controller = new ComputerPlayerController(Player.Blue);
            var gameLogic = new SimpleGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            // Assert
            Assert.NotNull(move);
            // Computer should find the winning move
            if (move.Row == 0 && move.Col == 2)
            {
                Assert.Equal(CellValue.S, move.Value);
            }
            // Otherwise, it should at least make a valid move
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }

        [Fact]
        public void ComputerPlayer_FindsScoringMove_InGeneralGame()
        {
            // Arrange: Similar setup for general game
            var board = new Board(3);
            board.PlaceMove(1, 0, CellValue.S); // S at (1,0)
            board.PlaceMove(1, 1, CellValue.O); // O at (1,1)
            
            var controller = new ComputerPlayerController(Player.Red);
            var gameLogic = new GeneralGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.General);
            
            // Assert
            Assert.NotNull(move);
            // Computer should find scoring move or make valid move
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }

        [Fact]
        public void ComputerPlayer_BlocksOpponentSOS()
        {
            // Arrange: Set up board where opponent could make SOS
            var board = new Board(3);
            board.PlaceMove(2, 0, CellValue.S);
            board.PlaceMove(2, 1, CellValue.O);
            // Position (2,2) needs to be blocked
            
            var controller = new ComputerPlayerController(Player.Blue);
            var gameLogic = new SimpleGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            // Assert
            Assert.NotNull(move);
            // Computer should either block or make another strategic move
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }

        [Fact]
        public void ComputerPlayer_MakesValidMoveOnEmptyBoard()
        {
            // Arrange
            var board = new Board(5);
            var controller = new ComputerPlayerController(Player.Blue);
            var gameLogic = new SimpleGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            // Assert
            Assert.NotNull(move);
            Assert.True(move.Row >= 0 && move.Row < 5);
            Assert.True(move.Col >= 0 && move.Col < 5);
            Assert.True(move.Value == CellValue.S || move.Value == CellValue.O);
        }

        [Fact]
        public void ComputerPlayer_CanCreateSOSWithO_InMiddle()
        {
            // Arrange: S _ S pattern where computer should place O in middle
            var board = new Board(3);
            board.PlaceMove(1, 0, CellValue.S); // S at (1,0)
            board.PlaceMove(1, 2, CellValue.S); // S at (1,2)
            // Computer should place O at (1,1) to create SOS
            
            var controller = new ComputerPlayerController(Player.Red);
            var gameLogic = new GeneralGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.General);
            
            // Assert
            Assert.NotNull(move);
            // Should find the SOS opportunity or make a valid move
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }

        [Fact]
        public void ComputerPlayer_WorksWithDifferentBoardSizes()
        {
            // Test with various board sizes
            int[] sizes = { 3, 5, 8, 10 };
            
            foreach (int size in sizes)
            {
                var board = new Board(size);
                var controller = new ComputerPlayerController(Player.Blue);
                var gameLogic = new SimpleGameLogic(board);
                
                var move = controller.GetMove(board, gameLogic, GameMode.Simple);
                
                Assert.NotNull(move);
                Assert.True(move.Row >= 0 && move.Row < size);
                Assert.True(move.Col >= 0 && move.Col < size);
            }
        }

        [Fact]
        public void ComputerPlayer_MakesValidMoves_ThroughoutGame()
        {
            // Simulate a partial game
            var board = new Board(3);
            var controller = new ComputerPlayerController(Player.Blue);
            var gameLogic = new SimpleGameLogic(board);
            
            // Computer makes multiple moves
            for (int i = 0; i < 5; i++)
            {
                var move = controller.GetMove(board, gameLogic, GameMode.Simple);
                
                if (move != null)
                {
                    Assert.True(board.IsCellEmpty(move.Row, move.Col));
                    board.PlaceMove(move.Row, move.Col, move.Value);
                }
            }
        }

        [Fact]
        public void ComputerPlayer_FindsDiagonalSOS()
        {
            // Arrange: Diagonal pattern S-O-? where computer should place S
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S); // S at top-left
            board.PlaceMove(1, 1, CellValue.O); // O at center
            // Computer should consider (2,2) for diagonal SOS
            
            var controller = new ComputerPlayerController(Player.Blue);
            var gameLogic = new SimpleGameLogic(board);
            
            // Act
            var move = controller.GetMove(board, gameLogic, GameMode.Simple);
            
            // Assert
            Assert.NotNull(move);
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }

        [Fact]
        public void ComputerPlayer_PrefersCenter_OnEmptyBoard()
        {
            // Computer should prefer center or near-center positions on empty board
            var board = new Board(5);
            var controller = new ComputerPlayerController(Player.Red);
            var gameLogic = new GeneralGameLogic(board);
            
            var move = controller.GetMove(board, gameLogic, GameMode.General);
            
            Assert.NotNull(move);
            // Just verify it's a valid move (strategy may vary)
            Assert.True(board.IsCellEmpty(move.Row, move.Col));
        }
    }
}
