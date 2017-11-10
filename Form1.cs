using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using Org.Eurekaa.PDF.iSafePDF.Properties;
using System.IO;
using Org.Eurekaa.PDF.iSafePDF.Lib;


//demo cert http://v4.nwp.home.net.pl/digi-seal/demo+certificate
//demo TSA http://www.ca-soft.com/request.aspx
namespace Org.Eurekaa.PDF.iSafePDF
{
    public partial class Form1 : Form
    {
        private PDFEncryption PDFEnc = new PDFEncryption();
        private PickBox pb = new PickBox();
        private PdfReader reader = null;
        X509Certificate2 certificateData = null;


        public Form1()
        {
            InitializeComponent();
            pb.WireControl(sigPicture);
            


            
        }


        private void debug(string txt)
        {
            DebugBox.AppendText(txt + System.Environment.NewLine);
        }



        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile;
            openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "PDF files *.pdf|*.pdf";
            openFile.Title = "Select a file";
            if (openFile.ShowDialog() != DialogResult.OK)
                return;

            inputBox.Text = openFile.FileName;

            
            
            try
            {
                reader = new PdfReader(inputBox.Text);

            }
            catch
            {
                PwdDialog dlg = new PwdDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string pwd = (dlg.Controls["pwdTextBox"] as TextBox).Text;
                    reader = new PdfReader(inputBox.Text, Tools.StrToByteArray(pwd));
                }
                else
                {
                    inputBox.Text = "";
                    return;
                }
            }
            MetaData md = new MetaData();
            md.Info = reader.Info;

            authorBox.Text = md.Author;
            titleBox.Text = md.Title;
            subjectBox.Text = md.Subject;
            kwBox.Text = md.Keywords;
            creatorBox.Text = md.Creator;
            prodBox.Text = md.Producer;


            numberOfPagesUpDown.Maximum = reader.NumberOfPages;
            numberOfPagesUpDown.Minimum = numberOfPagesUpDown.Value = 1;
            numberOfPagesUpDown_ValueChanged(numberOfPagesUpDown, null);

            sigPicture.Left = 0;
            sigPicture.Top = sigPicture.Parent.Height - sigPicture.Height;
            sigPicture_Move(sigPicture, null);
            
