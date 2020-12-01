using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    public partial class GameManager : Form
    {
        int score = 100;

        TimeSpan MS_PER_FRAME;

        bool onLeaf = false, wasDelayed = false;

        MovableObj[] movableObjs = new MovableObj[35];
        List<PictureBox> collectibles = new List<PictureBox>();

        int[] speeds = new int[35];

        Frog frog;


        public GameManager()
        {
            InitializeComponent();
            frog = new Frog(this.ClientSize.Width, this.ClientSize.Height, player);
        }

        public void GameLoop()
        {
            MS_PER_FRAME = TimeSpan.FromMilliseconds(1.0 / 60.0 * 10000.0);
            Stopwatch stopWatch = Stopwatch.StartNew();
            TimeSpan previous = stopWatch.Elapsed;
            TimeSpan lag = new TimeSpan(0);
            while (true)
            {
                TimeSpan current = stopWatch.Elapsed;
                TimeSpan elapsed = current - previous;
                previous = current;
                lag += elapsed;
                
                while (lag >= MS_PER_FRAME)
                {
                    UpdateGameLogic();

                    lag -= MS_PER_FRAME;
                }
                
                Application.DoEvents();
            }
        }

        private void UpdateGameLogic()
        {
            frog.Move();

            foreach (MovableObj obj in movableObjs)
            {
                obj.Move();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            frog.Rotate(e);
            frog.MoveTheFrog(e);
        }

        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            ScoreTimer.Start();
            if (score > 1)
            {
                score--;
                txtTimer.Text = "Score: " + score.ToString();
            }
            else
            {
                RestartGame();
            }
        }

        private void RestartButton(object sender, EventArgs e)
        {
            RestartGame();
        }

        private async void MainGameTimerEvent(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                            RestartGame();
                    }

                    if ((string)x.Tag == "leaf")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (!onLeaf)
                            {
                                onLeaf = true;
                                wasDelayed = false;
                                Wait();
                            }
                        }
                        else
                        {
                            if (onLeaf)
                            {
                                if (wasDelayed)
                                {
                                    onLeaf = false;
                                }
                            }
                        }
                    }

                    if (!onLeaf)
                    {
                        if ((string)x.Tag == "water")
                        {
                            if (player.Bounds.IntersectsWith(x.Bounds))
                            {
                                RestartGame();
                            }
                        }
                    }


                    if ((string)x.Tag == "slowTime")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            this.Controls.Remove(x);
                            MS_PER_FRAME = TimeSpan.FromMilliseconds(2.0 / 60.0 * 10000.0);
                            await Task.Delay(4000);
                            MS_PER_FRAME = TimeSpan.FromMilliseconds(1.0 / 60.0 * 10000.0);
                        }
                    }

                    if ((string)x.Tag == "addToScore")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            this.Controls.Remove(x);
                            score += 5;
                        }
                    }

                    if (x.Name == "finishLine")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            ScoreTimer.Stop();
                            Spawner.Stop();

                            player.Visible = false;
                            winLabel.Visible = true;
                            restartButton.Visible = true;

                            winLabel.Text += "You won!" + Environment.NewLine + "Score: " + score;
                        }
                    }
                }
            }
        }

        private void Spawner_Tick(object sender, EventArgs e)
        {
            Spawner.Start();
            SpawnCollectible();
        }

        private void RestartGame()
        {
            winLabel.Visible = false;
            restartButton.Visible = false;
            player.Visible = true;

            player.Left = this.ClientSize.Width / 2;
            player.Top = this.ClientSize.Height - 33;

            score = 100;

            foreach (var c in collectibles)
            {
                this.Controls.Remove(c);
            }

            gameTimer.Interval = 20;
            Spawner.Start();
            ScoreTimer.Start();
            gameTimer.Start();
        }

        private async void SpawnCollectible()
        {
            Random random = new Random();

            int randomNum = random.Next(0, 2);

            PictureBox pictureBox = new PictureBox();

            if (randomNum == 0)
            {
                pictureBox.ImageLocation = "../../Resources/Fly.png";
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Tag = "slowTime";
            }
            else
            {
                pictureBox.ImageLocation = "../../Resources/Worm.png";
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Tag = "addToScore";
            }

            pictureBox.Location = new Point(random.Next(50, 400), random.Next(1, 9) * 53);
            pictureBox.Height = 30;
            pictureBox.Width = 20;

            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            collectibles.Add(pictureBox);

            await Task.Delay(7000);
            this.Controls.Remove(pictureBox);
        }

        private void SpawnMovableObjs(object sender, EventArgs e)
        {
            int i = 0;

            foreach (Control x in this.Controls)
            {
                if (x.Name.Contains("enemy") || x.Name.Contains("leaf"))
                {
                    if(i%2 == 0)
                        movableObjs[i] = new MovableObj(this.ClientSize.Width, (PictureBox)x, 1);
                    else
                        movableObjs[i] = new MovableObj(this.ClientSize.Width, (PictureBox)x, -1);

                    i++;
                }
            }
        }

        public async void Wait()
        {
            await Task.Delay(500);
            wasDelayed = true;
        }

    }
}
