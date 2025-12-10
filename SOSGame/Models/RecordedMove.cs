namespace SOSGame.Models
{
    public class RecordedMove
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public CellValue Value { get; set; }
        public Player Player { get; set; }
        public int MoveNumber { get; set; }

        public RecordedMove(int row, int col, CellValue value, Player player, int moveNumber)
        {
            // Validation
            if (row < 0)
                throw new ArgumentException("Row must be non-negative", nameof(row));
            if (col < 0)
                throw new ArgumentException("Column must be non-negative", nameof(col));
            if (value == CellValue.Empty)
                throw new ArgumentException("Value cannot be Empty", nameof(value));
            if (moveNumber < 1)
                throw new ArgumentException("Move number must be at least 1", nameof(moveNumber));

            Row = row;
            Col = col;
            Value = value;
            Player = player;
            MoveNumber = moveNumber;
        }
    }
}
