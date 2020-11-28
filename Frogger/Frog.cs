using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    class Frog
    {
        PictureBox _playerFrog;

        int speed = 10;

        readonly int posX;
        readonly int posY;

        readonly int screenWidth;
        readonly int screenHeight;

        readonly FrogDirection frogDirection;

        public Frog(int posX, int posY, PictureBox playerFrog)
        {
            this._playerFrog = playerFrog;
            this.posX = posX / 2;
            this.posY = posY - 33;
            this.frogDirection = new FrogDirection(FrogDirection.Direction.Up);

            this.screenWidth = posX;
            this.screenHeight = posY;

        }

        public void ChangeDirection(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                frogDirection.MoveUp();

            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                frogDirection.MoveDown();

            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                frogDirection.MoveRight();

            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                frogDirection.MoveLeft();

            if(GameState.NotMoving == frogDirection.GetCurrentGameState())
            {
                frogDirection.StartMoving();
            }
        }

        public void Move()
        {
            if(frogDirection.GetCurrentGameState() == GameState.Moving)
            {
                switch (frogDirection.GetCurrentDirection())
                {
                    case FrogDirection.Direction.Left:
                        if(_playerFrog.Left - speed > 0)
                            _playerFrog.Left -= speed;
                        break;
                    case FrogDirection.Direction.Right:
                        if (_playerFrog.Right + speed < screenWidth)
                            _playerFrog.Left += speed;
                        break;
                    case FrogDirection.Direction.Up:
                        if (_playerFrog.Top - speed > 0)
                            _playerFrog.Top -= speed;
                        break;
                    case FrogDirection.Direction.Down:
                        if (_playerFrog.Top + speed < screenHeight - _playerFrog.Height)
                            _playerFrog.Top += speed;
                        break;
                }

                frogDirection.StopMoving();
            } 
        }

        public void Rotate(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                switch (frogDirection.GetCurrentDirection())
                {
                    case FrogDirection.Direction.Up:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case FrogDirection.Direction.Left:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case FrogDirection.Direction.Right:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                switch (frogDirection.GetCurrentDirection())
                {
                    case FrogDirection.Direction.Down:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case FrogDirection.Direction.Left:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case FrogDirection.Direction.Right:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
            }

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                switch (frogDirection.GetCurrentDirection())
                {
                    case FrogDirection.Direction.Up:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case FrogDirection.Direction.Down:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case FrogDirection.Direction.Right:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                }
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                switch (frogDirection.GetCurrentDirection())
                {
                    case FrogDirection.Direction.Up:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case FrogDirection.Direction.Down:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case FrogDirection.Direction.Left:
                        _playerFrog.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                }
            }

            _playerFrog.Refresh();
        }
    }
}
