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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
