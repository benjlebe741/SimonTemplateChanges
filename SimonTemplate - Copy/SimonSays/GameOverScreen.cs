using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class GameOverScreen : UserControl
    {
        const int PATTERN_Y = 160;
        const int PATTERN_HEIGHT = 60;
        public GameOverScreen()
        {
            InitializeComponent();
        }

        private void GameOverScreen_Load(object sender, EventArgs e)
        {
            //Show the length of the pattern
            int patternLength = Form1.pattern.Count - 1;
            lengthLabel.Text = Convert.ToString(patternLength);
            DisplayPattern();

            //Update High Score
            if (Form1.highScore < patternLength)
            {
                Form1.highScore = patternLength;
            }
            HighScoreLabel.Text = Convert.ToString(Form1.highScore);
        }

        void DisplayPattern()
        {

            int colorSpace = 0;
            if (Form1.pattern.Count - 1 != 0)
            {
                colorSpace = this.Width / (Form1.pattern.Count - 1);
            }

            for (int patternNum = 0; patternNum < Form1.pattern.Count - 1; patternNum++)
            {
                Graphics g = this.CreateGraphics();
                Rectangle rect = new Rectangle(colorSpace * patternNum, PATTERN_Y, colorSpace, PATTERN_HEIGHT);
                SolidBrush brush = new SolidBrush(Form1.flatColors[Form1.pattern[patternNum]]);
                g.FillRectangle(brush, rect);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            //Close this screen and open the MenuScreen
            Form1.DisplayScreen(this, new MenuScreen());
        }
    }
}
