namespace SOSGame.Models
{
    /// <summary>
    /// Abstract base class for player controllers.
    /// Implements the Strategy pattern to handle different player types (Human vs Computer).
    /// </summary>
    public abstract class PlayerController
    {
        protected readonly Player _player;
        protected readonly PlayerType _playerType;

        public Player Player => _player;
        public PlayerType PlayerType => _playerType;

        protected PlayerController(Player player, PlayerType playerType)
        {
            _player = player;
            _playerType = playerType;
        }

        /// <summary>
        /// Determines the next move for this player.
        /// Human players return null (waiting for UI input).
        /// Computer players calculate and return a move.
        /// </summary>
        public abstract Move? GetMove(Board board, GameLogic gameLogic, GameMode gameMode);

        /// <summary>
        /// Checks if this controller represents a human player.
        /// </summary>
        public bool IsHuman() => _playerType == PlayerType.Human;

        /// <summary>
        /// Checks if this controller represents a computer player.
        /// </summary>
        public bool IsComputer() => _playerType == PlayerType.Computer;

        public bool IsOpenAI() => _playerType == PlayerType.OpenAI;
    }
}
