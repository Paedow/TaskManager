using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.Xml;
using System.Diagnostics;

namespace rptm
{
    public partial class frmMain : Form
    {
        MessageServer ms = new MessageServer("http://www.nightking.org/rptm.api/");
        private string userName = "";
        public frmMain()
        {
            InitializeComponent();
            string test = txtChatlog.Text;
            refreshTimer.Start();
            Text = "RübenPartyTaskManager "+Application.ProductVersion;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            txtMessage.Focus();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Login();
            }
        }

        private void Login()
        {
            if (txtUsername.Text.Length >= 3)
            {
                txtChatlog.Enabled = true;
                txtMessage.Enabled = true;
                cmdSend.Enabled = true;
                txtUsername.Enabled = false;
                userName = txtUsername.Text;
                cmdLogin.Enabled = false;
                ms.SendMessage("[ " + userName + " hat sich angemeldet. ]", "system");
                RefreshChatWindow();
                txtMessage.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                HandleInput();
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshChatWindow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(userName)) ms.SendMessage(string.Format("[ {0} hat sich abgemeldet. ]", userName), "system");
        }

        private void RefreshChatWindow()
        {
            string oldText = txtChatlog.Text;
            txtChatlog.Text = ms.LoadMessages();
            if (txtChatlog.Text == oldText) return;
            txtChatlog.SelectionStart = txtChatlog.Text.Length;
            txtChatlog.ScrollToCaret();
        }

        private void HandleInput()
        {
            if (txtMessage.Text.Length >= 1)
            {
                if (txtMessage.Text[0] != '/')
                {
                    ms.SendMessage(txtMessage.Text, userName);
                    txtMessage.Text = "";
                    RefreshChatWindow();
                }
                else
                {
                    string cmd = txtMessage.Text.Substring(1);
                    if (cmd == "time")
                    {
                        if (!String.IsNullOrEmpty(userName)) ms.SendMessage("[ " + userName + " hat den Befehl \"" + cmd + "\" ausgeführt. ]", "system");
                        ms.SendMessage("*" + userName + "'s Aktuelle Systemzeit: " + DateTime.Now.ToLongTimeString(), "command");
                    }
                    if (cmd == "version")
                    {
                        if (!String.IsNullOrEmpty(userName)) ms.SendMessage("[ " + userName + " hat den Befehl \"" + cmd + "\" ausgeführt. ]", "system");
                        ms.SendMessage("*" + userName + "'s Aktuelle Clientversion: " + Application.ProductVersion, "command");
                    }
                    if (cmd == "update")
                    {
                        if (!String.IsNullOrEmpty(userName)) ms.SendMessage("[ " + userName + " hat den Befehl \"" + cmd + "\" ausgeführt. ]", "system");
                        CheckUpdates();
                    }
                    txtMessage.Text = "";
                }
            }
        }

        private void CheckUpdates()
        {
            WebClient wc = new WebClient();
            XmlDocument doc = new XmlDocument();
            string xmlString = wc.DownloadString("http://www.nightking.org/rptm.api/currentVersion.xml");
            doc.LoadXml(xmlString.Substring(3));
            string currVer = "";
            foreach (XmlNode xn in doc.LastChild)
            {
                if (xn.Name == "version")
                {
                    currVer = xn.InnerText.Trim();
                }
            }
            if (Int32.Parse(currVer.Split('.')[3]) > Int32.Parse(Application.ProductVersion.Split('.')[3]))
            {
                if(DialogResult.Yes == MessageBox.Show("Wollen Sie auf die aktuellste Version "+currVer+" updaten?", "Update verfügbar", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    wc.DownloadFileAsync(new Uri("http://www.nightking.org/rptm.api/Updater.exe"), Application.StartupPath+"\\Updater.exe");
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                }
            }
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(Application.StartupPath + "\\Updater.exe");
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ms.SendMessage(txtMessage.Text, txtUsername.Text);
            txtMessage.Text = "";
        }
    }
}
