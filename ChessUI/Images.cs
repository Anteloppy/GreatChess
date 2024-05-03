using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chess_Logic;

namespace Chess_UI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> redSources = new Dictionary<PieceType, ImageSource>()
        {
            { PieceType.Pawn, LoadImage("/Assets/red_pawn.png") },
            { PieceType.Rook, LoadImage("/Assets/red_rook.png") },
            { PieceType.Knight, LoadImage("/Assets/red_knight.png") },
            { PieceType.Bishop, LoadImage("/Assets/red_bishop.png") },
            { PieceType.King, LoadImage("/Assets/red_king.png") },
            { PieceType.Queen, LoadImage("/Assets/red_queen.png") }
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new Dictionary<PieceType, ImageSource>()
        {
            { PieceType.Pawn, LoadImage("/Assets/black_pawn.png") },
            { PieceType.Rook, LoadImage("/Assets/black_rook.png") },
            { PieceType.Knight, LoadImage("/Assets/black_knight.png") },
            { PieceType.Bishop, LoadImage("/Assets/black_bishop.png") },
            { PieceType.King, LoadImage("/Assets/black_king.png") },
            { PieceType.Queen, LoadImage("/Assets/black_queen.png") }
        };

        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType type)
        {
            switch (color)
            {
                case Player.Red: return redSources[type];
                case Player.Black: return blackSources[type];
                default: return null;
            };  
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
                return null;
            return GetImage(piece.Color, piece.Type);
        }
    }
}
