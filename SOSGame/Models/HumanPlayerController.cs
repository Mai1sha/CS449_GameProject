namespace SOSGame.Models
{
    /// <summary>
    /// Controller for human players. Returns null for GetMove() as humans make moves through UI.
    /// </summary>
    public class HumanPlayerController : PlayerController
    {
        public HumanPlayerController(Player player) : base(player, PlayerType.Human)
        {
        }

        /// <summary>
        /// Human players don't auto-generate moves; they input through UI.
        /// Always returns null.
        /// </summary>
        public override Move? GetMove(Board board, GameLogic gameLogic, GameMode gameMode)
        {
            // Human players make moves through UI interaction
            return null;
        }
    }
}
