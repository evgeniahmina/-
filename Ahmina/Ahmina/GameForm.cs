namespace Ahmina
{
    public partial class GameForm : Form
    {
        private int[,] gameBoard = new int[15, 15]; 
        private int currentPlayer = 1; 
        private int cellSize = 40;
        private PictureBox gameField;
        private Label statusLabel;
        private Button resetButton;

        public GameForm()
        {
            InitializeGameField(); 
        }

        private void InitializeGameField()
        {
           
            gameField = new PictureBox();
            gameField.Size = new Size(15 * cellSize, 15 * cellSize);
            gameField.Location = new Point(10, 50);
            gameField.BackColor = Color.White;
            gameField.BorderStyle = BorderStyle.FixedSingle;
            gameField.Paint += GameField_Paint;
            gameField.MouseClick += GameField_MouseClick;

           
            statusLabel = new Label();
            statusLabel.Location = new Point(10, 10);
            statusLabel.Size = new Size(200, 30);
            statusLabel.Text = "Ходят крестики (X)";
            statusLabel.Font = new Font("Arial", 12);

            
            resetButton = new Button();
            resetButton.Location = new Point(220, 10);
            resetButton.Size = new Size(100, 30);
            resetButton.Text = "Новая игра";
            resetButton.Click += ResetButton_Click;

            
            this.Controls.Add(gameField);
            this.Controls.Add(statusLabel);
            this.Controls.Add(resetButton);

           
            this.ClientSize = new Size(
                Math.Max(gameField.Width + 30, 350),
                gameField.Height + 100
            );
            this.Text = "Крестики-Нолики 15×15";
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
        }

        private void GameField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen gridPen = new Pen(Color.Black, 1);

           
            for (int i = 0; i <= 15; i++)
            {
                g.DrawLine(gridPen, i * cellSize, 0, i * cellSize, 15 * cellSize);
                g.DrawLine(gridPen, 0, i * cellSize, 15 * cellSize, i * cellSize);
            }

            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (gameBoard[row, col] == 1) 
                    {
                        DrawX(g, col, row, Color.Red);
                    }
                    else if (gameBoard[row, col] == 2) 
                    {
                        DrawO(g, col, row, Color.Blue);
                    }
                }
            }
        }
        private void DrawX(Graphics g, int col, int row, Color color)
        {
            Pen pen = new Pen(color, 3);
            int padding = 5;
            int x = col * cellSize;
            int y = row * cellSize;

            g.DrawLine(pen,
                x + padding, y + padding,
                x + cellSize - padding, y + cellSize - padding);
            g.DrawLine(pen,
                x + cellSize - padding, y + padding,
                x + padding, y + cellSize - padding);
        }

        private void DrawO(Graphics g, int col, int row, Color color)
        {
            Pen pen = new Pen(color, 3);
            int padding = 5;
            Rectangle rect = new Rectangle(
                col * cellSize + padding,
                row * cellSize + padding,
                cellSize - 2 * padding,
                cellSize - 2 * padding);

            g.DrawEllipse(pen, rect);
        }
        private void GameField_MouseClick(object sender, MouseEventArgs e)
        {
            int col = e.X / cellSize;
            int row = e.Y / cellSize;
            if (col >= 0 && col < 15 && row >= 0 && row < 15)
            {
                if (gameBoard[row, col] == 0)
                {
                    gameBoard[row, col] = currentPlayer;
                    if (CheckWin(row, col))
                    {
                        MessageBox.Show($"Победил игрок {(currentPlayer == 1 ? "X" : "O")}!",
                                        "Победа!",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        ResetGame();
                        return;
                    }
                    if (IsBoardFull())
                    {
                        MessageBox.Show("Ничья! Поле полностью заполнено.",
                                        "Ничья",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        ResetGame();
                        return;
                    }
                    currentPlayer = currentPlayer == 1 ? 2 : 1;
                    statusLabel.Text = $"Ходят {(currentPlayer == 1 ? "крестики (X)" : "нолики (O)")}";
                    gameField.Invalidate();
                }
            }
        }
        private bool CheckWin(int row, int col)
        {
            int player = gameBoard[row, col];
            if (player == 0) return false;
            int count = 1;
            for (int i = col - 1; i >= 0 && gameBoard[row, i] == player; i--) count++;
            for (int i = col + 1; i < 15 && gameBoard[row, i] == player; i++) count++;
            if (count >= 5) return true;
            count = 1;
            for (int i = row - 1; i >= 0 && gameBoard[i, col] == player; i--) count++;
            for (int i = row + 1; i < 15 && gameBoard[i, col] == player; i++) count++;
            if (count >= 5) return true;
            count = 1;
            for (int i = 1; row - i >= 0 && col - i >= 0 && gameBoard[row - i, col - i] == player; i++) count++;
            for (int i = 1; row + i < 15 && col + i < 15 && gameBoard[row + i, col + i] == player; i++) count++;
            if (count >= 5) return true;
            count = 1;
            for (int i = 1; row - i >= 0 && col + i < 15 && gameBoard[row - i, col + i] == player; i++) count++;
            for (int i = 1; row + i < 15 && col - i >= 0 && gameBoard[row + i, col - i] == player; i++) count++;
            if (count >= 5) return true;

            return false;
        }
        private bool IsBoardFull()
        {
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (gameBoard[row, col] == 0)
                        return false;
                }
            }
            return true;
        }
        private void ResetGame()
        {
            gameBoard = new int[15, 15];
            currentPlayer = 1;
            statusLabel.Text = "Ходят крестики (X)";
            gameField.Invalidate();
        }
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                ResetGame();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
