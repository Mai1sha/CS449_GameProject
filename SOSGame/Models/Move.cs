namespace SOSGame.Models
{
    public class Move
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public CellValue Value { get; set; }

        public Move(int row, int col, CellValue value)
        {
            Row = row;
            Col = col;
            Value = value;
        }
    }
}
