using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DolphinGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int score;
        TextBox score_text_box;
        DispatcherTimer gameTimer = new DispatcherTimer();

        bool game_over;
        TextBox end_screen_textbox;
        Random rand;
        Rectangle background1;
        Rectangle background2;
        ImageBrush background1Sprite;
        ImageBrush background2Sprite;

        Player playerInstance;
        Rock rockInstance;
        Gem gemInstance;

        Rect playerHitBox;
        Rect rockHitBox;
        Rect gemHitBox;

        int scrolling_speed = 0;
        int falling_speed = 2;
        int vertical_swim_speed = 3;
        int horizontal_swim_speed = 4;
        int gem_speed = 6;

        public MainWindow()
        {
            InitializeComponent();
            setupGame();
            startGame();
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && game_over == true)
            {
                startGame();
            }

            if (e.Key == Key.Right)
            {
                playerInstance.moveRight = 0;
            }
            if (e.Key == Key.Left)
            {
                playerInstance.moveLeft = 0;
            }
            if (e.Key == Key.Down)
            {
                playerInstance.moveDown = 0;
            }
            if (e.Key == Key.Up)
            {
                playerInstance.moveUp = 0;
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                playerInstance.moveRight = playerInstance.moveLeft + 1;
            }
            if (e.Key == Key.Left)
            {
                playerInstance.moveLeft = playerInstance.moveRight + 1;
            }

            if (e.Key == Key.Down)
            {
                playerInstance.moveDown = playerInstance.moveUp + 1;
            }
            if (e.Key == Key.Up)
            {
                playerInstance.moveUp = playerInstance.moveDown + 1;
            }
        }

        private void setupGame()
        {
            gameTimer.Interval = TimeSpan.FromMilliseconds(10); //3
            gameTimer.Tick += gameEngine;

            rand = new Random();

            scrolling_speed = 1;

            // Canvas is Height="450" Width="800"
            background1 = new Rectangle();
            background1.Width = 800;
            background1.Height = 450;
            myCanvas.Children.Add(background1);


            background2 = new Rectangle();
            background2.Width = 800;
            background2.Height = 450;
            myCanvas.Children.Add(background2);

            playerInstance = new Player(75, 175);
            myCanvas.Children.Add(playerInstance.rectangle);

            rockInstance = new Rock(rand.Next(100, 300), rand.Next(100, 300));
            myCanvas.Children.Add(rockInstance.rectangle);

            gemInstance = new Gem(50, 50);
            myCanvas.Children.Add(gemInstance.rectangle);

            score_text_box = new TextBox();
            score_text_box.Background = Brushes.Aquamarine;
            score_text_box.IsReadOnly = true;
            score_text_box.Focusable = false;
            myCanvas.Children.Add(score_text_box);

            end_screen_textbox = new TextBox();
            end_screen_textbox.Background = Brushes.Aquamarine;
            end_screen_textbox.Width = 200;
            end_screen_textbox.BorderBrush = Brushes.Black;
            end_screen_textbox.BorderThickness = new Thickness(5, 5, 5, 5);
            end_screen_textbox.FontWeight = FontWeights.Bold;
            end_screen_textbox.FontSize = 20;
            end_screen_textbox.TextWrapping = TextWrapping.Wrap;
            end_screen_textbox.IsReadOnly = true;
            end_screen_textbox.Focusable = false;
            end_screen_textbox.Text = "Press ENTER to try again";
            myCanvas.Children.Add(end_screen_textbox);

            myCanvas.Focus();
        }

        private void startGame()
        {
            game_over = false;
            score = 0;

            Canvas.SetLeft(background1, 0);
            Canvas.SetTop(background1, 0);
            background1Sprite = new ImageBrush();
            background1Sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background_1.png"));
            background1.Fill = background1Sprite;

            Canvas.SetLeft(background2, background1.Width);
            Canvas.SetTop(background2, 0);
            background2Sprite = new ImageBrush();
            background2Sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background_2.png"));
            background2.Fill = background2Sprite;

            Canvas.SetLeft(playerInstance.rectangle, background1.Width / 2);
            Canvas.SetTop(playerInstance.rectangle, background1.Height / 2);
            playerInstance.rectangle.StrokeThickness = 0;

            resetPosition(rockInstance.rectangle);

            resetPosition(gemInstance.rectangle);

            Canvas.SetLeft(score_text_box, 0);
            Canvas.SetTop(score_text_box, 0);
            score_text_box.Text = "Score: 0";

            Canvas.SetLeft(end_screen_textbox, (background1.Width / 2) - (end_screen_textbox.Width / 2));
            Canvas.SetTop(end_screen_textbox, (background1.Height / 2) - 100);
            end_screen_textbox.Visibility = Visibility.Hidden;

            gameTimer.Start();
        }

        private void endGame()
        {
            game_over = true;
            playerInstance.rectangle.StrokeThickness = 1;
            end_screen_textbox.Visibility = Visibility.Visible;
        }

        private void gameEngine(object sender, EventArgs e)
        {
            if (game_over == false)
            {
                scrollBackground();

                handleKeyPresses();

                checkCollision();

                playerInstance.animate();
            }

        }
        private void scrollBackground()
        {
            if (Canvas.GetLeft(background1) <= -background1.Width)
            {
                // reposition on right
                Canvas.SetLeft(background1, background2.Width - scrolling_speed);
            }
            else
            {
                // keep scrolling left
                Canvas.SetLeft(background1, Canvas.GetLeft(background1) - scrolling_speed);
            }

            if (Canvas.GetLeft(background2) <= -background2.Width)
            {
                // reposition on right
                Canvas.SetLeft(background2, background1.Width - scrolling_speed);
            }
            else
            {
                // keep scrolling left
                Canvas.SetLeft(background2, Canvas.GetLeft(background2) - scrolling_speed);
            }

            if (Canvas.GetLeft(rockInstance.rectangle) <= -rockInstance.rectangle.Width)
            {
                // reposition on right
                rockInstance.rectangle.Height = rand.Next(100, 300);
                rockInstance.rectangle.Width = rand.Next(100, 300);
                resetPosition(rockInstance.rectangle);

            }
            else
            {
                // keep scrolling left
                Canvas.SetLeft(rockInstance.rectangle, Canvas.GetLeft(rockInstance.rectangle) - scrolling_speed);
            }


            if (Canvas.GetLeft(gemInstance.rectangle) <= -gemInstance.rectangle.Width)
            {
                // reposition on right
                resetPosition(gemInstance.rectangle);

            }
            else
            {
                // keep scrolling left
                Canvas.SetLeft(gemInstance.rectangle, Canvas.GetLeft(gemInstance.rectangle) - gem_speed);
            }

        }

        private void checkCollision()
        {
            playerHitBox = new Rect(Canvas.GetLeft(playerInstance.rectangle), Canvas.GetTop(playerInstance.rectangle), playerInstance.rectangle.Width, playerInstance.rectangle.Height);
            rockHitBox = new Rect(Canvas.GetLeft(rockInstance.rectangle), Canvas.GetTop(rockInstance.rectangle), rockInstance.rectangle.Width, rockInstance.rectangle.Height);
            gemHitBox = new Rect(Canvas.GetLeft(gemInstance.rectangle), Canvas.GetTop(gemInstance.rectangle), gemInstance.rectangle.Width, gemInstance.rectangle.Height);


            if (playerHitBox.IntersectsWith(rockHitBox))
            {
                endGame();
            }

            if (playerHitBox.IntersectsWith(gemHitBox))
            {
                // reposition on right
                resetPosition(gemInstance.rectangle);
                updateScore(1);
            }

        }
        private void updateScore(int amount)
        {
            score += amount;

            score_text_box.Text = "Score: " + score;
        }
        private void resetPosition(Rectangle rectangle)
        {
            Canvas.SetLeft(rectangle, background1.Width + rand.Next(0, 300));
            Canvas.SetTop(rectangle, rand.Next(0, (int)background1.Height - 100));
        }
        private void handleKeyPresses()
        {
            double currentTop = Canvas.GetTop(playerInstance.rectangle);
            double currentLeft = Canvas.GetLeft(playerInstance.rectangle);
            double newTop = currentTop;
            double newLeft = currentLeft;

            // UP AND DOWN
            if (playerInstance.moveUp > playerInstance.moveDown)
            {
                // swim up
                newTop = currentTop - vertical_swim_speed;
            }
            else if (playerInstance.moveDown > playerInstance.moveUp)
            {
                // swim down
                newTop = currentTop + vertical_swim_speed;
            }
            else
            {
                // no up or down keys pressed
                // fall down
                newTop = currentTop + falling_speed;
            }

            // LEFT AND RIGHT
            if (playerInstance.moveRight > playerInstance.moveLeft)
            {
                // go right
                newLeft = currentLeft + horizontal_swim_speed;
            }
            else if (playerInstance.moveLeft > playerInstance.moveRight)
            {
                // go left
                newLeft = currentLeft - horizontal_swim_speed;
            }
            else
            {
                // no left or right keys pressed
            }

            if (newTop >= 0 &&
                newTop < background1.Height - playerInstance.rectangle.Height)
            {
                Canvas.SetTop(playerInstance.rectangle, newTop);
            }

            if (newLeft >= 0 &&
                newLeft < background1.Width - playerInstance.rectangle.Width)
            {
                Canvas.SetLeft(playerInstance.rectangle, newLeft);
            }

        }

    }
}
