using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Drawing.Drawing2D;

namespace SimonSays
{
    public partial class Form1 : Form
    {
        //Create a List to store the pattern. Must be accessable on other screens
        public static List<int> pattern = new List<int>() { 0, 1, 2, 3 };

        public static Color[] flatColors = new Color[] { Color.ForestGreen, Color.DarkRed, Color.DarkBlue, Color.Goldenrod };
        public static Color[] lightColors = new Color[] { Color.LimeGreen, Color.Pink, Color.LightSkyBlue, Color.LightYellow };
        public static SoundPlayer[] soundPlayers = new SoundPlayer[] { new SoundPlayer(Properties.Resources.green), new SoundPlayer(Properties.Resources.red), new SoundPlayer(Properties.Resources.blue), new SoundPlayer(Properties.Resources.yellow) };
        public static int highScore = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Launch MenuScreen
            DisplayScreen(this, new MenuScreen());
        }

        public static void DisplayScreen(object sender, UserControl next)
        {
            Form f;
            if (sender is Form)
            {
                f = (Form)sender;
            }
            else
            {
                UserControl current = (UserControl)sender;
                f = current.FindForm();
                f.Controls.Remove(current);
            }
            next.Location = new Point((f.Width - next.Width) / 2, (f.Height - next.Height) / 2);
            f.Controls.Add(next);
        }
    }
}
