using System;
using System.Windows.Forms;

namespace Frogger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new GameManager());

            GameManager gm = new GameManager();
            gm.Show(); 
            gm.GameLoop();
        }
    }
}
