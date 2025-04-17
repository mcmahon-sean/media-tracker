namespace media_tracker_desktop
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnDBConnectionTest = new Button();
            btnTestSteam = new Button();
            btnTestLastFMArtist = new Button();
            btnTestLastFMTrack = new Button();
            btnTestLastFMUser = new Button();
            btnTestTMDBAccount = new Button();
            btnTestTMDBMovie = new Button();
            btnTestTMDBTVShow = new Button();
            btnTestUserCreate = new Button();
            btnTestUserLogin = new Button();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            LblUsername = new Label();
            lblPassword = new Label();
            btnLinkSteam = new Button();
            btnLinkLastFm = new Button();
            btnLinkTmdb = new Button();
            btnGetUser = new Button();
            btnCheckSteam = new Button();
            btnCheckLastFM = new Button();
            btnCheckTMDB = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // btnDBConnectionTest
            // 
            btnDBConnectionTest.Location = new Point(29, 28);
            btnDBConnectionTest.Name = "btnDBConnectionTest";
            btnDBConnectionTest.Size = new Size(112, 34);
            btnDBConnectionTest.TabIndex = 0;
            btnDBConnectionTest.Text = "Test DB Connection";
            btnDBConnectionTest.UseVisualStyleBackColor = true;
            btnDBConnectionTest.Click += btnDBConnectionTest_Click;
            // 
            // btnTestSteam
            // 
            btnTestSteam.Location = new Point(178, 28);
            btnTestSteam.Name = "btnTestSteam";
            btnTestSteam.Size = new Size(112, 34);
            btnTestSteam.TabIndex = 1;
            btnTestSteam.Text = "Test steam";
            btnTestSteam.UseVisualStyleBackColor = true;
            btnTestSteam.Click += btnTestSteam_Click;
            // 
            // btnTestLastFMArtist
            // 
            btnTestLastFMArtist.Location = new Point(326, 28);
            btnTestLastFMArtist.Name = "btnTestLastFMArtist";
            btnTestLastFMArtist.Size = new Size(164, 34);
            btnTestLastFMArtist.TabIndex = 2;
            btnTestLastFMArtist.Text = "Test LastFM Artist";
            btnTestLastFMArtist.UseVisualStyleBackColor = true;
            btnTestLastFMArtist.Click += btnTestLastFMArtist_Click;
            // 
            // btnTestLastFMTrack
            // 
            btnTestLastFMTrack.Location = new Point(326, 79);
            btnTestLastFMTrack.Name = "btnTestLastFMTrack";
            btnTestLastFMTrack.Size = new Size(164, 34);
            btnTestLastFMTrack.TabIndex = 3;
            btnTestLastFMTrack.Text = "Test LastFM Track";
            btnTestLastFMTrack.UseVisualStyleBackColor = true;
            btnTestLastFMTrack.Click += btnTestLastFMTrack_Click;
            // 
            // btnTestLastFMUser
            // 
            btnTestLastFMUser.Location = new Point(326, 130);
            btnTestLastFMUser.Name = "btnTestLastFMUser";
            btnTestLastFMUser.Size = new Size(164, 34);
            btnTestLastFMUser.TabIndex = 4;
            btnTestLastFMUser.Text = "Test LastFM User";
            btnTestLastFMUser.UseVisualStyleBackColor = true;
            btnTestLastFMUser.Click += btnTestLastFMUser_Click;
            // 
            // btnTestTMDBAccount
            // 
            btnTestTMDBAccount.Location = new Point(541, 28);
            btnTestTMDBAccount.Name = "btnTestTMDBAccount";
            btnTestTMDBAccount.Size = new Size(189, 34);
            btnTestTMDBAccount.TabIndex = 5;
            btnTestTMDBAccount.Text = "Test TMDB Account";
            btnTestTMDBAccount.UseVisualStyleBackColor = true;
            btnTestTMDBAccount.Click += btnTestTMDBAccount_Click;
            // 
            // btnTestTMDBMovie
            // 
            btnTestTMDBMovie.Location = new Point(541, 79);
            btnTestTMDBMovie.Name = "btnTestTMDBMovie";
            btnTestTMDBMovie.Size = new Size(189, 34);
            btnTestTMDBMovie.TabIndex = 6;
            btnTestTMDBMovie.Text = "Test TMDB Movie";
            btnTestTMDBMovie.UseVisualStyleBackColor = true;
            btnTestTMDBMovie.Click += btnTestTMDBMovie_Click;
            // 
            // btnTestTMDBTVShow
            // 
            btnTestTMDBTVShow.Location = new Point(541, 130);
            btnTestTMDBTVShow.Name = "btnTestTMDBTVShow";
            btnTestTMDBTVShow.Size = new Size(189, 34);
            btnTestTMDBTVShow.TabIndex = 7;
            btnTestTMDBTVShow.Text = "Test TMDB TV Show";
            btnTestTMDBTVShow.UseVisualStyleBackColor = true;
            btnTestTMDBTVShow.Click += btnTestTMDBTVShow_Click;
            // 
            // btnTestUserCreate
            // 
            btnTestUserCreate.Location = new Point(26, 196);
            btnTestUserCreate.Name = "btnTestUserCreate";
            btnTestUserCreate.Size = new Size(160, 34);
            btnTestUserCreate.TabIndex = 8;
            btnTestUserCreate.Text = "Test User Create";
            btnTestUserCreate.UseVisualStyleBackColor = true;
            btnTestUserCreate.Click += btnTestUserCreate_Click;
            // 
            // btnTestUserLogin
            // 
            btnTestUserLogin.Location = new Point(26, 156);
            btnTestUserLogin.Name = "btnTestUserLogin";
            btnTestUserLogin.Size = new Size(160, 34);
            btnTestUserLogin.TabIndex = 9;
            btnTestUserLogin.Text = "Test User Login";
            btnTestUserLogin.UseVisualStyleBackColor = true;
            btnTestUserLogin.Click += btnTestUserLogin_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(26, 82);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(190, 31);
            txtUsername.TabIndex = 10;
            txtUsername.Text = "testDesktopUser";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(26, 119);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(190, 31);
            txtPassword.TabIndex = 11;
            txtPassword.Text = "testDesktopPassword";
            // 
            // LblUsername
            // 
            LblUsername.AutoSize = true;
            LblUsername.Location = new Point(222, 82);
            LblUsername.Name = "LblUsername";
            LblUsername.Size = new Size(91, 25);
            LblUsername.TabIndex = 14;
            LblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(222, 119);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(87, 25);
            lblPassword.TabIndex = 15;
            lblPassword.Text = "Password";
            // 
            // btnLinkSteam
            // 
            btnLinkSteam.Location = new Point(29, 372);
            btnLinkSteam.Name = "btnLinkSteam";
            btnLinkSteam.Size = new Size(125, 34);
            btnLinkSteam.TabIndex = 16;
            btnLinkSteam.Text = "Link Steam";
            btnLinkSteam.UseVisualStyleBackColor = true;
            btnLinkSteam.Click += btnLinkSteam_Click;
            // 
            // btnLinkLastFm
            // 
            btnLinkLastFm.Location = new Point(178, 372);
            btnLinkLastFm.Name = "btnLinkLastFm";
            btnLinkLastFm.Size = new Size(135, 34);
            btnLinkLastFm.TabIndex = 17;
            btnLinkLastFm.Text = "Link LastFM";
            btnLinkLastFm.UseVisualStyleBackColor = true;
            btnLinkLastFm.Click += btnLinkLastFm_Click;
            // 
            // btnLinkTmdb
            // 
            btnLinkTmdb.Location = new Point(326, 372);
            btnLinkTmdb.Name = "btnLinkTmdb";
            btnLinkTmdb.Size = new Size(135, 34);
            btnLinkTmdb.TabIndex = 18;
            btnLinkTmdb.Text = "Link TMDB";
            btnLinkTmdb.UseVisualStyleBackColor = true;
            btnLinkTmdb.Click += btnLinkTmdb_Click;
            // 
            // btnGetUser
            // 
            btnGetUser.Location = new Point(479, 372);
            btnGetUser.Name = "btnGetUser";
            btnGetUser.Size = new Size(171, 34);
            btnGetUser.TabIndex = 19;
            btnGetUser.Text = "Get Username";
            btnGetUser.UseVisualStyleBackColor = true;
            btnGetUser.Click += btnGetUser_Click;
            // 
            // btnCheckSteam
            // 
            btnCheckSteam.Location = new Point(29, 332);
            btnCheckSteam.Name = "btnCheckSteam";
            btnCheckSteam.Size = new Size(125, 34);
            btnCheckSteam.TabIndex = 20;
            btnCheckSteam.Text = "Check Steam";
            btnCheckSteam.UseVisualStyleBackColor = true;
            btnCheckSteam.Click += btnCheckSteam_Click;
            // 
            // btnCheckLastFM
            // 
            btnCheckLastFM.Location = new Point(178, 332);
            btnCheckLastFM.Name = "btnCheckLastFM";
            btnCheckLastFM.Size = new Size(135, 34);
            btnCheckLastFM.TabIndex = 21;
            btnCheckLastFM.Text = "Check LastFM";
            btnCheckLastFM.UseVisualStyleBackColor = true;
            btnCheckLastFM.Click += btnCheckLastFM_Click;
            // 
            // btnCheckTMDB
            // 
            btnCheckTMDB.Location = new Point(326, 332);
            btnCheckTMDB.Name = "btnCheckTMDB";
            btnCheckTMDB.Size = new Size(135, 34);
            btnCheckTMDB.TabIndex = 22;
            btnCheckTMDB.Text = "Check TMDB";
            btnCheckTMDB.UseVisualStyleBackColor = true;
            btnCheckTMDB.Click += btnCheckTMDB_Click;
            // 
            // button1
            // 
            button1.Location = new Point(541, 208);
            button1.Name = "button1";
            button1.Size = new Size(189, 34);
            button1.TabIndex = 23;
            button1.Text = "Create Request Token";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(btnCheckTMDB);
            Controls.Add(btnCheckLastFM);
            Controls.Add(btnCheckSteam);
            Controls.Add(btnGetUser);
            Controls.Add(btnLinkTmdb);
            Controls.Add(btnLinkLastFm);
            Controls.Add(btnLinkSteam);
            Controls.Add(lblPassword);
            Controls.Add(LblUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(btnTestUserLogin);
            Controls.Add(btnTestUserCreate);
            Controls.Add(btnTestTMDBTVShow);
            Controls.Add(btnTestTMDBMovie);
            Controls.Add(btnTestTMDBAccount);
            Controls.Add(btnTestLastFMUser);
            Controls.Add(btnTestLastFMTrack);
            Controls.Add(btnTestLastFMArtist);
            Controls.Add(btnTestSteam);
            Controls.Add(btnDBConnectionTest);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDBConnectionTest;
        private Button btnTestSteam;
        private Button btnTestLastFMArtist;
        private Button btnTestLastFMTrack;
        private Button btnTestLastFMUser;
        private Button btnTestTMDBAccount;
        private Button btnTestTMDBMovie;
        private Button btnTestTMDBTVShow;
        private Button btnTestUserCreate;
        private Button btnTestUserLogin;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label LblUsername;
        private Label lblPassword;
        private Button btnLinkSteam;
        private Button btnLinkLastFm;
        private Button btnLinkTmdb;
        private Button btnGetUser;
        private Button btnCheckSteam;
        private Button btnCheckLastFM;
        private Button btnCheckTMDB;
        private Button button1;
    }
}
