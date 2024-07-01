using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Menu : Form
    {
        private int borderSize = 2; 
        private Color defaultTabColor = Color.FromArgb(41, 128, 185);
        private Color TabColorOnClick = Color.FromArgb(73, 180, 230);

        public Menu()
        {
            InitializeComponent();
            customizeDesign();
            panelMenu.Width = 350;
            this.Padding = new Padding(borderSize);
            this.BackColor = Color.FromArgb(98, 102, 244);
        }

        public Menu(int roleId)
        {
            InitializeComponent();
            panelMenu.Width = 350;
            btnCreateUser.Enabled = CheckUserPermission(roleId, "Create User");
            btnChangePassword.Enabled = CheckUserPermission(roleId, "Change Password");
            btnCreateUserRoles.Enabled = CheckUserPermission(roleId, "Create User Role");
            btnUserPermission.Enabled = CheckUserPermission(roleId, "Set User Permission");
        }

        private void customizeDesign()
        {
            panelUACSubMenu.Visible = false;
        }

        private void ResetTabColors()
        {
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

        private bool CheckUserPermission(int roleId, string module)
        {
            var permissions = Shared.context.UserRollsPermissions
                .Where(p => p.RollsId == roleId && p.Module == module)
                .ToList();
            bool anyPermissionEnabled = permissions.Any(p => p.IsEnable);
            return anyPermissionEnabled;
        }   

        private void hideSubMenu()
        {
            if(panelUACSubMenu.Visible == true) 
            {  
                panelUACSubMenu.Visible = false;
            }
        }

        private void showSubMenu(Panel submenu) 
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

        private void Menu_Resize(object sender, EventArgs e)
        {
            AdjustForm();
        }

        private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(8, 8, 8, 0); 
                    break;
                case FormWindowState.Normal:
                    this.Padding = new Padding(borderSize); 
                    break;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200)
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
            {   //Expand menu
                panelMenu.Width = 350;
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

        private void  openChildForm (Form childForm)
        {
            if (childForm != null)
            {
                panelDesktop.Controls.Clear();
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                panelDesktop.Controls.Add(childForm);
                panelDesktop.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();
            }
        }

        private void iconBtnUAC_Click(object sender, EventArgs e)
        {
            ResetTabColors();
            ResetUACSubMenuTabColors();
            iconBtnUAC.BackColor = TabColorOnClick;
            iconBtnUAC.ForeColor = Color.White;
            showSubMenu(panelUACSubMenu);

            if (this.panelMenu.Width == 75)
            {
                panelUACSubMenu.Visible = false;
                
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            this.Hide();
            form.Show();
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

        private void iconBtnAbout_Click(object sender, EventArgs e)
        {
            ResetTabColors();
            ResetUACSubMenuTabColors();
            iconBtnAbout.BackColor = TabColorOnClick;
            iconBtnAbout.ForeColor = Color.White;
            openChildForm(new About());
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            panelUACSubMenu.Visible = false;
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
            iconBtnHome.BackColor = TabColorOnClick;
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
                Size formSize = this.ClientSize;
                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }
    }
}
