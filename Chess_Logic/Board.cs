﻿namespace Chess_Logic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8,8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.Red, null },
            { Player.Black, null }
        };


        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value;  }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value;}
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.Red);
            this[7, 1] = new Knight(Player.Red);
            this[7, 2] = new Bishop(Player.Red);
            this[7, 3] = new Queen(Player.Red);
            this[7, 4] = new King(Player.Red);
            this[7, 5] = new Bishop(Player.Red);
            this[7, 6] = new Knight(Player.Red);
            this[7, 7] = new Rook(Player.Red);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.Red);
            }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);

                    if (!IsEmpty(pos))
                        yield return pos;
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();
            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        //public bool InsufficientMaterial()
        //{
        //    Counting counting = CountPieces();

        //    return IsKingVSKing(counting) || IsKingKnightVSKing(counting) || IsKingBishopVSKing(counting) || IsKingBishopVSKingBishop(counting);
        //}

        private static bool IsKingVSKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVSKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.Red(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVSKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.Red(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private static bool IsKingBishopVSKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
                return false;

            if (counting.Red(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
                return false;

            Board board = new Board();
            Position rBishopPos = board.FindPiece(Player.Red, PieceType.Bishop);
            Position bBishopPos = board.FindPiece(Player.Black, PieceType.Bishop);

            return rBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
                return false;

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;
        }

        public bool CastleRightKS(Player player)
        {
            switch (player)
            {
                case Player.Red: return IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7));
                case Player.Black: return IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7));
                default: return false;
            }
        }

        public bool CastleRightQS(Player player)
        {
            switch (player)
            {
                case Player.Red: return IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0));
                case Player.Black: return IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0));
                default: return false;
            }
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(IsInside))
            {
                Piece piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                    continue;

                EnPassant move = new EnPassant(pos, skipPos);
                if (move.IsLegal(this))
                    return true;
            }

            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());

            if (skipPos == null)
                return false;

            Position[] pawnPositions;

            switch (player)
            {
                case Player.Red: pawnPositions = new Position[] { skipPos + Direction.SouthEast, skipPos + Direction.SouthWest }; break;
                case Player.Black: pawnPositions = new Position[] { skipPos + Direction.NorthEast, skipPos + Direction.NorthWest }; break;
                default: pawnPositions = Array.Empty<Position>(); break;
            }

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}