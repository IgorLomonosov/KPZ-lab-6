﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisLibrary;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        public static MainWindow Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindow();
                }
                return instance;
            }
        }
       
        public List<User> userList = new List<User>();
        public string filePath = "..\\..\\..\\..\\ScoreTable.txt";
        public List<User> updatedUserList = new List<User>();
        public Options option;
        public User users;
        public int delay;
        Menu menu = new Menu();
        private readonly ImageSource[] tileImages1 = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };
        private readonly ImageSource[] tileImages2 = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed1.png", UriKind.Relative))
        };
        private readonly ImageSource[] blockImages1 = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
        };
        private readonly ImageSource[] blockImages2 = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T(1).png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z(1).png", UriKind.Relative)),
        };
        private readonly Image[,] imageControls;
        private readonly int maxDelay = 500;
        private readonly int minDelay = 100;
        private readonly int delayDecrease = 2;
        private readonly string backgroundImagePath = ".\\Assets\\tetris_background.png";
        private readonly string backgroundImage2Path = ".\\Assets\\tetris_background2.png";
        private readonly string scoreLabelTextEn = "Score:";
        private readonly string scoreLabelTextUa = "Рахунок:";
        private readonly string nextButtonTextEn = "Next";
        private readonly string nextButtonTextUa = "Наступна";
        private readonly string holdButtonTextEn = "Hold";
        private readonly string holdButtonTextUa = "Утримувати";
        private readonly string finalScoreLabelTextEn = "Score:";
        private readonly string finalScoreLabelTextUa = "Рахунок:";
        private readonly string playAgainButtonTextEn = "Play Again";
        private readonly string playAgainButtonTextUa = "Спробувати ще раз";
        private readonly string highScoreTableButtonTextEn = "High Score Table";
        private readonly string highScoreTableButtonTextUa = "Рекордна Таблиця";
        private readonly string returnToMenuButtonTextEn = "Return To Menu";
        private readonly string returnToMenuButtonTextUa = "Повернення до меню";
        private GameState gameState = new GameState();
        
        private void ChangeFormBackgroundImage()
        {
            string imagePath = backgroundImagePath;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath));
            this.Background = imageBrush;
        }
        private void ChangeFormBackgroundImage2()
        {
            string imagePath1 = backgroundImage2Path;
            ImageBrush imageBrush1 = new ImageBrush();
            imageBrush1.ImageSource = new BitmapImage(new Uri(imagePath1));
            this.Background = imageBrush1;
        }
        private void InitializeGame()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }
        private MainWindow()
        {
            instance = this; 
            InitializeGame();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }
        public static MainWindow GetInstance()
        {
            if (instance == null)
            {
                instance = new MainWindow();
            }
            return instance;
        }
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }
        private void DrawGrid(GameGrid grid)
        {
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r,c].Opacity = 1;
                    if(option.Theme == 0)
                    {
                      imageControls[r, c].Source = tileImages1[id];
                    }
                    if(option.Theme == 1)
                    {
                      imageControls[r, c].Source = tileImages2[id];
                    }

                }
            }
        }
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                if (option.Theme == 0)
                {
                    imageControls[p.Row, p.Column].Source = tileImages1[block.Id];
                }
                if (option.Theme == 1)
                {
                    imageControls[p.Row, p.Column].Source = tileImages2[block.Id];
                }
            }
        }
        private Block CreateNewBlock()
        {
            BlockType type = (BlockType)new Random().Next(0, 7);
            return Block.CreateBlock(type);
        }
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            if(option.Theme == 0)
            {
                NextImage.Source = blockImages1[next.Id];
            }
            if (option.Theme == 1)
            {
                NextImage.Source = blockImages2[next.Id];
            }
        }
        private void DrawHeldBlock(Block heldBlock) 
        {
            if(heldBlock == null)
            {
                if(option.Theme == 0)
                {
                    HoldImage.Source = blockImages1[0];
                }
                if(option.Theme == 1)
                {
                    HoldImage.Source = blockImages2[0];
                }
            }
            else
            {
                if(option.Theme == 0)
                {
                    HoldImage.Source = blockImages1[heldBlock.Id];
                }
                if(option.Theme == 1)
                {
                    HoldImage.Source = blockImages2[heldBlock.Id];
                }
            }
        }
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                if(option.Theme == 0)
                {
                    imageControls[p.Row + dropDistance, p.Column].Source = tileImages1[block.Id];
                }
                if(option.Theme == 1)
                {
                    imageControls[p.Row + dropDistance, p.Column].Source = tileImages2[block.Id];
                }
            }
        }
        private void Draw(GameState gameState)
        {
            if(option.Ghost_Block == 0)
            {
                DrawGrid(gameState.GameGrid);
                DrawGhostBlock(gameState.CurrentBlock);
                DrawBlock(gameState.CurrentBlock);
                DrawNextBlock(gameState.BlockQueue);
                DrawHeldBlock(gameState.HeldBlock);
                if (option.Language == 0)
                {
                    ScoreText.Text = $"{scoreLabelTextEn} {gameState.Score}";
                }
                if (option.Language == 1)
                {
                    ScoreText.Text = $"{scoreLabelTextUa} {gameState.Score}";
                }
            }
            if(option.Ghost_Block == 1)
            {
                DrawGrid(gameState.GameGrid);
                DrawBlock(gameState.CurrentBlock);
                DrawNextBlock(gameState.BlockQueue);
                DrawHeldBlock(gameState.HeldBlock);
                if (option.Language == 0)
                {
                    ScoreText.Text = $"{scoreLabelTextEn} {gameState.Score}";
                }
                if (option.Language == 1)
                {
                    ScoreText.Text = $"{scoreLabelTextUa} {gameState.Score}";
                }
            }
           
        }
        private async Task GameLoop()
        {
            gameState.CurrentBlock = CreateNewBlock();
            Draw(gameState);

            while (!gameState.GameOver)
            {
                delay = CalculateDelay(option.Difficult, gameState.Score);
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            UpdateFinalScoreText(option.Language, gameState.Score);

            var difficulty = (Difficulty)option.Difficult;
            users = UpdateUserScores(users, difficulty, gameState.Score);

            SaveUserScores(filePath, userList, users);
            GameOver.Text = GetGameOverText(option.Language);
        }

        private int CalculateDelay(int difficulty, int score)
        {
            int baseDelay = 1000;
            int minDelay = 100;
            double delayDecrease = 0.1;

            return difficulty switch
            {
                0 => Math.Max(minDelay, baseDelay - (score / 3)),
                1 => Math.Max(minDelay, baseDelay - score),
                2 => Math.Max(minDelay, baseDelay - (int)(score * delayDecrease)),
                _ => baseDelay,
            };
        }

        private void UpdateFinalScoreText(int language, int score)
        {
            if (language == 0)
            {
                FinalScoreText.Text = $"{scoreLabelTextEn} {score}";
            }
            else if (language == 1)
            {
                FinalScoreText.Text = $"{scoreLabelTextUa} {score}";
            }
        }

        private User UpdateUserScores(User user, Difficulty difficulty, int score)
        {
            bool userFound = false;

            foreach (var existingUser in userList)
            {
                if (existingUser.Name == user.Name)
                {
                    switch (difficulty)
                    {
                        case Difficulty.Easy:
                            existingUser.Score_Easy = Math.Max(existingUser.Score_Easy, score);
                            break;
                        case Difficulty.Normal:
                            existingUser.Score_Normal = Math.Max(existingUser.Score_Normal, score);
                            break;
                        case Difficulty.Hard:
                            existingUser.Score_Hard = Math.Max(existingUser.Score_Hard, score);
                            break;
                    }

                    userFound = true;
                }

                updatedUserList.Add(existingUser);
            }

            if (!userFound)
            {
                switch (difficulty)
                {
                    case Difficulty.Easy:
                        user.Score_Easy = score;
                        break;
                    case Difficulty.Normal:
                        user.Score_Normal = score;
                        break;
                    case Difficulty.Hard:
                        user.Score_Hard = score;
                        break;
                }

                updatedUserList.Add(user);
            }

            return user;
        }

        private void SaveUserScores(string filePath, List<User> userList, User updatedUser)
        {
            using (var sw = new StreamWriter(filePath, false))
            {
                foreach (var user in updatedUserList)
                {
                    string line = $"{user.Name},{user.Score_Easy},{user.Score_Normal},{user.Score_Hard}";
                    sw.WriteLine(line);
                }
            }
        }

        private string GetGameOverText(int language)
        {
            return language switch
            {
                0 => "Game Over",
                1 => "Ви програли!",
                _ => "Game Over"
            };
        }

        private enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            menu.settings = option;
            menu.Show();
            this.Close();
        }

        private void HighScoreTable_Click(object sender, RoutedEventArgs e)
        {
            HighScoreTablee highScoreTablee = new HighScoreTablee();
            highScoreTablee.userlist = updatedUserList;
            highScoreTablee.userss = users;
            highScoreTablee.optionss = option;
            highScoreTablee.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string name = parts[0];
                        int score_easy = Convert.ToInt32(parts[1]);
                        int score_normal = Convert.ToInt32(parts[2]);
                        int score_hard = Convert.ToInt32(parts[3]);
                        User user = new User {Name = name, Score_Easy = score_easy, Score_Normal = score_normal , Score_Hard = score_hard};
                        userList.Add(user);
                    }
                }
                sr.Close();
            }
            if (option.Theme == 0)
            {
                Uri imageUri = new Uri(backgroundImagePath);
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(imageUri));
                this.Background = imageBrush;
            }
            if(option.Theme == 1)
            {
                Uri imageUri = new Uri(backgroundImage2Path);
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(imageUri));
                this.Background = imageBrush;
            }
            if(option.Language == 0)
            {
                ScoreText.Text = scoreLabelTextEn;
                Next.Text = nextButtonTextEn;
                Hold.Text = holdButtonTextEn;
                FinalScoreText.Text = finalScoreLabelTextEn;
                PlayAgain.Content = playAgainButtonTextEn;
                HighScoreTable.Content = highScoreTableButtonTextEn;
                ReturnToMenu.Content = returnToMenuButtonTextEn;
            }
            if(option.Language == 1)
            {
                ScoreText.Text = scoreLabelTextUa;
                Next.Text = nextButtonTextUa;
                Hold.Text = holdButtonTextUa;
                FinalScoreText.Text = finalScoreLabelTextUa;
                PlayAgain.Content = playAgainButtonTextUa;
                HighScoreTable.Content = highScoreTableButtonTextUa;
                ReturnToMenu.Content = returnToMenuButtonTextUa;
            }
        }
    }
}
