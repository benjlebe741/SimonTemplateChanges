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
    public partial class rotatingGameScreen : UserControl
    {
        #region Global Variables
        Button[] buttons = new Button[4];
        GraphicsPath outerCircle = new GraphicsPath();
        GraphicsPath innerCircle = new GraphicsPath();
        GraphicsPath polygon = new GraphicsPath();
        Matrix rotationMatrix = new Matrix();
        Matrix smallRotationMatrix = new Matrix();
        const int OUTER_CIRCLE = 50;
        const int INNER_CIRCLE = 220;
        const int OFFSET = 7;
        const double ROTATION_DEGREE = 4;

        int placeInPattern = 0;
        Random random = new Random();
        #endregion
        public rotatingGameScreen()
        {
            InitializeComponent();
            Form1.pattern.Clear();

            //Region Info
            outerCircle.AddEllipse(new Rectangle(OUTER_CIRCLE / 2, OUTER_CIRCLE / 2, this.Width - OUTER_CIRCLE, this.Height - OUTER_CIRCLE));
            innerCircle.AddEllipse(new Rectangle(INNER_CIRCLE / 2, INNER_CIRCLE / 2, this.Width - INNER_CIRCLE, this.Height - INNER_CIRCLE));

            //Matrix Info
            Point centre = new Point(this.Width / 2, this.Height / 2);
            rotationMatrix.RotateAt(360 / buttons.Length, centre);
            smallRotationMatrix.RotateAt((float)ROTATION_DEGREE, centre);

            //Creating the intersecting shape
            double angle = (Math.PI * 2) / buttons.Length;
            PointF[] polygonPoints = new PointF[4] { new PointF(centre.X + OFFSET, centre.Y - OFFSET), new PointF((this.Width / 2) + OFFSET, 0), new PointF(this.Width, 0), new PointF(this.Width, (this.Height / 2) - OFFSET) };
            polygon.AddPolygon(polygonPoints);


            setUpButtons();
        }

        Region createRegion()
        {
            Region region = new Region();
            region.Intersect(outerCircle);
            region.Exclude(innerCircle);
            region.Intersect(polygon);
            return region;
        }
        void setUpButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                //Fill the screen with each button
                buttons[i] = new Button();
                this.Controls.Add(buttons[i]);
                buttons[i].Location = new Point(0, 0);
                buttons[i].Size = new Size(this.Width, this.Height);
                buttons[i].BackColor = Form1.flatColors[i];
                buttons[i].Name = i.ToString();
                buttons[i].Click += new EventHandler(Button_Click);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button ghostButton = (Button)sender;
            int buttonNum = Convert.ToInt32(ghostButton.Name);

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
            // else GameOver
            else
            {
                //Close this screen and open the GameOverScreen
                Form1.DisplayScreen(this, new GameOverScreen());
            }
        }

        void LightButton(int currentButton, int waitTime)
        {
            //Get the button you want to change, then alternate between its lit-up and not-lit-up mode
            Button button = buttons[currentButton];
            //Play correct button Sound:
            Form1.soundPlayers[currentButton].Play();
            ChangeButtonLook(button, Form1.lightColors[currentButton], waitTime);
            ChangeButtonLook(button, Form1.flatColors[currentButton], waitTime);
        }
        void ChangeButtonLook(Button button, Color color, int waitTime)
        {
            button.BackColor = color;
            wait(waitTime);
        }

        private void ComputerTurn()
        {
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
        }

        void wait(int waitTime)
        {
            Refresh();
            Thread.Sleep(waitTime);
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            updateRegions();
        }

        void updateRegions()
        {
            //rotate each region
            polygon.Transform(smallRotationMatrix);

            for (int i = 0; i < buttons.Length; i++)
            {
                polygon.Transform(rotationMatrix);
                buttons[i].Region = createRegion();
            }
        }

        private void rotatingGameScreen_Load(object sender, EventArgs e)
        {
            updateRegions();
            ComputerTurn();
        }
    }
}
