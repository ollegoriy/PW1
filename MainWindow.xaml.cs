using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PW1
{
    public partial class MainWindow : Window
    {
        private bool isPlayerTurn;
        private string playerSymbol = "X";
        private string computerSymbol = "O";
        private Button[,] buttons;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[,] {
                { A1, A2, A3 },
                { B1, B2, B3 },
                { C1, C2, C3 }
            };
            NewGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || button.Content != null) return;

            button.Content = playerSymbol;
            if (CheckForWinner(playerSymbol))
            {
                StatusText.Text = $"Победитель: Игрок ({playerSymbol})";
                EndGame();
                return;
            }
            else if (IsDraw())
            {
                StatusText.Text = "Ничья!";
                EndGame();
                return;
            }

            isPlayerTurn = !isPlayerTurn;
            if (!isPlayerTurn)
            {
                ComputerMove();
            }
        }

        private void ComputerMove()
        {
            var availableButtons = buttons.Cast<Button>().Where(b => b.Content == null).ToList();
            if (availableButtons.Count > 0)
            {
                Button buttonToClick = availableButtons[random.Next(availableButtons.Count)];
                buttonToClick.Content = computerSymbol;
                if (CheckForWinner(computerSymbol))
                {
                    StatusText.Text = $"Победитель: Компьютер ({computerSymbol})";
                    EndGame();
                    return;
                }
                else if (IsDraw())
                {
                    StatusText.Text = "Ничья!";
                    EndGame();
                    return;
                }

                isPlayerTurn = true;
            }
        }

        private bool CheckForWinner(string symbol)
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Content == buttons[i, 1].Content &&
                    buttons[i, 1].Content == buttons[i, 2].Content &&
                    buttons[i, 0].Content != null)
                    return true;

                if (buttons[0, i].Content == buttons[1, i].Content &&
                    buttons[1, i].Content == buttons[2, i].Content &&
                    buttons[0, i].Content != null)
                    return true;
            }

            if ((buttons[0, 0].Content == buttons[1, 1].Content &&
                 buttons[1, 1].Content == buttons[2, 2].Content &&
                 buttons[0, 0].Content != null) ||
                (buttons[0, 2].Content == buttons[1, 1].Content &&
                 buttons[1, 1].Content == buttons[2, 0].Content &&
                 buttons[0, 2].Content != null))
                return true;

            if (IsDraw())
            {
                StatusText.Text = "Ничья!";
                DisableButtons();
                return false;
            }

            return false;
        }

        private bool IsDraw()
        {
            foreach (Button btn in buttons)
            {
                if (btn.Content == null)
                {
                    return false;
                }
            }
            StatusText.Text = "Ничья!";
            return true;
        }

        private void EndGame()
        {
            DisableButtons();
            if (playerSymbol == "X")
            {
                playerSymbol = "O";
                computerSymbol = "X";
            }
            else
            {
                playerSymbol = "X";
                computerSymbol = "O";
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            foreach (Button btn in buttons)
            {
                btn.IsEnabled = true;
                btn.Content = null;
            }
            if (playerSymbol == "X")
            {
                isPlayerTurn = true;
                StatusText.Text = $"Ход игрока ({playerSymbol})";
            }
            else
            {
                isPlayerTurn = false;
                ComputerMove();
                StatusText.Text = $"Ход игрока ({playerSymbol})";
            }

        }

        private void DisableButtons()
        {
            foreach (Button btn in buttons)
            {
                btn.IsEnabled = false;
            }
        }
    }
}
