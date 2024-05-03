namespace Chess_Logic
{
    public enum Player
    {
        None,
        Red,
        Black
    }

    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            switch(player)
            {
                case Player.Red: return Player.Black;
                case Player.Black: return Player.Red;
                default: return Player.None;
            }
        }

    }
}
