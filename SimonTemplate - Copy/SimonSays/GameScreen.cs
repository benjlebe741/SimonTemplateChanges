using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        #region Global Variables
        //Create an int guess variable to track what part of the pattern the user is at
        int placeInPattern = 0;
        Button[] buttons;
        //Constant values for positioning different regions
        const int OUTER_CIRCLE = 200;
        const int INNER_CIRCLE = 90;
        const int INNER_OFFSET = 4;
        const int REMOVE_BORDER = 4;
        const int BACK_CIRCLE_OFFSET = 39;
        //Graphics paths
        GraphicsPath removedBorders = new GraphicsPath();
        GraphicsPath removedArc = new GraphicsPath();
        GraphicsPath addedArc = new GraphicsPath();
        GraphicsPath backgroundCircle = new GraphicsPath();
        Matrix transformMatrix = new Matrix();

        Point buttonAxis;
        Random random = new Random();
        #endregion
        public GameScreen()
        {
            InitializeComponent();
            //Reset the pattern on a new game, get the buttons array to refrence the buttons on screen
            Form1.pattern.Clear();
            buttons = new Button[4] { greenButton, redButton, blueButton, yellowButton };

            //Change The Buttons Regions:
            int size = greenButton.Width;
            addedArc.AddEllipse(new Rectangle(size - (OUTER_CIRCLE / 2), size - (OUTER_CIRCLE / 2), OUTER_CIRCLE, OUTER_CIRCLE));
            removedArc.AddEllipse(new Rectangle(size - (INNER_CIRCLE / 2) + INNER_OFFSET, size - (INNER_CIRCLE / 2) + INNER_OFFSET, INNER_CIRCLE, INNER_CIRCLE));

            //Create Transformation Matrix to rotate buttons
            transformMatrix.RotateAt(90, new PointF(greenButton.Width / 2, greenButton.Height / 2));
            //Apply new region to buttons
            SetButtonRegions(transformMatrix);

            //Apply a region to the label in the background, displaying a black circle and practicing regions
            backgroundCircle.AddEllipse(new Rectangle(0 + BACK_CIRCLE_OFFSET, 0 + BACK_CIRCLE_OFFSET, this.Width - (2 * BACK_CIRCLE_OFFSET), this.Width - (2 * BACK_CIRCLE_OFFSET)));
            backCircle.Region = new Region(backgroundCircle);

            buttonAxis = new Point(this.Width / 2, this.Height / 2);
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            ComputerTurn();
        }

        private void ComputerTurn()
        {
            //Dissable buttons *THIS DOESNT WORK BECAUSE IM USING THREAD.SLEEP INSTEAD OF A PROPER TIMER*
            ButtonState(false);
            //Change background color to show its the computers turn
            this.BackColor = Color.Black;
            wait(1000);

            //Get rand num between 0 and 4 (0, 1, 2, 3) and add to pattern list.
            Form1.pattern.Add(random.Next(0, 3));

            //Create a for loop that shows each value in the pattern by lighting up approriate button
            for (int i = 0; i < Form1.pattern.Count; i++)
            {
                LightButton(Form1.pattern[i], 300);
            }

            //Reset the place in the pattern, and switch things back so the player can have their turn
            placeInPattern = 0;
            this.BackColor = Color.DimGray;
            ButtonState(true);
        }

        void LightButton(int currentButton, int waitTime)
        {
            //Get the button you want to change, then alternate between its lit-up and not-lit-up mode
            Button button = buttons[currentButton];
            //Play correct button Sound:
            Form1.soundPlayers[currentButton].Play();
            ChangeButtonLook(button, Form1.lightColors[currentButton], 1, waitTime);
            ChangeButtonLook(button, Form1.flatColors[currentButton], -1, waitTime);
        }

        #region buttonClicks
        private void greenButton_Click(object sender, EventArgs e)
        {
            buttonClick(0);
        }

        private void redButton_Click(object sender, EventArgs e)
        {
            buttonClick(1);
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            buttonClick(3);
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            buttonClick(2);
        }
        #endregion
        void SetButtonRegions(Matrix matrix)
        {
            //For each button, set their region to be an ark, cut out the small circle, and intersect it with a rectangle smaller than the boarders. This could be simplified by changing one region and just using that probably
            for (int i = 0; i < 4; i++)
            {
                buttons[i].Region = new Region(addedArc);
                buttons[i].Region.Exclude(removedArc);
                buttons[i].Region.Intersect(new RectangleF(REMOVE_BORDER, REMOVE_BORDER, greenButton.Width - (2 * REMOVE_BORDER), greenButton.Height - (2 * REMOVE_BORDER)));

                //After a buttons region has been set, rotate the regions 90 degrees to use it for the next button.
                addedArc.Transform(matrix);
                removedArc.Transform(matrix);
                removedBorders.Transform(matrix);
            }
        }
        void buttonClick(int buttonNum)
        {
            // change button color
            LightButton(buttonNum, 100);
            //Is the value in the pattern list at index [placeInPattern] equal to the pressed button?
            if (buttonNum == Form1.pattern[placeInPattern])
            {
                placeInPattern++;

                //Check to see if we are at the end of the pattern, (guess is the same as pattern list count).
                if (placeInPattern == Form1.pattern.Count)
                {
                    ComputerTurn();
                }
            }
            // else call GameOver method
            else
            {
                GameOver();
            }
        }
        public void GameOver()
        {
            //TODO: Play a game over sound

            //Close this screen and open the GameOverScreen
            Form1.DisplayScreen(this, new GameOverScreen());
        }
        void ButtonState(bool state)
        {
            foreach (Button button in buttons)
            {
                button.Enabled = state;
            }
        }
        void wait(int waitTime)
        {
            Refresh();
            Thread.Sleep(waitTime);
        }
        void ChangeButtonLook(Button button, Color color, int largeOrSmall, int waitTime)
        {
            button.BackColor = color;
            button.Width += 10 * largeOrSmall;
            button.Height += 10 * largeOrSmall;
            button.Location = new Point(button.Location.X - (5 * largeOrSmall), button.Location.Y - (5 * largeOrSmall));
            wait(waitTime);
        }
        //Everything is pretty much done by this line, I wanted to experiment with rotating regions, so thats what everything after this is!
        private void WaitTimer_Tick(object sender, EventArgs e)
        {
            ////I would want to make an actual timer instead of using sleep here-- to stop players from being able to give button inputs while its the computers turn
        }

        private void backCircle_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
