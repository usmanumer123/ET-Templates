using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormsApp1
{
    public partial class Menu : Form
    {

        CFL_CV_PracticeEntities1 context = new CFL_CV_PracticeEntities1();

        


        

        private Color defaultTabColor = Color.FromArgb(41, 128, 185);
        // private Color defaultTabColor = Color.White;




        //Here is the code of Tab changing colors 


        private void ResetTabColors()
        {
            //// Reset the background color of all tab buttons to default
            //foreach (Control control in panelMenu.Controls)
            //{
            //    if (control is Button tabButton)
            //    {
            //        tabButton.BackColor = defaultTabColor;
            //    }
            //}
            // Reset the background color and foreground color of all icon buttons to default
            iconBtnHome.BackColor = Color.White;
            iconBtnHome.ForeColor = defaultTabColor;
            iconBtnHome.IconColor = defaultTabColor;
            iconBtnUAC.BackColor = Color.White;
            iconBtnUAC.ForeColor = defaultTabColor;
            iconBtnUAC.IconColor = defaultTabColor;
            iconBtnAbout.BackColor = Color.White;
            iconBtnAbout.ForeColor = defaultTabColor;
            iconBtnAbout.IconColor = defaultTabColor;
        }
        private void ResetUACSubMenuTabColors()
        {
            // Reset the background color and foreground color of all tab buttons to default
            foreach (Control control in panelUACSubMenu.Controls)
            {
                if (control is Button tabButton)
                {
                    tabButton.BackColor = Color.White;
                    tabButton.ForeColor = defaultTabColor;
                }
            }
        }


       
        private bool IsAdmin(int roleId)
        {
            return true;
        }
        public Menu(int roleId)
        {
            InitializeComponent();


            var isAdmin = IsAdmin(roleId);

            btnCreateUser.Enabled = isAdmin && CheckUserPermission(roleId, "Create User");
            btnChangePassword.Enabled = isAdmin && CheckUserPermission(roleId, "Change Password");
            btnCreateUserRoles.Enabled = isAdmin && CheckUserPermission(roleId, "Create User Role");
            btnUserPermission.Enabled = isAdmin && CheckUserPermission(roleId, "Set User Permission");

            // Similar lines for other modules and permissions
        }

        private bool CheckUserPermission(int roleId, string module)
        {
            var permissions = context.UserRollsPermissions.FirstOrDefault(p => p.RollsId == roleId && p.Module == module );
            return permissions != null && permissions.IsEnable;
        }

        //changing of colors for disable 
        //private void SetButtonState(Button button, bool isEnabled)
        //{
        //    button.Enabled = isEnabled;
        //    button.BackColor = isEnabled ? SystemColors.Control : Color.LightGray;
        //    button.ForeColor = isEnabled ? SystemColors.ControlText : Color.LightGray;
        //}




        private void AdjustTabsEnabled()
        {
            // Enable/disable tab icon buttons based on the user's role

           
            //iconButton4.Enabled = false;
            //iconButton5.Enabled = isAdmin;
            //iconButton6.Enabled = isAdmin;
        }
        //Fields
        private int borderSize = 2;
        private Size formSize; //Keep form size when it is minimized and restored.
                               //Since the form is resized because it takes into account the
                               //size of the title bar and borders.

        //Constructor
        public Menu()
        {
            InitializeComponent();
            customizeDesign();

           

            //CollapseMenu();
            this.Padding = new Padding(borderSize);//Border size
            this.BackColor = Color.FromArgb(98, 102, 244);//Border color

        }

        private void customizeDesign()
        {
            panelUACSubMenu.Visible = false;
        }

        private void hideSubMenu()
        {
            if(panelUACSubMenu.Visible == true) 
            { 
            
            panelUACSubMenu.Visible = false;
            
            }
        }

        private void showSubMenu( Panel submenu) 
        {
            if(submenu.Visible == false)
            {
                hideSubMenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panelTitleBar_MouseDown_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_NCCALCSIZE = 0x0083;
        //    if(m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
        //    {
        //        return;
        //    }
        //    base.WndProc(ref m);

        //}
        //Overridden methods
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 10;

            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right

            ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>

            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          

                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion

            //Remove border and keep snap window
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }

            //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);

                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }

        //Event Ethod
        private void Menu_Resize(object sender, EventArgs e)
        {
            AdjustForm();
        }

        //private method

            private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(8, 8, 8, 0); break;
                case FormWindowState.Normal:
                    if (this.Padding.Top != borderSize)
                        this.Padding = new Padding(borderSize);
                    this.Padding = new Padding(borderSize); break;



            }

        }

        private void btnMinimizeClick_Click(object sender, EventArgs e)
        {

            formSize = this.ClientSize;
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click_Click(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {
                formSize = this.ClientSize;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = formSize;
            }
        }

        private void btnClose_Click_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200) //Collapse menu
            {
                panelMenu.Width = 75;
                pictureBox1.Visible = false;
                panelUACSubMenu.Visible = false;
                btnMenu.Dock = DockStyle.Top;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            else
            { //Expand menu
                panelMenu.Width = 230;
                pictureBox1.Visible = true;
                btnMenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10, 0, 0, 0);
                }
            }
        }

        private void SubmenuCollapse()
        {

            if (this.panelMenu.Width > 200) //Collapse menu
                                            //{
                                            //    panelMenu.Width = 75;
                                            //    pictureBox1.Visible = false;
                                            //    panelUACSubMenu.Visible = true;
                                            //    btnMenu.Dock = DockStyle.Top;
                                            //    foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                                            //    {
                                            //        menuButton.Text = "";
                                            //        menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                                            //        menuButton.Padding = new Padding(0);
                                            //    }
                                            //}
                                            //    else
            { //Expand menu
                panelMenu.Width = 230;
                pictureBox1.Visible = true;
                btnMenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10, 0, 0, 0);
                }
            }

        }
        private Form activeForm = null;
        private void  openChildForm (Form childForm)
        {
            if (activeForm != null)
            
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;

            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            
        }

        private void iconBtnUAC_Click(object sender, EventArgs e)
        {
            ResetTabColors();
            ResetUACSubMenuTabColors();
            iconBtnUAC.BackColor = defaultTabColor;
            iconBtnUAC.ForeColor = Color.White;

            showSubMenu(panelUACSubMenu);
            if (this.panelMenu.Width == 75)
            {
                panelUACSubMenu.Visible = false;
                
            }
            //CollapseMenu();
            //SubmenuCollapse();
            //openChildForm(new About());
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new CreateUser());
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Login form = new Login();
            form.Show();
            this.Hide();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            openChildForm(new CreateUser());
            ResetUACSubMenuTabColors();
            btnCreateUser.BackColor = Color.SkyBlue;
            btnCreateUser.ForeColor = Color.White;

        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            openChildForm(new ChangePassword());
            ResetUACSubMenuTabColors();
            btnChangePassword.BackColor = Color.SkyBlue;
            btnChangePassword.ForeColor = Color.White;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void iconBtnAbout_Click(object sender, EventArgs e)
        {
            ResetTabColors();
            ResetUACSubMenuTabColors();
            iconBtnAbout.BackColor = defaultTabColor;
            iconBtnAbout.ForeColor = Color.White;
            openChildForm(new About());
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            panelUACSubMenu.Visible = false;
            //iconBtnHome.BackColor = Color.SkyBlue;


        }

        private void btnCreateUserRoles_Click(object sender, EventArgs e)
        {
            openChildForm(new CreateUserRole());
            ResetUACSubMenuTabColors();
            btnCreateUserRoles.BackColor = Color.SkyBlue;
            btnCreateUserRoles.ForeColor = Color.White;
        }

        private void btnUserPermission_Click(object sender, EventArgs e)
        {
            openChildForm(new SetUserPermission());
            ResetUACSubMenuTabColors();
            btnUserPermission.BackColor = Color.SkyBlue;
            btnUserPermission.ForeColor = Color.White;
        }

        private void iconBtnHome_Click(object sender, EventArgs e)
        {
            panelDesktop.Visible = true;
            ResetTabColors();
            ResetUACSubMenuTabColors();
            iconBtnHome.BackColor = defaultTabColor;
            iconBtnHome.ForeColor = Color.White;
            openChildForm(new Home());

        }

        private void iconBtnHome_MouseEnter(object sender, EventArgs e)
        {
            if (!iconBtnHome.Focused)
            {
                iconBtnHome.BackColor = Color.DarkBlue;
                iconBtnHome.ForeColor = Color.White;
                iconBtnHome.IconColor = Color.White;
            }
        }

        private void iconBtnHome_MouseLeave(object sender, EventArgs e)
        {
            if (!iconBtnHome.Focused)
            {
                iconBtnHome.ForeColor = defaultTabColor;
                iconBtnHome.BackColor = Color.White;
                iconBtnHome.IconColor = defaultTabColor;
            }
        }

        private void iconBtnUAC_MouseEnter(object sender, EventArgs e)
        {
            if (!iconBtnUAC.Focused)
            {
                iconBtnUAC.BackColor = Color.DarkBlue;
                iconBtnUAC.ForeColor = Color.White;
                iconBtnUAC.IconColor = Color.White;
            }
        }

        private void iconBtnUAC_MouseLeave(object sender, EventArgs e)
        {
            if (!iconBtnUAC.Focused)
            {
                iconBtnUAC.ForeColor = defaultTabColor;
                iconBtnUAC.BackColor = Color.White;
                iconBtnUAC.IconColor = defaultTabColor;
            }
        }

        private void iconBtnHome_MouseHover(object sender, EventArgs e)
        {

            //iconBtnHome.BackColor = Color.DarkBlue;
            //iconBtnHome.ForeColor = Color.White;
            
        }

        private void iconBtnAbout_MouseLeave(object sender, EventArgs e)
        {
            if (!iconBtnAbout.Focused)
            {
                iconBtnAbout.ForeColor = defaultTabColor;
                iconBtnAbout.BackColor = Color.White;
                iconBtnAbout.IconColor = defaultTabColor;
            }
        }

        private void iconBtnAbout_MouseEnter(object sender, EventArgs e)
        {
            if (!iconBtnAbout.Focused)
            {
                iconBtnAbout.BackColor = Color.DarkBlue;
                iconBtnAbout.ForeColor = Color.White;
                iconBtnAbout.IconColor = Color.White;
            }
        }

        private void btnexit_MouseEnter(object sender, EventArgs e)
        {
            //if (!btnexit.Focused)
            //{
            //    btnexit.BackColor = Color.DarkBlue;
            //    btnexit.ForeColor = Color.White;
            //    btnexit.IconColor = Color.White;
            //}

        }

        private void btnexit_MouseLeave(object sender, EventArgs e)
        {
            //if (!iconBtnAbout.Focused)
            //{
            //   btnexit.ForeColor = defaultTabColor;
            //   btnexit.BackColor = Color.White;
            //    btnexit.IconColor = defaultTabColor;
            //}

        }
    }
    
}
