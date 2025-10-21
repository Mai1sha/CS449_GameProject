using SOSGame_2.Models;

namespace SOSGame_2.Tests
{
    public class BoardTests
    {
        [Fact]
        public void Board_InitializedWithCorrectSize()
        {
            var board = new Board(5);
            Assert.Equal(5, board.Size);
        }

        [Fact]
        public void Board_ThrowsException_WhenSizeLessThan3()
        {
            Assert.Throws<ArgumentException>(() => new Board(2));
        }

        [Fact]
        public void Board_AllCellsEmpty_OnInitialization()
        {
            var board = new Board(3);
            
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Assert.Equal(CellValue.Empty, board.GetCell(row, col));
                }
            }
        }

        [Fact]
        public void PlaceMove_ReturnsTrue_WhenCellIsEmpty()
        {
            var board = new Board(3);
            bool result = board.PlaceMove(0, 0, CellValue.S);
            
            Assert.True(result);
            Assert.Equal(CellValue.S, board.GetCell(0, 0));
        }

        [Fact]
        public void PlaceMove_ReturnsFalse_WhenCellIsOccupied()
        {
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S);
            bool result = board.PlaceMove(0, 0, CellValue.O);
            
            Assert.False(result);
            Assert.Equal(CellValue.S, board.GetCell(0, 0));
        }

        [Fact]
        public void PlaceMove_ThrowsException_WhenValueIsEmpty()
        {
            var board = new Board(3);
            Assert.Throws<ArgumentException>(() => board.PlaceMove(0, 0, CellValue.Empty));
        }

        [Fact]
        public void PlaceMove_ThrowsException_WhenCoordinatesOutOfBounds()
        {
            var board = new Board(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => board.PlaceMove(5, 5, CellValue.S));
        }

        [Fact]
        public void IsCellEmpty_ReturnsTrue_ForEmptyCell()
        {
            var board = new Board(3);
            Assert.True(board.IsCellEmpty(0, 0));
        }

        [Fact]
        public void IsCellEmpty_ReturnsFalse_ForOccupiedCell()
        {
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S);
            Assert.False(board.IsCellEmpty(0, 0));
        }

        [Fact]
        public void IsFull_ReturnsFalse_WhenBoardHasEmptyCells()
        {
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S);
            Assert.False(board.IsFull());
        }

        [Fact]
        public void IsFull_ReturnsTrue_WhenAllCellsOccupied()
        {
            var board = new Board(3);
            
            // Fill all cells
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board.PlaceMove(row, col, row % 2 == 0 ? CellValue.S : CellValue.O);
                }
            }
            
            Assert.True(board.IsFull());
        }

        [Fact]
        public void Reset_ClearsAllCells()
        {
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            
            board.Reset();
            
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Assert.Equal(CellValue.Empty, board.GetCell(row, col));
                }
            }
        }
    }
}
