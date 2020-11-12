using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    public partial class Form1 : Form
    {
        bool goUp, goDown, goLeft, goRight, isGameOver;

        int speed = 7;

        int enemySpeed = 3;

        int score = 0;

        PictureBox[] enemies = new PictureBox[23];
        List<PictureBox> collectables = new List<PictureBox>();

        int[] speeds = new int[23];

        public Form1()
        {
            InitializeComponent();

            int i = 0;

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "enemy")
                {
                    enemies[i] = (PictureBox)x;
                    i++;
                }
            }

            for(int j = 0; j < speeds.Length; j++)
            {
                if(j%2 == 0)
                {
                    speeds[j] = enemySpeed;
                }
                else
                {
                    speeds[j] = -enemySpeed;
                }
            }
        }

        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            ScoreTimer.Start();
            score++;
            txtTimer.Text = "Time: " + score.ToString();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            if (goLeft)
            {
                player.Left -= speed;
            }
            if (goRight)
            {
                player.Left += speed;
            }
            if (goUp)
            {
                player.Top -= speed;
            }
            if (goDown)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "enemy")
                    {
                        if(player.Bounds.IntersectsWith(x.Bounds))
                            RestartGame();
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


            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Left -= speeds[i];

                if (enemies[i].Left < 0 || enemies[i].Left + enemies[i].Width > this.ClientSize.Width)
                {
                    speeds[i] = -speeds[i];
                }
            }
        }

        private void Spawner_Tick(object sender, EventArgs e)
        {
            Spawner.Start();
            SpawnCollectable();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }



        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Escape && isGameOver)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;

            winLabel.Visible = false;
            restartButton.Visible = false;
            player.Visible = true;

            player.Left = 220;
            player.Top = 570;

            score = 0;

            foreach(var c in collectables)
            {
                this.Controls.Remove(c);
            }
            
            Spawner.Start();
            ScoreTimer.Start();
            gameTimer.Start();
        }

        private void SpawnCollectable()
        {
            Random random = new Random();

            int randomNum = random.Next(0,2);

            PictureBox pictureBox = new PictureBox();

            if (randomNum == 0)
            {
                pictureBox.BackColor = Color.Black;
                pictureBox.Tag = "slowTime";
            }
            else
            {
                pictureBox.BackColor = Color.Blue;
                pictureBox.Tag = "decreaseScore";
            }

            pictureBox.Location = new Point(random.Next(50, 400), random.Next(2, 9) * 53);
            pictureBox.Height = 30;
            pictureBox.Width = 20;

            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            collectables.Add(pictureBox);
            
        }
    }
}
