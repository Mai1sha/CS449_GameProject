namespace SOSGame.Models
{
    public class SimpleGameLogic : GameLogic
    {
        public SimpleGameLogic(Board board) : base(board)
        {
        }

        public override List<SOSSequence> CheckForSOS(int row, int col, Player currentPlayer)
        {
            return DetectAllSOSSequences(row, col, currentPlayer);
        }

        public override bool ShouldSwitchPlayer(List<SOSSequence> sosSequences)
        {
            return true;
        }

        public override void UpdateGameState(List<SOSSequence> sosSequences, Player currentPlayer)
        {
            if (sosSequences.Count > 0)
            {
                _winner = currentPlayer;
                _gameOver = true;
                
                if (currentPlayer == Player.Blue)
                    _blueScore = 1;
                else
                    _redScore = 1;
            }
            else if (_board.IsFull())
            {
                _gameOver = true;
                _winner = null;
            }
        }

        protected override void DetermineWinner()
        {
        }
    }
}
