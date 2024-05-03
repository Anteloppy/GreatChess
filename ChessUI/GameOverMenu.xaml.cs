using Chess_Logic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Chess_UI
{
    /// <summary>
    /// Логика взаимодействия для GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;

        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }

        private static string GetWinnerText(Player winner)
        {
            switch (winner)
            {
                case Player.Red: return "RED WINS!";
                case Player.Black: return "BLACK WINS!";
                case Player.None: return "IT'S A DRAW";
                default: return "";
            }
        }

        private static string PlayerString(Player player)
        {
            switch (player)
            {
                case Player.Red: return "RED";
                case Player.Black: return "BLACK";
                case Player.None: return "";
                default: return "";
            }
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            switch (reason)
            {
                case EndReason.Stallemate: return $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE";
                case EndReason.Checkmate: return $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE";
                case EndReason.FiftyMoveRule: return "FIFTY-MOVE RULE";
                case EndReason.InsufficientMaterial: return "INSUFFICIENT MATERIAL";
                case EndReason.ThreefoldRepetition: return "THREEFOLD REPETITION";
                default: return "";
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
