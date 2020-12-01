using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frogger
{
    class Worm
    {
        PictureBox collectible = new PictureBox();
        public Worm()
        {
            SetImage();
            SetPosition();
        }

        public PictureBox GetWorm()
        {
            return this.collectible;
        }
        private void SetImage()
        {
            collectible.ImageLocation = "../../Resources/Worm.png";
            collectible.SizeMode = PictureBoxSizeMode.AutoSize;
            collectible.Tag = "addToScore";
        }
        public void SetPosition()
        {
            Random random = new Random();

            collectible.Location = new Point(random.Next(50, 400), random.Next(1, 9) * 53);
            collectible.Height = 30;
            collectible.Width = 20;
        }
    }
}
