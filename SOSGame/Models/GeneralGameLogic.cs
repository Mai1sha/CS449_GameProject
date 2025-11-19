namespace SOSGame.Models
{
    public class GeneralGameLogic : GameLogic
    {
        public GeneralGameLogic(Board board) : base(board)
        {
        }

        public override List<SOSSequence> CheckForSOS(int row, int col, Player currentPlayer)
        {
            return DetectAllSOSSequences(row, col, currentPlayer);
        }

        public override bool ShouldSwitchPlayer(List<SOSSequence> sosSequences)
        {
            return sosSequences.Count == 0;
        }

        public override void UpdateGameState(List<SOSSequence> sosSequences, Player currentPlayer)
        {
            if (sosSequences.Count > 0)
            {
                if (currentPlayer == Player.Blue)
                    _blueScore += sosSequences.Count;
                else
                    _redScore += sosSequences.Count;
            }

            if (_board.IsFull())
            {
                _gameOver = true;
                DetermineWinner();
            }
        }

        protected override void DetermineWinner()
        {
            if (_blueScore > _redScore)
                _winner = Player.Blue;
            else if (_redScore > _blueScore)
                _winner = Player.Red;
            else
                _winner = null;
        }
    }
}
