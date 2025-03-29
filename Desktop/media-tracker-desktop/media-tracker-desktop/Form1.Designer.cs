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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDBConnectionTest);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnDBConnectionTest;
    }
}
