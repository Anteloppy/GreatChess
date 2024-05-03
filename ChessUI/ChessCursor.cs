using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Chess_UI
{
    public static class ChessCursor
    {
        public static readonly Cursor RedCursor = LoadCursor("/Assets/red_cursor.cur");
        public static readonly Cursor BlackCursor = LoadCursor("/Assets/black_cursor.cur");

        private static Cursor LoadCursor(string filePath)
        {
            Stream stream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;
            return new Cursor(stream, true);
        }
    }
}
