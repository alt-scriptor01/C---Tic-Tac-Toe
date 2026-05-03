using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe2
{
    public partial class Form1 : Form
    {
        private Button[,] buttons = new Button[3, 3];
        private Label statusLabel, scoreLabel;
        private TableLayoutPanel boardPanel;
        private Random rand = new Random();
        private string playerName = "Player";
        private int playerWins = 0, computerWins = 0, ties = 0;
        private bool playerTurn;

        public Form1()
        {
            InitializeComponent();
            SetupGameUI();
            StartNewGame();
        }

        private void SetupGameUI()
        {
            this.Text = "Tic Tac Toe";
            this.Size = new Size(400, 500);
            this.MinimumSize = new Size(300, 400);
            this.MaximumSize = new Size(600, 700);

            var mainPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
            this.Controls.Add(mainPanel);

            scoreLabel = new Label { Text = "Wins: 0 | Losses: 0 | Ties: 0", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            mainPanel.Controls.Add(scoreLabel);

            boardPanel = new TableLayoutPanel { RowCount = 3, ColumnCount = 3, Dock = DockStyle.Fill };
            mainPanel.Controls.Add(boardPanel);

            for (int i = 0; i < 3; i++)
            {   
                for (int j = 0; j < 3; j++)
                {
                    var button = new Button { Dock = DockStyle.Fill, Font = new Font("Arial", 24, FontStyle.Bold) };
                    button.Click += Button_Click;
                    buttons[i, j] = button;
                    boardPanel.Controls.Add(button, j, i);
                }
            }

            statusLabel = new Label { Text = "Welcome!", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            mainPanel.Controls.Add(statusLabel);

            var controlPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true };
            mainPanel.Controls.Add(controlPanel);

            var newGameButton = new Button { Text = "New Game" };
            newGameButton.Click += (s, e) => StartNewGame();
            controlPanel.Controls.Add(newGameButton);

            var exitButton = new Button { Text = "Exit" };
            exitButton.Click += (s, e) => Application.Exit();
            controlPanel.Controls.Add(exitButton);
        }

        private void StartNewGame()
        {
            foreach (var button in buttons) button.Text = "";
            playerTurn = rand.Next(2) == 0;
            statusLabel.Text = playerTurn ? $"{playerName}'s turn (X)" : "Computer's turn (O)";
            if (!playerTurn) ComputerMove();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (!playerTurn) return;

            var button = sender as Button;
            if (button.Text != "") return;

            button.Text = "X";
            playerTurn = false;
            statusLabel.Text = "Computer's turn (O)";

            if (!CheckWinner()) ComputerMove();
        }

        private void ComputerMove()
        {
            var emptyButtons = buttons.Cast<Button>().Where(b => b.Text == "").ToList();
            if (emptyButtons.Count > 0)
            {
                var move = emptyButtons[rand.Next(emptyButtons.Count)];
                move.Text = "O";
                playerTurn = true;
                statusLabel.Text = $"{playerName}'s turn (X)";
                CheckWinner();
            }
        }

        private bool CheckWinner()
        {
            string[,] board = new string[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    board[i, j] = buttons[i, j].Text;

            string winner = null;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) winner = board[i, 0];
                if (board[0, i] != "" && board[0, i] == board[1, i] && board[1, i] == board[2, i]) winner = board[0, i];
            }
            if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) winner = board[0, 0];
            if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) winner = board[0, 2];

            if (winner != null)
            {
                if (winner == "X")
                {
                    playerWins++;
                    statusLabel.Text = $"{playerName} Wins!";
                }
                else
                {
                    computerWins++;
                    statusLabel.Text = "Computer Wins!";
                }
                UpdateScore();
                return true;
            }

            if (buttons.Cast<Button>().All(b => b.Text != ""))
            {
                ties++;
                statusLabel.Text = "It's a tie!";
                UpdateScore();
                return true;
            }
            return false;
        }

        private void UpdateScore()
        {
            scoreLabel.Text = $"Wins: {playerWins} | Losses: {computerWins} | Ties: {ties}";
        }
    }
}
