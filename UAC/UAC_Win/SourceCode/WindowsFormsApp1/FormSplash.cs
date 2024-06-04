using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormSplash : Form
    {
        private const int AnimationSteps = 30; // Number of animation steps
        private const int AnimationDuration = 1000; // Animation duration in milliseconds
        private int currentStep = 0;
        private int targetWidth;
        private int targetHeight;
        private int stepWidth;
        private int stepHeight;

        public FormSplash()
        {
            InitializeComponent();
            targetWidth = panel1.Width + 100; // Target width after animation
            targetHeight = panel1.Height + 100; // Target height after animation
            stepWidth = (targetWidth - panel1.Width) / AnimationSteps;
            stepHeight = (targetHeight - panel1.Height) / AnimationSteps;

            Timer timer = new Timer();
            timer.Interval = AnimationDuration / AnimationSteps;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentStep < AnimationSteps)
            {
                panel1.Width += stepWidth;
                panel1.Height += stepHeight;
                currentStep++;
            }
            else
            {
                ((Timer)sender).Stop(); // Stop the timer
                // Animation complete
            }
        }
    }
}
