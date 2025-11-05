namespace SOSGame.Models
{
    public class SOSSequence
    {
        public int StartRow { get; }
        public int StartCol { get; }
        public int EndRow { get; }
        public int EndCol { get; }
        public Player Player { get; }

        public SOSSequence(int startRow, int startCol, int endRow, int endCol, Player player)
        {
            StartRow = startRow;
            StartCol = startCol;
            EndRow = endRow;
            EndCol = endCol;
            Player = player;
        }
    }
}
