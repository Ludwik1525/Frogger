﻿using System;
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
        int leafSpeed = 2;

        int score = 100;

        bool onLeaf = false, wasDelayed = false;

        string rotation = "up";

        PictureBox[] movableObjs = new PictureBox[35];
        List<PictureBox> collectables = new List<PictureBox>();

        int[] speeds = new int[35];

        public Form1()
        {
            InitializeComponent();

            int i = 0;

            foreach (Control x in this.Controls)
            {
                if (x.Name.Contains("enemy") || x.Name.Contains("leaf"))
                {
                    movableObjs[i] = (PictureBox)x;
                    i++;
                }
            }

            for(int j = 0; j < speeds.Length; j++)
            {
                if(j%2 == 0)
                {
                    if ((string)movableObjs[j].Tag == "enemy")
                    {
                        speeds[j] = enemySpeed;
                    }
                    else
                    {
                        speeds[j] = leafSpeed;
                    }
                    movableObjs[j].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    movableObjs[j].Refresh();
                }
                else
                {
                    if ((string)movableObjs[j].Tag == "enemy")
                    {
                        speeds[j] = -enemySpeed;
                    }
                    else
                    {
                        speeds[j] = -leafSpeed;
                    }
                }
            }
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

        private void restartButton_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private async void MainGameTimerEvent(object sender, EventArgs e)
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
                        if (player.Bounds.IntersectsWith(x.Bounds))
                            RestartGame();
                    }

                    if ((string)x.Tag == "leaf")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (!onLeaf)
                            {
                                Console.WriteLine("I'M ON A LEAF");
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
                                    Console.WriteLine("I'M NOT ON A LEAF");
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

                    if (x.Name == "borderLeft")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                            goLeft = false;
                    }
                    if (x.Name == "borderRight")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                            goRight = false;
                    }
                    if (x.Name == "borderBottom")
                    {if (player.Bounds.IntersectsWith(x.Bounds))
                        goDown = false;
                    }

                    if ((string)x.Tag == "slowTime")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            this.Controls.Remove(x);
                            gameTimer.Interval = 100;
                            await Task.Delay(4000);
                            gameTimer.Interval = 20;
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

            for (int i = 0; i < movableObjs.Length; i++)
            {
                movableObjs[i].Left -= speeds[i];

                if (movableObjs[i].Left + movableObjs[i].Width < 0)
                {
                    movableObjs[i].Left = this.ClientSize.Width;
                }
                if (movableObjs[i].Left > this.ClientSize.Width)
                {
                    movableObjs[i].Left = -movableObjs[i].Width;
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
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                if(player.Top < this.ClientSize.Height - 3/2 * player.Height)
                    goDown = true;

                switch(rotation)
                {
                    case "up":
                        player.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case "down":
                        break;
                    case "left":
                        player.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case "right":
                        player.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }
                player.Refresh();
                rotation = "down";
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                goUp = true;
                switch (rotation)
                {
                    case "up":
                        break;
                    case "down":
                        player.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case "left":
                        player.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case "right":
                        player.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                player.Refresh();
                rotation = "up";
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                if (player.Left > player.Width)
                    goLeft = true;

                switch (rotation)
                {
                    case "up":
                        player.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case "down":
                        player.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case "left":
                        break;
                    case "right":
                        player.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                }
                player.Refresh();
                rotation = "left";
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                if(player.Left < this.ClientSize.Width - player.Width)
                    goRight = true;

                switch (rotation)
                {
                    case "up":
                        player.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case "down":
                        player.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case "left":
                        player.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case "right":
                        break;
                }
                player.Refresh();
                rotation = "right";
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
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

            player.Left = this.ClientSize.Width / 2;
            player.Top = this.ClientSize.Height - 33;

            score = 100;

            foreach (var c in collectables)
            {
                this.Controls.Remove(c);
            }

            gameTimer.Interval = 20;
            Spawner.Start();
            ScoreTimer.Start();
            gameTimer.Start();
        }

        private async void SpawnCollectable()
        {
            Random random = new Random();

            int randomNum = random.Next(0,2);

            PictureBox pictureBox = new PictureBox();

            if (randomNum == 0)
            {
                pictureBox.ImageLocation = "../../Properties/Fly.png";
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Tag = "slowTime";
            }
            else
            {
                pictureBox.ImageLocation = "../../Properties/Worm.jpg";
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Tag = "addToScore";
            }

            pictureBox.Location = new Point(random.Next(50, 400), random.Next(1, 8) * 53);
            pictureBox.Height = 30;
            pictureBox.Width = 20;

            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            collectables.Add(pictureBox);

            await Task.Delay(7000);
            this.Controls.Remove(pictureBox);
        }

        public async void Wait()
        {
            await Task.Delay(500);
            wasDelayed = true;
        }

    }
}
