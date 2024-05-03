using System.Text;

namespace Chess_Logic
{
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public StateString(Player currentPlayer, Board board)
        {
            AddPiecePlacement(board);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
            AddCastlingRights(board);
            sb.Append(' ');
            AddEnPassant(board, currentPlayer);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char c;
            switch (piece.Type)
            {
                case PieceType.Pawn: c = 'p'; break;
                case PieceType.Rook: c = 'r'; break;
                case PieceType.Knight: c = 'n'; break;
                case PieceType.Bishop: c = 'b'; break;
                case PieceType.Queen: c = 'q'; break;
                case PieceType.King: c = 'k'; break;
                default: c = ' '; break;
            }

            if (piece.Color == Player.Red)
                return char.ToUpper(c);

            return c;
        }

        private void AddRowData(Board board, int row)
        {
            int empty = 0;

            for (int c = 0; c < 8; c++)
            {
                if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PieceChar(board[row, c]));
            }

            if (empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                if (r != 0)
                    sb.Append('/');

                AddRowData(board, r);
            }
        }

        private void AddCurrentPlayer(Player currentPlayer)
        {
            if (currentPlayer == Player.Red)
                sb.Append('e');
            else
                sb.Append('b');
        }

        private void AddCastlingRights(Board board)
        {
            bool castleRKS = board.CastleRightKS(Player.Red);
            bool castleRQS = board.CastleRightQS(Player.Red);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            if (!(castleRKS || castleRQS || castleBKS || castleBQS))
            {
                sb.Append('-');
                return;
            }
            if (castleRKS)
                sb.Append('K');
            if (castleRQS)
                sb.Append('Q');
            if (castleBKS)
                sb.Append('k');
            if (castleBQS)
                sb.Append('q');
        }

        private void AddEnPassant(Board board, Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            sb.Append(file);
            sb.Append(rank);
        }
    }
}