            sigPicture.Width = 50;
            sigPicture.Height = 20;
            sigPicture_Resize(sigPicture, null);
            
            
        }



        private void button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFile;
            saveFile = new System.Windows.Forms.SaveFileDialog();            
            saveFile.Filter = "PDF files (*.pdf)|*.pdf";
            saveFile.Title = "Save PDF File";
            if (saveFile.ShowDialog() != DialogResult.OK)
                return;
            outputBox.Text = saveFile.FileName;

        }



        private void ProcessBtn_Click(object sender, EventArgs e)
        {
            debug("*****Started (document = " + inputBox.Text + " => "+outputBox.Text+") ");

            debug("Checking certificate ...");
            Cert myCert=null;
            try
            {
                string tsaUrl = String.IsNullOrEmpty(TSAUrlTextBox.Text) ? null : TSAUrlTextBox.Text;
                if (certificateData != null)
                {
                    //X509Certificate2 cert = certsListBox.SelectedItem as X509Certificate2;
                    byte[] bytes = certificateData.Export(X509ContentType.Pfx, certificatePwdBox.Text);
                    myCert = new Cert(bytes, certificatePwdBox.Text, tsaUrl, tsaLogin.Text, tsaPwd.Text);
                }
                else
                {
                   myCert = new Cert(certificateTextBox.Text, certificatePwdBox.Text, tsaUrl, tsaLogin.Text, tsaPwd.Text);
                }

                debug("Certificate OK");                
            }
            catch (Exception ex)
            {                
                debug("Warning : No valid certificate found, please make sure you entered a valid certificate file and password");
                //debug("Exception : " + ex.ToString());
                debug(" ==> Continue ... the document will not be signed !");
                //return;
            }

            debug("Checking encryption options ...");
            PDFEnc.UserPwd = encUserPwd.Text;
            PDFEnc.OwnerPwd = encOwnerPwd.Text;


            debug("Creating new MetaData object... ");

            //Adding Meta Datas
            MetaData MyMD = new MetaData();
            MyMD.Author = authorBox.Text;
            MyMD.Title = titleBox.Text;
            MyMD.Subject = subjectBox.Text;
            MyMD.Keywords = kwBox.Text;
            MyMD.Creator = creatorBox.Text;
            MyMD.Producer = prodBox.Text;


            debug("Processing document ... ");
            PDFSigner pdfs = new PDFSigner(inputBox.Text, outputBox.Text, myCert, MyMD);
            PDFSignatureAP sigAp = new PDFSignatureAP();
            sigAp.SigReason = Reasontext.Text;
            sigAp.SigContact = Contacttext.Text;
            sigAp.SigLocation = Locationtext.Text;
            sigAp.Visible = SigVisible.Checked;
            sigAp.Multi = multiSigChkBx.Checked;
            sigAp.Page = Convert.ToInt32(numberOfPagesUpDown.Value);
            sigAp.CustomText = custSigText.Text;

            if (sigImgBox.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                sigImgBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                sigAp.RawData = ms.ToArray();
                ms.Close();
            }

            sigAp.SigX = (float)sigPosX.Value;
            sigAp.SigY = (float)sigPosY.Value;
            sigAp.SigW = (float)sigWidth.Value;
            sigAp.SigH = (float)sigHeight.Value;
            
            pdfs.Sign(sigAp, encryptChkBx.Checked, PDFEnc);

            debug("Done :)");

            MessageBox.Show("The document has been succesfully processed", "iSafePDF :: Signature done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PDFEnc.Permissions.Add(permissionType.Assembly);
            PDFEnc.Permissions.Add(permissionType.Copy);
            PDFEnc.Permissions.Add(permissionType.DegradedPrinting);
            PDFEnc.Permissions.Add(permissionType.FillIn);
            PDFEnc.Permissions.Add(permissionType.ModifyAnnotation);
            PDFEnc.Permissions.Add(permissionType.ModifyContent);
            PDFEnc.Permissions.Add(permissionType.Printing);
            PDFEnc.Permissions.Add(permissionType.ScreenReaders);

            webBrowser1.Navigate("about:blank");
            if (webBrowser1.Document != null)
            {
                webBrowser1.Document.Write(string.Empty);
            }
            webBrowser1.DocumentText = Resources.DonateBtn;           
        }


        private void tsaCbx_CheckedChanged(object sender, EventArgs e)
        {
            TSAUrlTextBox.Enabled = (sender as CheckBox).Checked;
            tsaLogin.Enabled = (sender as CheckBox).Checked;
            tsaPwd.Enabled = (sender as CheckBox).Checked;
            TSALbl1.Enabled = (sender as CheckBox).Checked;
            TSALbl2.Enabled = (sender as CheckBox).Checked;
            TSALbl3.Enabled = (sender as CheckBox).Checked;
            
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void encryptChkBx_CheckedChanged(object sender, EventArgs e)
        {
            EncryptionGrp.Enabled = (sender as CheckBox).Checked;
            ProtectionGrp.Enabled = (sender as CheckBox).Checked;
            if ((sender as CheckBox).Checked)
                multiSigChkBx.Checked = false;
        }

        private void encAssemblyChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.Assembly);
            else PDFEnc.Permissions.Remove(permissionType.Assembly);
        }

        private void encCopyChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.Copy);
            else PDFEnc.Permissions.Remove(permissionType.Copy);

        }

        private void encDegPrintChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.DegradedPrinting);
            else PDFEnc.Permissions.Remove(permissionType.DegradedPrinting);

        }

        private void encFillInChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.FillIn);
            else PDFEnc.Permissions.Remove(permissionType.FillIn);

        }

        private void encModAnnotChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.ModifyAnnotation);
            else PDFEnc.Permissions.Remove(permissionType.ModifyAnnotation);

        }

        private void encModContChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.ModifyContent);
            else PDFEnc.Permissions.Remove(permissionType.ModifyContent);

        }

        private void encPrintChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.Printing);
            else PDFEnc.Permissions.Remove(permissionType.Printing);

        }

        private void encScrRdChk_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked) PDFEnc.Permissions.Add(permissionType.ScreenReaders);
            else PDFEnc.Permissions.Remove(permissionType.ScreenReaders);

        }



        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
        }

        private void tabControl1_Deselected(object sender, TabControlEventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessBtn.Visible = !tabControl1.SelectedTab.Equals(aboutTab);
        }

        private void VisitProjectHome(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //LinkLabel lnk = sender as LinkLabel;
            System.Diagnostics.Process.Start("IExplore", "http://isafepdf.eurekaa.org");
            
        }



        private void certTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile;
            openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "*.jpg|*.gif|*.bmp|*.png";
            openFile.Title = "Select a file";
            if (openFile.ShowDialog() != DialogResult.OK)
                return;

            sigPicture.Image = sigImgBox.Image = new Bitmap(openFile.FileName);
        }




        private void numberOfPagesUpDown_ValueChanged(object sender, EventArgs e)
        {

            
            iTextSharp.text.Rectangle rect = reader.GetPageSize(Convert.ToInt32(numberOfPagesUpDown.Value));

            pagePreviewPanel.Top = 0;

            if (rect.Width > rect.Height)
            {
                pagePreviewPanel.Width = pagePreviewPanel.Parent.Width;
                pagePreviewPanel.Height = Convert.ToInt32((pagePreviewPanel.Width * rect.Height) / rect.Width);
            }
            else
            {
                pagePreviewPanel.Height = pagePreviewPanel.Parent.Height;
                pagePreviewPanel.Width = Convert.ToInt32((pagePreviewPanel.Height * rect.Width) / rect.Height);                
            }
            pagePreviewPanel.Left = (pagePreviewPanel.Parent.Width - pagePreviewPanel.Width) / 2;
            pagePreviewPanel.Top = (pagePreviewPanel.Parent.Height - pagePreviewPanel.Height) / 2;


            sigPosX.Maximum = Convert.ToInt32(rect.Width);
            sigPosY.Maximum = Convert.ToInt32(rect.Height);

            sigWidth.Maximum = Convert.ToInt32(rect.Width);
            sigHeight.Maximum = Convert.ToInt32(rect.Height);
            
        }

        private void sigPicture_Move(object sender, EventArgs e)
        {
            iTextSharp.text.Rectangle rect = reader.GetPageSize(Convert.ToInt32(numberOfPagesUpDown.Value));
            


            decimal X = Convert.ToInt32( (rect.Width * sigPicture.Left) / pagePreviewPanel.Width ); 

            decimal Y = sigPicture.Parent.Height - sigPicture.Top - sigPicture.Height;
            Y =  Convert.ToInt32( (rect.Height * (float)Y) / pagePreviewPanel.Height );

            if (X > sigPosX.Maximum) X = sigPosX.Maximum;
            if (X < sigPosX.Minimum) X = sigPosX.Minimum;
            if (Y > sigPosY.Maximum) Y = sigPosY.Maximum;
            if (Y < sigPosY.Minimum) Y = sigPosY.Minimum;

            sigPosX.Value = X;
            sigPosY.Value = Y;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            sigPicture.Image = sigImgBox.Image = null;
        }

        private void sigPicture_Resize(object sender, EventArgs e)
        {
            iTextSharp.text.Rectangle rect = reader.GetPageSize(Convert.ToInt32(numberOfPagesUpDown.Value));

            decimal W = Convert.ToInt32((rect.Width * sigPicture.Width) / pagePreviewPanel.Width);

            decimal H = Convert.ToInt32((rect.Height * sigPicture.Height) / pagePreviewPanel.Height);
            
            if (W > sigWidth.Maximum) W = sigWidth.Maximum;
            if (W < sigWidth.Minimum) W = sigWidth.Minimum;
            if (H > sigHeight.Maximum) H = sigHeight.Maximum;
            if (H < sigHeight.Minimum) H = sigHeight.Minimum;

            sigWidth.Value = W;
            sigHeight.Value = H;
        }

        private void SigVisible_CheckedChanged(object sender, EventArgs e)
        {
            sigPanel1.Visible = sigPanel2.Visible = sigPanel1.Enabled = sigPanel2.Enabled = SigVisible.Checked;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void selectSertBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(inputBox.Text) || String.IsNullOrEmpty(outputBox.Text))
            {
                MessageBox.Show("Please go to the 'Document tab' and select a source and a target file first", "iSafePDF :: Action required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            certificateData = null;
            sigPanel.Visible = false;
            certificateTextBox.Text = String.Empty;

            CertificateDialog dlg = new CertificateDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ListBox certs = (dlg.Controls["certsListBox"] as ListBox);
                if (certs.SelectedItem != null)
                {
                    certificateData = certs.SelectedItem as X509Certificate2;
                    certificateTextBox.Text = certs.SelectedItem.ToString();
                }
                else
                {
                    certificateTextBox.Text = (dlg.Controls["certTextBox"] as TextBox).Text;
                }
                certificatePwdBox.Text = (dlg.Controls["passwordBox"] as TextBox).Text;

                sigPanel.Visible = !String.IsNullOrEmpty(certificateTextBox.Text);
            }

        }



        
        

    }
}