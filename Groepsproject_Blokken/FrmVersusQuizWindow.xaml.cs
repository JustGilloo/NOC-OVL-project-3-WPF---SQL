using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmVersusQuizWindow.xaml
    /// </summary>
    public partial class FrmVersusQuizWindow : Window
    {
        private readonly ImageSource[] arrTilesImages = new ImageSource[]
          {
        new BitmapImage(new Uri("Assets/Tetris/TileEmpty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileCyan.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileBlue.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileOrange.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileYellow.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileGreen.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TilePurple.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/TileRed.png", UriKind.Relative))
          };
        private readonly ImageSource[] arrBlockImages = new ImageSource[]
        {
        new BitmapImage(new Uri("Assets/Tetris/Block-Empty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-I.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-J.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-L.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-O.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-S.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-T.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Tetris/Block-Z.png", UriKind.Relative))
        };
        bool correctAnswerClicked = false;
        bool blockPlaced = true;
        bool buzzerPressed = false;
        bool buzzerPlayer1 = false;
        bool buzzerPlayer2 = false;
        bool questionQueue = true;
        private readonly Image[,] arrImageControls;
        private GameState gameState = new GameState();
        public Player ingelogdePlayer1;
        public Player ingelogdePlayer2;
        public PrimeWord gekozenPrimeword = new PrimeWord();
        //deze is voor in de game vensters
        char[] wordForDisplay = "________".ToCharArray(); //dit is wat we tonen op het scherm
        char[] versnipperdPrimeWord = new char[8]; //dit is het myPrimeWord.Primeword waar we mee gaan werken
                                                   //myPrimeword.Hint is je hint dat je kan tonen

        public MediaPlayer buzzerSound = new MediaPlayer();
        public MediaPlayer backgroundMusicPlayer = new MediaPlayer();
        public MediaPlayer audioCue = new MediaPlayer();
        public MediaPlayer blokkenTick = new MediaPlayer();

        private BrushConverter bc = new BrushConverter();
        private List<Question> tempLijstVragen = new List<Question>();
        public List<Question> finalLijstVragen = new List<Question>();
        public List<String> geluidjes = new List<String>();
        private Random random = new Random();
        private Question nieuweVraag = new Question();
        private GameLogSP eenGame = new GameLogSP();
        private System.Timers.Timer _delayTimer;
        private int tellerTimer = 120;
        private DispatcherTimer timer;
        public List<string> gekozenVragenLijsten = new List<string>();
        string json = "";
        public Player ingelogdePlayerMainWindow = new Player();
        int teller = 0;
        public FrmVersusQuizWindow()
        {
            InitializeComponent();
            arrImageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LijstMetGeluidjesMaken();
            InlezenVragen();
            ProfilePicturesInladen();
            RandomQuestionPicker();
            EnableDisableAnswers();
            //TODO: profielfotos inladen spelers
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        public void ProfilePicturesInladen()
        {
            ingelogdePlayer1.ImageInladenMetMemoryStream();
            ingelogdePlayer2.ImageInladenMetMemoryStream();
            imgSpeler1.Source = ingelogdePlayer1.BMP;
            imgSpeler2.Source = ingelogdePlayer2.BMP;
            imgSpeler3.Source = ingelogdePlayer1.BMP;
            imgSpeler4.Source = ingelogdePlayer2.BMP;
        }
        public void InlezenVragen()
        {
            foreach (string path in gekozenVragenLijsten)
            {
                using (StreamReader r = new StreamReader("../../Questionaires/" + path))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    tempLijstVragen.Clear();
                    json = r.ReadToEnd(); // Tekst inlezen in een string
                    tempLijstVragen = JsonSerializer.Deserialize<List<Question>>(json);

                    foreach (Question aQuestion in tempLijstVragen)
                    {
                        finalLijstVragen.Add(aQuestion);
                    }
                }
            }
        }
        private void RandomQuestionPicker()
        {
            int randomQuestionIndex = random.Next(0, finalLijstVragen.Count);
            nieuweVraag = finalLijstVragen[randomQuestionIndex];
            finalLijstVragen.RemoveAt(randomQuestionIndex);
            Vraag.Text = nieuweVraag.TheQuestion;
            Vraag2.Text = nieuweVraag.TheQuestion;
            nieuweVraag.InsertAnswersInButtons(btnAntwoord1, btnAntwoord2, btnAntwoord3, btnAntwoord4);
            btnAntwoord1.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord2.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord3.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord4.Background = (Brush)bc.ConvertFrom("#fea702");
            teller++;
            PlaybackSound();
            
            geluidjes.RemoveAt(0);
        }
        public void ShowCorrectAnswer(List<Button> lijstButtons)
        {
            foreach (Button button in lijstButtons)
            {
                if ((string)button.Content == nieuweVraag.CorrectAnswer)
                {
                    button.Background = Brushes.Green;
                }
            }
        }
        private void CheckAnswer(Button button)
        {
            if ((string)button.Content == nieuweVraag.CorrectAnswer)
            {
                if (buzzerPlayer1 == true)
                {
                    gameState.ScorePlayerOne += 50;
                }
                if (buzzerPlayer2 == true)
                {
                    gameState.ScorePlayerTwo += 50;
                }
                button.Background = Brushes.Green;
                button.BorderThickness = new Thickness(0);
                lblScoreSpeler1.Content = gameState.ScorePlayerOne.ToString();
                lblScoreSpeler2.Content = gameState.ScorePlayerTwo.ToString();
                correctAnswerClicked = true;
                gameState.BlockIsPlaced = false;
                //txtScore.Text = (Convert.ToInt32(txtScore.Text) + 50).ToString();
            }
            else
            {
                if (buzzerPlayer1 == true)
                {
                    gameState.ScorePlayerOne -= 50;
                }
                if (buzzerPlayer2 == true)
                {
                    gameState.ScorePlayerTwo -= 50;
                }
                button.Background = Brushes.OrangeRed;
                button.BorderThickness = new Thickness(0);
                lblScoreSpeler1.Content =  gameState.ScorePlayerOne.ToString();
                lblScoreSpeler2.Content =  gameState.ScorePlayerTwo.ToString();

                ShowCorrectAnswer(new List<Button> { btnAntwoord1, btnAntwoord2, btnAntwoord3, btnAntwoord4 });
            }
            Overlay.Visibility = Visibility.Collapsed;
        }

        public void EnableDisableAnswers()
        {
            switch (buzzerPressed)
            {
                case (true):
                    btnAntwoord1.IsEnabled = true;
                    btnAntwoord2.IsEnabled = true;
                    btnAntwoord3.IsEnabled = true;
                    btnAntwoord4.IsEnabled = true;
                    break;
                case (false):
                    btnAntwoord1.IsEnabled = false;
                    btnAntwoord2.IsEnabled = false;
                    btnAntwoord3.IsEnabled = false;
                    btnAntwoord4.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }
        private void ClickEvent()
        {
            btnAntwoord1.MouseEnter -= btnAntwoord1_MouseEnter;
            btnAntwoord1.MouseLeave -= btnAntwoord1_MouseLeave;
            btnAntwoord1.Click -= btnAntwoord1_Click;
            btnAntwoord2.MouseEnter -= btnAntwoord2_MouseEnter;
            btnAntwoord2.MouseLeave -= btnAntwoord2_MouseLeave;
            btnAntwoord2.Click -= btnAntwoord2_Click;
            btnAntwoord3.MouseEnter -= btnAntwoord3_MouseEnter;
            btnAntwoord3.MouseLeave -= btnAntwoord3_MouseLeave;
            btnAntwoord3.Click -= btnAntwoord3_Click;
            btnAntwoord4.MouseEnter -= btnAntwoord4_MouseEnter;
            btnAntwoord4.MouseLeave -= btnAntwoord4_MouseLeave;
            btnAntwoord4.Click -= btnAntwoord4_Click;
        }
        private void btnAntwoord1_Click(object sender, RoutedEventArgs e)
        {
            ClickEvent();
            CheckAnswer(btnAntwoord1);
            Delay();
        }

        private void btnAntwoord2_Click(object sender, RoutedEventArgs e)
        {
            ClickEvent();
            CheckAnswer(btnAntwoord2);
            Delay();
        }

        private void btnAntwoord3_Click(object sender, RoutedEventArgs e)
        {
            ClickEvent();
            CheckAnswer(btnAntwoord3);
            Delay();
        }

        private void btnAntwoord4_Click(object sender, RoutedEventArgs e)
        {
            ClickEvent();
            CheckAnswer(btnAntwoord4);
            Delay();
        }

        private void btnAntwoord1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord1.Background = System.Windows.Media.Brushes.WhiteSmoke;
            btnAntwoord1.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord1.BorderThickness = new Thickness(5);
        }

        private void btnAntwoord1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord1.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord1.BorderThickness = new Thickness(0);
        }

        private void btnAntwoord2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord2.Background = System.Windows.Media.Brushes.WhiteSmoke;
            btnAntwoord2.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord2.BorderThickness = new Thickness(5);
        }

        private void btnAntwoord2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord2.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord2.BorderThickness = new Thickness(0);
        }

        private void btnAntwoord3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord3.Background = System.Windows.Media.Brushes.WhiteSmoke;
            btnAntwoord3.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord3.BorderThickness = new Thickness(5);
        }

        private void btnAntwoord3_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord3.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord3.BorderThickness = new Thickness(0);
        }

        private void btnAntwoord4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord4.Background = System.Windows.Media.Brushes.WhiteSmoke;
            btnAntwoord4.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord4.BorderThickness = new Thickness(5);
        }

        private void btnAntwoord4_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnAntwoord4.Background = (Brush)bc.ConvertFrom("#fea702");
            btnAntwoord4.BorderThickness = new Thickness(0);
        }

        private async void Delay()
        {
            await GameLoop();
            _delayTimer = new System.Timers.Timer(100); // 5 seconds delay
            _delayTimer.Elapsed += (s, args) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    if (gameState.BlockIsPlaced == true)
                    {
                        correctAnswerClicked = false;
                        buzzerPressed = false;
                        buzzerPlayer1 = false;
                        buzzerPlayer2 = false;
                        brdImgSpeler1.BorderBrush = null;
                        brdImgSpeler2.BorderBrush = null;
                        brdImgSpeler3.BorderBrush = null;
                        brdImgSpeler4.BorderBrush = null;
                        brdImgSpeler1.BorderThickness = new Thickness(0);
                        brdImgSpeler2.BorderThickness = new Thickness(0);
                        brdImgSpeler3.BorderThickness = new Thickness(0);
                        brdImgSpeler4.BorderThickness = new Thickness(0);
                        EnableDisableAnswers();
                        questionQueue = true;
                        RandomQuestionPicker();
                        btnAntwoord1.MouseEnter += btnAntwoord1_MouseEnter;
                        btnAntwoord1.MouseLeave += btnAntwoord1_MouseLeave;
                        btnAntwoord1.Click += btnAntwoord1_Click;
                        btnAntwoord2.MouseEnter += btnAntwoord2_MouseEnter;
                        btnAntwoord2.MouseLeave += btnAntwoord2_MouseLeave;
                        btnAntwoord2.Click += btnAntwoord2_Click;
                        btnAntwoord3.MouseEnter += btnAntwoord3_MouseEnter;
                        btnAntwoord3.MouseLeave += btnAntwoord3_MouseLeave;
                        btnAntwoord3.Click += btnAntwoord3_Click;
                        btnAntwoord4.MouseEnter += btnAntwoord4_MouseEnter;
                        btnAntwoord4.MouseLeave += btnAntwoord4_MouseLeave;
                        btnAntwoord4.Click += btnAntwoord4_Click;
                    }
                    await GameLoop();
                });
            };
            _delayTimer.AutoReset = false;
            _delayTimer.Start();
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            questionQueue = false;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            tellerTimer--;
            lblScoreSpeler1.Content = gameState.ScorePlayerOne.ToString();
            lblScoreSpeler2.Content = gameState.ScorePlayerTwo.ToString();
            if (teller == 10 && blockPlaced == true)
            {
                timer.Stop();
                backgroundMusicPlayer.Stop();
                grdGameOver.Visibility = System.Windows.Visibility.Visible;
                GameLogVS gespeeldeGame = new GameLogVS();
                gespeeldeGame.Player1Name = ingelogdePlayer1.Name;
                gespeeldeGame.Player2Name = ingelogdePlayer2.Name;
                gespeeldeGame.Player1Score = gameState.ScorePlayerOne;
                gespeeldeGame.Player2Score = gameState.ScorePlayerTwo;
                //Speler 1 wint
                if (gameState.ScorePlayerOne > gameState.ScorePlayerTwo)
                {

                    gespeeldeGame.Winner = ingelogdePlayer1.Name;
                    txtFinalScore.Text = "De winnaar is " + ingelogdePlayer1 + " met een score van " + gameState.ScorePlayerOne + "!";
                    if (ingelogdePlayer1.VSGamesPlayed == null)
                    {
                        ingelogdePlayer1.VSGamesWon = 1;
                        ingelogdePlayer1.VSGamesPlayed = 1;
                    }
                    else
                    {
                        ingelogdePlayer1.VSGamesWon++;
                        ingelogdePlayer1.VSGamesPlayed++;
                    }
                    if (ingelogdePlayer2.VSGamesPlayed == null)
                    {
                        ingelogdePlayer2.VSGamesPlayed = 1;
                    }
                    else
                    {
                        ingelogdePlayer2.VSGamesPlayed++;
                    }

                }
                //Gelijkspel.
                else if (gameState.ScorePlayerOne == gameState.ScorePlayerTwo)
                {
                    gespeeldeGame.Winner = ingelogdePlayer1.Name + " & " + ingelogdePlayer2.Name;
                    txtFinalScore.Text = "Gelijkspel! Jullie zijn beide winnaar met een score van " + gameState.ScorePlayerTwo + "!";
                    if (ingelogdePlayer1.VSGamesPlayed == null)
                    {
                        ingelogdePlayer1.VSGamesWon = 1;
                        ingelogdePlayer1.VSGamesPlayed = 1;
                    }
                    else
                    {
                        ingelogdePlayer1.VSGamesWon++;
                        ingelogdePlayer1.VSGamesPlayed++;
                    }
                    if (ingelogdePlayer2.VSGamesPlayed == null)
                    {
                        ingelogdePlayer2.VSGamesWon = 1;
                        ingelogdePlayer2.VSGamesPlayed = 1;
                    }
                    else
                    {
                        ingelogdePlayer2.VSGamesWon++;
                        ingelogdePlayer2.VSGamesPlayed++;
                    }

                }
                //Speler2 wint
                else
                {
                    gespeeldeGame.Winner = ingelogdePlayer2.Name;
                    txtFinalScore.Text = "De winnaar is " + ingelogdePlayer2 + " met een score van " + gameState.ScorePlayerTwo + "!";
                    if (ingelogdePlayer1.VSGamesPlayed == null)
                    {
                        ingelogdePlayer1.VSGamesPlayed = 1;
                    }
                    else
                    {
                        ingelogdePlayer1.VSGamesPlayed++;
                    }
                    if (ingelogdePlayer2.VSGamesPlayed == null)
                    {
                        ingelogdePlayer2.VSGamesPlayed = 1;
                        ingelogdePlayer2.VSGamesWon = 1;
                    }
                    else
                    {
                        ingelogdePlayer2.VSGamesPlayed++;
                        ingelogdePlayer2.VSGamesWon++;
                    }
                }
                //HighscoreVScheck
                if (ingelogdePlayer1.VSHighscore == null || ingelogdePlayer1.VSHighscore < gameState.ScorePlayerOne)
                {
                    ingelogdePlayer1.VSHighscore = gameState.ScorePlayerOne;
                }
                if (ingelogdePlayer2.VSHighscore == null || ingelogdePlayer2.VSHighscore < gameState.ScorePlayerTwo)
                {
                    ingelogdePlayer2.VSHighscore = gameState.ScorePlayerTwo;
                }

                //Wegschrijven/updaten
                DataManager.InsertGameLogVS(gespeeldeGame);
                DataManager.UpdatePlayer(ingelogdePlayer2);
                DataManager.UpdatePlayer(ingelogdePlayer1);
            }
        }
        private Image[,] SetupGameCanvas(GameGrid gameGrid)
        {
            Image[,] imageControls = new Image[gameGrid.Rows, gameGrid.Columns];
            double cellsize = 55;

            for (int row = 0; row < gameGrid.Rows; row++)
            {
                for (int column = 0; column < gameGrid.Columns; column++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellsize,
                        Height = cellsize
                    };
                    Canvas.SetTop(imageControl, (row - 2) * cellsize + 10);
                    Canvas.SetLeft(imageControl, column * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[row, column] = imageControl;
                }
            }
            gameState.RowIsCleared = false;
            gameState.BlockIsPlaced = true;
            return imageControls;
        }
        private void DrawGrid(GameGrid gameGrid)
        {
            for (int row = 0; row < gameGrid.Rows; ++row)
            {
                for (int column = 0; column < gameGrid.Columns; column++)
                {
                    int id = gameGrid[row, column];
                    arrImageControls[row, column].Opacity = 1;
                    arrImageControls[row, column].Source = arrTilesImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position position in block.TilePositions())
            {
                arrImageControls[position.Row, position.Column].Opacity = 1;
                arrImageControls[position.Row, position.Column].Source = arrTilesImages[block.ID];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block nextBlock = blockQueue.NextBlock;
        }
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
            Draw(gameState);
        }

        private async Task GameLoop()
        {
            string soundeffectFilePath = "../../Assets/Sounds/Blokken ValgeluidjeSeconde.wav";
            Draw(gameState);
            while (!gameState.GameOver && correctAnswerClicked == true && gameState.BlockIsPlaced == false)
            {
                gameState.BlockIsPlaced = false;
                int delay = 950;
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                if (!string.IsNullOrEmpty(soundeffectFilePath))
                {
                    blokkenTick.Open(new Uri(soundeffectFilePath, UriKind.Relative));
                    blokkenTick.Volume = 0.5;
                    blokkenTick.Play();
                }
                if (gameState.RowIsCleared == true)
                {
                    if (buzzerPlayer1 == true)
                    {
                        gameState.ScorePlayerOne += gameState.Score;
                    }
                    else if (buzzerPlayer2 == true)
                    {
                        gameState.ScorePlayerTwo += gameState.Score;
                    }
                    gameState.Score = 0;
                }
                Draw(gameState);
            }
            //if (gameState.GameOver)
            //{
            //    grdGameOver.Visibility = Visibility.Visible;
            //    txtFinalScore.Text = gameState.Score.ToString();
            //}
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string buzzerSoundPath = "../../Assets/Sounds/Blokken Afdruk.wav";
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Q:
                    if (correctAnswerClicked == true && buzzerPlayer1 == true && teller != 10)
                    {
                        gameState.MoveBlockLeft();
                    }
                    break;
                case Key.D:
                    if (correctAnswerClicked == true && buzzerPlayer1 == true && teller != 10)
                    {
                        gameState.MoveBlockRight();
                    }
                    break;
                case Key.X:
                    if (correctAnswerClicked == true && buzzerPlayer1 == true && teller != 10)
                    {
                        gameState.MoveBlockDown();
                    }
                    break;
                case Key.A:
                    if (gameState.BlockIsPlaced == true && buzzerPlayer1 == true && teller != 10)
                    {
                        gameState.RotateBlockCounterClockwise();
                    }
                    break;
                case Key.E:
                    if (correctAnswerClicked == true && buzzerPlayer1 == true && teller != 10)
                    {
                        gameState.RotateBlockClockWise();
                    }
                    break;
                case Key.Z:
                    if (buzzerPressed == false && questionQueue == false && teller != 10)
                    {
                        buzzerPressed = true;
                        buzzerPlayer1 = true;
                        Overlay.Visibility = Visibility.Visible;
                        brdImgSpeler1.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
                        brdImgSpeler1.BorderThickness = new Thickness(5);
                        brdImgSpeler3.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
                        brdImgSpeler3.BorderThickness = new Thickness(5);
                        brdImgSpeler3.Visibility = Visibility.Visible;
                        brdImgSpeler4.Visibility = Visibility.Hidden;
                        if (!string.IsNullOrEmpty(buzzerSoundPath))
                        {
                            buzzerSound.Open(new Uri(buzzerSoundPath, UriKind.Relative));
                            buzzerSound.Volume = 0.5;
                            buzzerSound.Play();
                        }
                        EnableDisableAnswers();
                    }
                    break;
                case Key.NumPad4:
                    if (correctAnswerClicked == true && buzzerPlayer2 == true && teller != 10)
                    {
                        gameState.MoveBlockLeft();
                    }
                    break;
                case Key.NumPad6:
                    if (correctAnswerClicked == true && buzzerPlayer2 == true && teller != 10)
                    {
                        gameState.MoveBlockRight();
                    }
                    break;
                case Key.NumPad2:
                    if (correctAnswerClicked == true && buzzerPlayer2 == true && teller != 10)
                    {
                        gameState.MoveBlockDown();
                    }
                    break;
                case Key.NumPad7:
                    if (correctAnswerClicked == true && buzzerPlayer2 == true && teller != 10)
                    {
                        gameState.RotateBlockCounterClockwise();
                    }
                    break;
                case Key.NumPad9:
                    if (correctAnswerClicked == true && buzzerPlayer2 == true && teller != 10)
                    {
                        gameState.RotateBlockClockWise();
                    }
                    break;
                case Key.NumPad8:
                    if (buzzerPressed == false && questionQueue == false && teller != 10)
                    {
                        buzzerPressed = true;
                        buzzerPlayer2 = true;
                        Overlay.Visibility = Visibility.Visible;
                        brdImgSpeler2.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
                        brdImgSpeler2.BorderThickness = new Thickness(5);
                        brdImgSpeler4.BorderBrush = (Brush)bc.ConvertFrom("#fea702");
                        brdImgSpeler4.BorderThickness = new Thickness(5);
                        brdImgSpeler4.Visibility = Visibility.Visible;
                        brdImgSpeler3.Visibility = Visibility.Hidden;
                        if (!string.IsNullOrEmpty(buzzerSoundPath))
                        {
                            buzzerSound.Open(new Uri(buzzerSoundPath, UriKind.Relative));
                            buzzerSound.Volume = 0.5;
                            buzzerSound.Play();
                        }
                        EnableDisableAnswers();
                    }
                    break;
                case Key.Tab:
                    //gameState.HoldBlock();
                    break;
                case Key.Space:
                    if (correctAnswerClicked == false)
                    {
                        //gameState.DropBlock();
                    }
                    break;
                case Key.P:
                    //gameState.Pause = true;
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.ingelogdePlayerLoginscreen = ingelogdePlayer1;
            this.Close();
            mainwindow.ShowDialog();
        }

        public void PlaybackSound()
        {
            string audioCue = geluidjes[0];
            if (!string.IsNullOrEmpty(audioCue))
            {
                this.audioCue.Open(new Uri(audioCue, UriKind.Relative));


                this.audioCue.Play();
                this.audioCue.MediaEnded += new EventHandler(Media_Ended);
            }
        }

        public void LijstMetGeluidjesMaken()
        {
            geluidjes.Add("../../Assets/Sounds/Mario Kart Race Start.wav");
            geluidjes.Add("../../Assets/Sounds/Ben Crabbe kan er niet meer tegen.wav");
            geluidjes.Add("../../Assets/Sounds/Boma.wav");
            geluidjes.Add("../../Assets/Sounds/Friends.wav");
            geluidjes.Add("../../Assets/Sounds/Kelder Gert.wav");
            geluidjes.Add("../../Assets/Sounds/Ok Let's Go.wav");
            geluidjes.Add("../../Assets/Sounds/Plop.wav");            
            geluidjes.Add("../../Assets/Sounds/Ready for Eddy.wav");
            geluidjes.Add("../../Assets/Sounds/Windows 95.wav");
            geluidjes.Add("../../Assets/Sounds/Air Horn.wav");
            geluidjes.Add("../../Assets/Sounds/Random.wav");

        }

    }
}
