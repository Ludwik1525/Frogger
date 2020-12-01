using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    public partial class GameManager : Form
    {
        double score = 100.0;

        bool onLeaf = false, isGameFinished = false;

        TimeSpan MS_PER_FRAME;

        MovableObj[] movableObjs = new MovableObj[35];
        List<PictureBox> collectibles = new List<PictureBox>();
        ObjectPool<Fly> FlyObjectPool = new ObjectPool<Fly>();
        ObjectPool<Worm> WormObjectPool = new ObjectPool<Worm>();

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
            if (!isGameFinished)
            {
                frog.Move();

                foreach (MovableObj obj in movableObjs)
                {
                    obj.Move();
                }

                if (GameTimer.Enabled)
                {
                    if (score > 0.1)
                    {
                        score -= 0.1;
                        txtTimer.Text = "Score: " + String.Format("{0:0.00}", score);
                    }
                }
            }

            
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                RestartGame();
            }
            else
            {
                frog.Rotate(e);
                frog.MoveTheFrog(e);
            }
        }

        private async void MainGameTimerEvent(object sender, EventArgs e)
        {
            onLeaf = false;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if ((string)x.Tag == "enemy")
                        {
                                RestartGame();
                        }

                        if ((string)x.Tag == "leaf" || (string)x.Tag == "ground")
                        {
                            if (!onLeaf)
                            {
                                onLeaf = true;
                            }
                        }

                        if ((string)x.Tag == "water")
                        {
                            if (!onLeaf)
                            {
                                RestartGame();
                            }
                        }

                        if ((string)x.Tag == "slowTime")
                        {
                            this.Controls.Remove(x);
                            MS_PER_FRAME = TimeSpan.FromMilliseconds(2.0 / 60.0 * 10000.0);
                            await Task.Delay(4000);
                            MS_PER_FRAME = TimeSpan.FromMilliseconds(1.0 / 60.0 * 10000.0);
                        }

                        if ((string)x.Tag == "addToScore")
                        {
                            this.Controls.Remove(x);
                            score += 5;
                        }

                        if (x.Name == "finishLine")
                        {
                            isGameFinished = true;

                            GameTimer.Stop();
                            SpawnerTimer.Stop();

                            winLabel.Visible = true;
                            winLabel.BringToFront();

                            winLabel.Text = "You won!" + Environment.NewLine + "Score: " + String.Format("{0:0.00}", score)
                                + Environment.NewLine + "Press [ESC] to restart.";
                        }
                    }
                }
            }
        }

        private async void SpawnCollectible(object sender, EventArgs e)
        {
            Random random = new Random();

            int randomNum = random.Next(0, 2);

            if (randomNum == 0)
            {
                Fly obj = FlyObjectPool.Get();
                FlyObjectPool.Release(obj);

                obj.SetPosition();
                var fly = obj.GetFly();

                this.Controls.Add(fly);
                fly.BringToFront();
                collectibles.Add(fly);

                await Task.Delay(7000);
                this.Controls.Remove(fly);
            }
            else
            {
                Worm obj = WormObjectPool.Get();
                WormObjectPool.Release(obj);
                obj.SetPosition();


                var worm = obj.GetWorm();

                this.Controls.Add(worm);
                worm.BringToFront();
                collectibles.Add(worm);

                await Task.Delay(7000);
                this.Controls.Remove(worm);
            }

            
        }

        private void SetupBoard(object sender, EventArgs e)
        {
            GameTimer.Start();
            SpawnerTimer.Start();

            int i = 0;

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "enemy" || (string)x.Tag == "leaf")
                {
                    if(i%2 == 0)
                        movableObjs[i] = new MovableObj(this.ClientSize.Width, (PictureBox)x, 1);
                    else
                        movableObjs[i] = new MovableObj(this.ClientSize.Width, (PictureBox)x, -1);

                    i++;
                }
            }
        }

        private void RestartGame()
        {
            winLabel.Visible = false;

            player.Left = this.ClientSize.Width / 2;
            player.Top = this.ClientSize.Height - 33;

            score = 100.0;

            foreach (var c in collectibles)
            {
                this.Controls.Remove(c);
            }

            if (isGameFinished)
                isGameFinished = false;

            SpawnerTimer.Start();
            GameTimer.Start();
        }
    }
}
