namespace media_tracker_desktop.Forms
{
    partial class UpdateUserForm
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
            label6 = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtUsername = new TextBox();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtPassword = new TextBox();
            txtEmail = new TextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label6
            // 
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(39, 86);
            label6.Name = "label6";
            label6.Size = new Size(198, 38);
            label6.TabIndex = 0;
            label6.Text = "Username:";
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 11F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(39, 149);
            label1.Name = "label1";
            label1.Size = new Size(198, 38);
            label1.TabIndex = 1;
            label1.Text = "First Name:";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 11F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(39, 212);
            label2.Name = "label2";
            label2.Size = new Size(198, 38);
            label2.TabIndex = 2;
            label2.Text = "Last Name:";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 11F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(39, 275);
            label3.Name = "label3";
            label3.Size = new Size(198, 38);
            label3.TabIndex = 3;
            label3.Text = "Email:";
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 11F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(39, 338);
            label4.Name = "label4";
            label4.Size = new Size(198, 38);
            label4.TabIndex = 4;
            label4.Text = "Password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(153, 23);
            label5.Name = "label5";
            label5.Size = new Size(179, 45);
            label5.TabIndex = 5;
            label5.Text = "Edit Details";
            // 
            // txtUsername
            // 
            txtUsername.Cursor = Cursors.No;
            txtUsername.Location = new Point(193, 87);
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = true;
            txtUsername.Size = new Size(273, 31);
            txtUsername.TabIndex = 6;
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(193, 150);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(273, 31);
            txtFirstName.TabIndex = 7;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(193, 213);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(273, 31);
            txtLastName.TabIndex = 8;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(193, 339);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(273, 31);
            txtPassword.TabIndex = 9;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(193, 276);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(273, 31);
            txtEmail.TabIndex = 9;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(39, 438);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(139, 47);
            btnConfirm.TabIndex = 10;
            btnConfirm.Text = "&Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(327, 438);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(139, 47);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Canc&el";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // UpdateUserForm
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            CancelButton = btnCancel;
            ClientSize = new Size(505, 527);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(txtLastName);
            Controls.Add(txtFirstName);
            Controls.Add(txtUsername);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label6);
            Name = "UpdateUserForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Detail";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtUsername;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtPassword;
        private TextBox txtEmail;
        private Button btnConfirm;
        private Button btnCancel;
    }
}