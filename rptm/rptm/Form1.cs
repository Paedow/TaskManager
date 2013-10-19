using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rptm
{
    public partial class Form1 : Form
    {
        MessageServer ms = new MessageServer("http://www.nightking.org/rptm.api/");
        private string userName = "";
        public Form1()
        {
            InitializeComponent();
            string test = textBox1.Text;
            refreshTimer.Start();
            Text = Application.ProductVersion;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (textBox3.Text.Length >= 3)
                {
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    button1.Enabled = true;
                    textBox3.Enabled = false;
                    userName = textBox3.Text;
                    button2.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshChatWindow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length >= 3)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                textBox3.Enabled = false;
                userName = textBox3.Text;
                button2.Enabled = false;
                ms.SendMessage(userName + " hat sich angemeldet.", "system");
                RefreshChatWindow();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(userName)) ms.SendMessage(userName + " hat sich abgemeldet.", "system");
        }

        private void RefreshChatWindow()
        {
            string oldText = textBox1.Text;
            textBox1.Text = ms.LoadMessages();
            if (textBox1.Text == oldText) return;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void HandleInput()
        {
            if (textBox2.Text.Length >= 1)
            {
                if (textBox2.Text[0] != '/')
                {
                    ms.SendMessage(textBox2.Text, userName);
                    textBox2.Text = "";
                    RefreshChatWindow();
                }
                else
                {
                    string cmd = textBox2.Text.Substring(1);
                    if (!String.IsNullOrEmpty(userName)) ms.SendMessage(userName + " hat den Befehl \"" + cmd + "\" ausgeführt.", "system");
                    if (cmd == "time")
                    {
                        ms.SendMessage(userName + "\\'s Aktuelle Systemzeit: " + DateTime.Now.ToLongTimeString(), "command");
                    }
                    if (cmd == "version")
                    {
                        ms.SendMessage(userName + "\\'s Aktuelle Clientversion: " + Application.ProductVersion, "command");
                    }
                    textBox2.Text = "";
                }
            }
        }
    }
}
