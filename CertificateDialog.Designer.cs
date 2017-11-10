namespace Org.Eurekaa.PDF.iSafePDF
{
    partial class CertificateDialog
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.certsListBox = new System.Windows.Forms.ListBox();
            this.browseBtn = new System.Windows.Forms.Button();
            this.certTextBox = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(508, 369);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(144, 39);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 197);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(53, 13);
            this.label22.TabIndex = 28;
            this.label22.Text = "or browse";
            // 
            // certsListBox
            // 
            this.certsListBox.FormattingEnabled = true;
            this.certsListBox.Location = new System.Drawing.Point(12, 25);
            this.certsListBox.Name = "certsListBox";
            this.certsListBox.Size = new System.Drawing.Size(640, 147);
            this.certsListBox.TabIndex = 27;
            this.certsListBox.SelectedIndexChanged += new System.EventHandler(this.certsListBox_SelectedIndexChanged);
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(594, 196);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(58, 21);
            this.browseBtn.TabIndex = 22;
            this.browseBtn.Text = "Browse";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // certTextBox
            // 
            this.certTextBox.Location = new System.Drawing.Point(80, 197);
            this.certTextBox.Name = "certTextBox";
            this.certTextBox.Size = new System.Drawing.Size(508, 20);
            this.certTextBox.TabIndex = 21;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(12, 265);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(164, 13);
            this.label24.TabIndex = 23;
            this.label24.Text = "Password (if protected certificate)";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(182, 265);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(470, 20);
            this.passwordBox.TabIndex = 25;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(12, 9);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(130, 13);
            this.label25.TabIndex = 24;
            this.label25.Text = "Choose certificate from list";
            // 
            // CertificateDialog
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 420);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.certsListBox);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.browseBtn);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.certTextBox);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.label24);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificateDialog";
            this.Text = "Certificate";
            this.Load += new System.EventHandler(this.CertificateDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ListBox certsListBox;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.TextBox certTextBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label25;
    }
}