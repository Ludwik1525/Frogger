using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    class MovableObj
    {
        private int enemySpeed = 4;
        private int leafSpeed = 1;

        private int screenWidth;

        private PictureBox myObj;
        private int mySpeed;
        private int myDirection;

        public MovableObj(int screenWidth, PictureBox obj, int direction)
        {
            this.screenWidth = screenWidth;
            myObj = obj;
            SetMySpeed(obj);
            myDirection = direction;

            if(myDirection > 0)
            {
                myObj.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
        }

        private void SetMySpeed(PictureBox obj)
        {
            if (obj.Name.Contains("enemy"))
            {
                mySpeed = enemySpeed;
            }
            else
            {
                mySpeed = leafSpeed;
            }
        }

        public void Move()
        {
            myObj.Left -= (mySpeed * myDirection);

            if (myObj.Left + myObj.Width < 0)
            {
                myObj.Left = screenWidth;
            }
            if (myObj.Left > screenWidth)
            {
                myObj.Left = -myObj.Width;
            }
        }
    }
}
