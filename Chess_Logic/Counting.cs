namespace Chess_Logic
{
    public class Counting
    {
        private readonly Dictionary<PieceType, int> redCount = new Dictionary<PieceType, int>();
        private readonly Dictionary<PieceType, int> blackCount = new Dictionary<PieceType, int>();

        public int TotalCount { get; private set; }

        public Counting()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                redCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        public void Increment(Player color, PieceType type)
        {
            if (color == Player.Red)
                redCount[type]++;
            else if (color == Player.Black)
                blackCount[type]++;

            TotalCount++;
        }

        public int Red(PieceType type)
        {
            return redCount[type];
        }

        public int Black(PieceType type)
        {
            return blackCount[type];
        }
    }
}
