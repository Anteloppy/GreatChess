using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Logic
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly PieceType newType;

        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            FromPos = from;
            ToPos = to;
            this.newType = newType;
        }

        private Piece CreatePromotionPiece(Player color)
        {
            switch (newType)
            {
                case PieceType.Rook: return new Rook(color);
                case PieceType.Knight: return new Knight(color);
                case PieceType.Bishop: return new Bishop(color);
                default: return new Queen(color);
            }
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece promotionPiec = CreatePromotionPiece(pawn.Color);
            promotionPiec.HasMoved = true;
            board[ToPos] = promotionPiec;

            return true;
        }
    }
}
