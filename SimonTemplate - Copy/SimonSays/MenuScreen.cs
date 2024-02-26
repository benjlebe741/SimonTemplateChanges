﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            //Remove this screen and start the GameScreen
            Form1.DisplayScreen(this, new GameScreen());
        }


        private void exitButton_Click(object sender, EventArgs e)
        {
            //End the application
            Application.Exit();
        }

        private void newRotatingButton_Click(object sender, EventArgs e)
        {
            //Remove this screen and start the RotatingGameScreen
            Form1.DisplayScreen(this, new rotatingGameScreen());
        }
    }
}
