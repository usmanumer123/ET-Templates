

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Splash : Form
    {
        private int targetXLabel4;
        private int targetXLabel5;
        private int targetXPictureBox1;
        private int labelMoveStep = 1; // Number of pixels to move the labels each time (lower for slower movement)
        private int labelMoveSpeed = 20; // Interval in milliseconds between label movements (higher for slower movement)
        private int pictureBoxMoveStep = 1; // Number of pixels to move the picture box each time (lower for slower movement)
        private int pictureBoxMoveSpeed = 20; // Interval in milliseconds between picture box movements (higher for slower movement)
        private int elapsedSeconds = 0; // Elapsed seconds since splash screen started
        private const int splashDuration = 20; // Duration of splash screen in seconds

        public Splash()
        {
            InitializeComponent();
            timer1.Start();
            // Set initial positions of labels and picture box
            targetXLabel4 = label4.Location.X;
            targetXLabel5 = label5.Location.X;
            targetXPictureBox1 = pictureBox1.Location.X;
            label4.Location = new Point(label4.Location.X + 24, label4.Location.Y);
            label5.Location = new Point(label5.Location.X + 24, label5.Location.Y);
            pictureBox1.Location = new Point(pictureBox1.Location.X + 24, pictureBox1.Location.Y);
            InitializeLabelAnimationTimer();
            InitializePictureBoxAnimationTimer();
        }

        private void InitializeLabelAnimationTimer()
        {
            Timer timerLabelAnimation = new Timer();
            timerLabelAnimation.Interval = labelMoveSpeed;
            timerLabelAnimation.Tick += TimerLabelAnimation_Tick;
            timerLabelAnimation.Start();
        }

        private void InitializePictureBoxAnimationTimer()
        {
            Timer timerPictureBoxAnimation = new Timer();
            timerPictureBoxAnimation.Interval = pictureBoxMoveSpeed;
            timerPictureBoxAnimation.Tick += TimerPictureBoxAnimation_Tick;
            timerPictureBoxAnimation.Start();
        }

        private void TimerLabelAnimation_Tick(object sender, EventArgs e)
        {
            // Move labels with transition
            if (label4.Location.X > targetXLabel4)
            {
                label4.Location = new Point(label4.Location.X - labelMoveStep, label4.Location.Y);
            }

            if (label5.Location.X > targetXLabel5)
            {
                label5.Location = new Point(label5.Location.X - labelMoveStep, label5.Location.Y);
            }

            // Check if labels reached the target position
            if (label4.Location.X <= targetXLabel4 && label5.Location.X <= targetXLabel5)
            {
                label4.Location = new Point(targetXLabel4, label4.Location.Y);
                label5.Location = new Point(targetXLabel5, label5.Location.Y);
            }
        }

        private void TimerPictureBoxAnimation_Tick(object sender, EventArgs e)
        {
            // Move picture box with transition
            if (pictureBox1.Location.X > targetXPictureBox1)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X - pictureBoxMoveStep, pictureBox1.Location.Y);
            }

            // Check if picture box reached the target position
            if (pictureBox1.Location.X <= targetXPictureBox1)
            {
                pictureBox1.Location = new Point(targetXPictureBox1, pictureBox1.Location.Y);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            // Move labels with transition
            label4.Location = new Point(label4.Location.X - 2, label4.Location.Y);
            label5.Location = new Point(label5.Location.X - 2, label5.Location.Y);

            // Check if labels reached the target position
            if (label4.Location.X <= targetXLabel4 || label5.Location.X <= targetXLabel5)
            {
                label4.Location = new Point(targetXLabel4, label4.Location.Y);
                label5.Location = new Point(targetXLabel5, label5.Location.Y);
            }

            // Move picture box with transition
            pictureBox1.Location = new Point(pictureBox1.Location.X - 2, pictureBox1.Location.Y);

            // Check if picture box reached the target position
            if (pictureBox1.Location.X <= targetXPictureBox1)
            {
                pictureBox1.Location = new Point(targetXPictureBox1, pictureBox1.Location.Y);
            }

            // Increase elapsed time
            elapsedSeconds++;

            // Check if elapsed time is greater than or equal to splash duration
            if (elapsedSeconds >= splashDuration)
            {
                timer1.Stop();
                Login form = new Login();
                form.Show();
                this.Hide();
            }


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }

     
    }


