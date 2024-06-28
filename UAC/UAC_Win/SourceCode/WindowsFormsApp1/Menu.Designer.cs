namespace WindowsFormsApp1
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.panelMenu = new System.Windows.Forms.Panel();
            this.iconBtnAbout = new FontAwesome.Sharp.IconButton();
            this.panelUACSubMenu = new System.Windows.Forms.Panel();
            this.btnUserPermission = new System.Windows.Forms.Button();
            this.btnCreateUserRoles = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnexit = new FontAwesome.Sharp.IconButton();
            this.iconBtnUAC = new FontAwesome.Sharp.IconButton();
            this.iconBtnHome = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMenu = new FontAwesome.Sharp.IconButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelDesktop = new System.Windows.Forms.Panel();
            this.panelMenu.SuspendLayout();
            this.panelUACSubMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelDesktop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.White;
            this.panelMenu.Controls.Add(this.iconBtnAbout);
            this.panelMenu.Controls.Add(this.panelUACSubMenu);
            this.panelMenu.Controls.Add(this.btnexit);
            this.panelMenu.Controls.Add(this.iconBtnUAC);
            this.panelMenu.Controls.Add(this.iconBtnHome);
            this.panelMenu.Controls.Add(this.panel1);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(4);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(427, 684);
            this.panelMenu.TabIndex = 0;
            // 
            // iconBtnAbout
            // 
            this.iconBtnAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.iconBtnAbout.FlatAppearance.BorderSize = 0;
            this.iconBtnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconBtnAbout.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconBtnAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnAbout.IconChar = FontAwesome.Sharp.IconChar.BookOpenReader;
            this.iconBtnAbout.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnAbout.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBtnAbout.IconSize = 30;
            this.iconBtnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnAbout.Location = new System.Drawing.Point(0, 427);
            this.iconBtnAbout.Margin = new System.Windows.Forms.Padding(4);
            this.iconBtnAbout.Name = "iconBtnAbout";
            this.iconBtnAbout.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.iconBtnAbout.Size = new System.Drawing.Size(427, 50);
            this.iconBtnAbout.TabIndex = 8;
            this.iconBtnAbout.Tag = "About";
            this.iconBtnAbout.Text = "   About";
            this.iconBtnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconBtnAbout.UseVisualStyleBackColor = true;
            this.iconBtnAbout.Click += new System.EventHandler(this.iconBtnAbout_Click);
            this.iconBtnAbout.MouseEnter += new System.EventHandler(this.iconBtnAbout_MouseEnter);
            this.iconBtnAbout.MouseLeave += new System.EventHandler(this.iconBtnAbout_MouseLeave);
            // 
            // panelUACSubMenu
            // 
            this.panelUACSubMenu.BackColor = System.Drawing.Color.White;
            this.panelUACSubMenu.Controls.Add(this.btnUserPermission);
            this.panelUACSubMenu.Controls.Add(this.btnCreateUserRoles);
            this.panelUACSubMenu.Controls.Add(this.btnChangePassword);
            this.panelUACSubMenu.Controls.Add(this.btnCreateUser);
            this.panelUACSubMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUACSubMenu.Location = new System.Drawing.Point(0, 223);
            this.panelUACSubMenu.Margin = new System.Windows.Forms.Padding(4);
            this.panelUACSubMenu.Name = "panelUACSubMenu";
            this.panelUACSubMenu.Size = new System.Drawing.Size(427, 204);
            this.panelUACSubMenu.TabIndex = 7;
            // 
            // btnUserPermission
            // 
            this.btnUserPermission.BackColor = System.Drawing.Color.White;
            this.btnUserPermission.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUserPermission.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnUserPermission.FlatAppearance.BorderSize = 0;
            this.btnUserPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserPermission.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserPermission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnUserPermission.Location = new System.Drawing.Point(0, 147);
            this.btnUserPermission.Margin = new System.Windows.Forms.Padding(4);
            this.btnUserPermission.Name = "btnUserPermission";
            this.btnUserPermission.Padding = new System.Windows.Forms.Padding(80, 0, 0, 0);
            this.btnUserPermission.Size = new System.Drawing.Size(427, 49);
            this.btnUserPermission.TabIndex = 4;
            this.btnUserPermission.Text = "User Role Permission";
            this.btnUserPermission.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserPermission.UseVisualStyleBackColor = false;
            this.btnUserPermission.Click += new System.EventHandler(this.btnUserPermission_Click);
            // 
            // btnCreateUserRoles
            // 
            this.btnCreateUserRoles.BackColor = System.Drawing.Color.White;
            this.btnCreateUserRoles.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreateUserRoles.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnCreateUserRoles.FlatAppearance.BorderSize = 0;
            this.btnCreateUserRoles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateUserRoles.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateUserRoles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnCreateUserRoles.Location = new System.Drawing.Point(0, 98);
            this.btnCreateUserRoles.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateUserRoles.Name = "btnCreateUserRoles";
            this.btnCreateUserRoles.Padding = new System.Windows.Forms.Padding(80, 0, 0, 0);
            this.btnCreateUserRoles.Size = new System.Drawing.Size(427, 49);
            this.btnCreateUserRoles.TabIndex = 3;
            this.btnCreateUserRoles.Text = "Create User Role";
            this.btnCreateUserRoles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateUserRoles.UseVisualStyleBackColor = false;
            this.btnCreateUserRoles.Click += new System.EventHandler(this.btnCreateUserRoles_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BackColor = System.Drawing.Color.White;
            this.btnChangePassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChangePassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnChangePassword.FlatAppearance.BorderSize = 0;
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnChangePassword.Location = new System.Drawing.Point(0, 49);
            this.btnChangePassword.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Padding = new System.Windows.Forms.Padding(80, 0, 0, 0);
            this.btnChangePassword.Size = new System.Drawing.Size(427, 49);
            this.btnChangePassword.TabIndex = 2;
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.BackColor = System.Drawing.Color.White;
            this.btnCreateUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreateUser.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnCreateUser.FlatAppearance.BorderSize = 0;
            this.btnCreateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateUser.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnCreateUser.Location = new System.Drawing.Point(0, 0);
            this.btnCreateUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Padding = new System.Windows.Forms.Padding(80, 0, 0, 0);
            this.btnCreateUser.Size = new System.Drawing.Size(427, 49);
            this.btnCreateUser.TabIndex = 1;
            this.btnCreateUser.Text = "Create User";
            this.btnCreateUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateUser.UseVisualStyleBackColor = false;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // btnexit
            // 
            this.btnexit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnexit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnexit.FlatAppearance.BorderSize = 0;
            this.btnexit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnexit.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexit.ForeColor = System.Drawing.Color.White;
            this.btnexit.IconChar = FontAwesome.Sharp.IconChar.SignOutAlt;
            this.btnexit.IconColor = System.Drawing.Color.White;
            this.btnexit.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnexit.IconSize = 30;
            this.btnexit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnexit.Location = new System.Drawing.Point(0, 561);
            this.btnexit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 12);
            this.btnexit.Name = "btnexit";
            this.btnexit.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnexit.Size = new System.Drawing.Size(427, 123);
            this.btnexit.TabIndex = 6;
            this.btnexit.Tag = "Logout";
            this.btnexit.Text = "   Logout";
            this.btnexit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnexit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnexit.UseVisualStyleBackColor = false;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // iconBtnUAC
            // 
            this.iconBtnUAC.Dock = System.Windows.Forms.DockStyle.Top;
            this.iconBtnUAC.FlatAppearance.BorderSize = 0;
            this.iconBtnUAC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconBtnUAC.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconBtnUAC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnUAC.IconChar = FontAwesome.Sharp.IconChar.Key;
            this.iconBtnUAC.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnUAC.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBtnUAC.IconSize = 30;
            this.iconBtnUAC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnUAC.Location = new System.Drawing.Point(0, 173);
            this.iconBtnUAC.Margin = new System.Windows.Forms.Padding(4);
            this.iconBtnUAC.Name = "iconBtnUAC";
            this.iconBtnUAC.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.iconBtnUAC.Size = new System.Drawing.Size(427, 50);
            this.iconBtnUAC.TabIndex = 2;
            this.iconBtnUAC.Tag = "UAC";
            this.iconBtnUAC.Text = "   UAC";
            this.iconBtnUAC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnUAC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconBtnUAC.UseVisualStyleBackColor = false;
            this.iconBtnUAC.Click += new System.EventHandler(this.iconBtnUAC_Click);
            this.iconBtnUAC.MouseEnter += new System.EventHandler(this.iconBtnUAC_MouseEnter);
            this.iconBtnUAC.MouseLeave += new System.EventHandler(this.iconBtnUAC_MouseLeave);
            // 
            // iconBtnHome
            // 
            this.iconBtnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.iconBtnHome.FlatAppearance.BorderSize = 0;
            this.iconBtnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconBtnHome.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconBtnHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnHome.IconChar = FontAwesome.Sharp.IconChar.HomeUser;
            this.iconBtnHome.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.iconBtnHome.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBtnHome.IconSize = 30;
            this.iconBtnHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnHome.Location = new System.Drawing.Point(0, 123);
            this.iconBtnHome.Margin = new System.Windows.Forms.Padding(4);
            this.iconBtnHome.Name = "iconBtnHome";
            this.iconBtnHome.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.iconBtnHome.Size = new System.Drawing.Size(427, 50);
            this.iconBtnHome.TabIndex = 1;
            this.iconBtnHome.Tag = "Home";
            this.iconBtnHome.Text = "   Home";
            this.iconBtnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconBtnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconBtnHome.UseVisualStyleBackColor = true;
            this.iconBtnHome.Click += new System.EventHandler(this.iconBtnHome_Click);
            this.iconBtnHome.MouseEnter += new System.EventHandler(this.iconBtnHome_MouseEnter);
            this.iconBtnHome.MouseLeave += new System.EventHandler(this.iconBtnHome_MouseLeave);
            this.iconBtnHome.MouseHover += new System.EventHandler(this.iconBtnHome_MouseHover);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.btnMenu);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 123);
            this.panel1.TabIndex = 0;
            // 
            // btnMenu
            // 
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.IconChar = FontAwesome.Sharp.IconChar.Navicon;
            this.btnMenu.IconColor = System.Drawing.Color.White;
            this.btnMenu.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu.IconSize = 30;
            this.btnMenu.Location = new System.Drawing.Point(227, 15);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(4);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(72, 49);
            this.btnMenu.TabIndex = 1;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(64, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 85);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(427, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(7, 684);
            this.panel2.TabIndex = 3;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(854, 684);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // panelDesktop
            // 
            this.panelDesktop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelDesktop.Controls.Add(this.pictureBox2);
            this.panelDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesktop.Location = new System.Drawing.Point(427, 0);
            this.panelDesktop.Margin = new System.Windows.Forms.Padding(4);
            this.panelDesktop.Name = "panelDesktop";
            this.panelDesktop.Size = new System.Drawing.Size(854, 684);
            this.panelDesktop.TabIndex = 2;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 684);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.panelMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Menu_Load);
            this.Resize += new System.EventHandler(this.Menu_Resize);
            this.panelMenu.ResumeLayout(false);
            this.panelUACSubMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelDesktop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton iconBtnHome;
        private FontAwesome.Sharp.IconButton btnexit;
        private FontAwesome.Sharp.IconButton iconBtnUAC;
        private FontAwesome.Sharp.IconButton btnMenu;
        private System.Windows.Forms.Panel panelUACSubMenu;
        private System.Windows.Forms.Button btnCreateUser;
        private FontAwesome.Sharp.IconButton iconBtnAbout;
        private System.Windows.Forms.Button btnUserPermission;
        private System.Windows.Forms.Button btnCreateUserRoles;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panelDesktop;
    }
}