using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Org.Eurekaa.PDF.iSafePDF
{
    public partial class CertificateDialog : Form
    {
        public CertificateDialog()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile;
            openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "Certificate files (*.pfx)|*.pfx|Certificate files (*.p12)|*.p12";
            openFile.Title = "Select a file";
            if (openFile.ShowDialog() != DialogResult.OK)
                return;

            certTextBox.Text = openFile.FileName;


            certsListBox.Enabled = false;

            certsListBox.SelectedItem = null;

            certsListBox.Enabled = true;

        }


        private void OKBtn_Click(object sender, EventArgs e)
        {

        }

        private void CertificateDialog_Load(object sender, EventArgs e)
        {
            X509Store store = new X509Store(StoreName.My);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 cert in store.Certificates)
            {
                certsListBox.Items.Add(cert);
            }


        }

        private void certsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListBox).Enabled)
                certTextBox.Text = string.Empty;
        }
    }
}
