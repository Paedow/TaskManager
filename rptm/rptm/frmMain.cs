using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            Text = Application.ProductVersion;
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
                if (txtUsername.Text.Length >= 3)
                {
                    txtChatlog.Enabled = true;
                    txtMessage.Enabled = true;
                    cmdSend.Enabled = true;
                    txtUsername.Enabled = false;
                    userName = txtUsername.Text;
                    cmdLogin.Enabled = false;
                    ms.SendMessage(userName + " hat sich angemeldet.", "system");
                    RefreshChatWindow();
                }
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
            if (txtUsername.Text.Length >= 3)
            {
                txtChatlog.Enabled = true;
                txtMessage.Enabled = true;
                cmdSend.Enabled = true;
                txtUsername.Enabled = false;
                userName = txtUsername.Text;
                cmdLogin.Enabled = false;
                ms.SendMessage(string.Format("[ {0} hat sich angemeldet. ]", userName), "system");
                RefreshChatWindow();
            }
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
                    if (!String.IsNullOrEmpty(userName)) ms.SendMessage(userName + " hat den Befehl \"" + cmd + "\" ausgeführt.", "system");
                    if (cmd == "time")
                    {
                        ms.SendMessage(userName + "\\'s Aktuelle Systemzeit: " + DateTime.Now.ToLongTimeString(), "command");
                    }
                    if (cmd == "version")
                    {
                        ms.SendMessage(userName + "\\'s Aktuelle Clientversion: " + Application.ProductVersion, "command");
                    }
                    txtMessage.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ms.SendMessage(txtMessage.Text, txtUsername.Text);
            txtMessage.Text = "";
        }
    }
}
