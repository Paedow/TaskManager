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
                if (textBox3.Text.Length >= 3)
                {
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    button1.Enabled = true;
                    textBox3.Enabled = false;
                    userName = textBox3.Text;
                    button2.Enabled = false;
                    ms.SendMessage("system", userName+" hat sich angemeldet.");
                    RefreshChatWindow();
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text.Length >= 1)
                {
                    ms.SendMessage(userName, textBox2.Text);
                    textBox2.Text = "";
                    RefreshChatWindow();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= 1)
            {
                ms.SendMessage(userName, textBox2.Text);
                textBox2.Text = "";
                RefreshChatWindow();
            }
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
                ms.SendMessage("system", userName + " hat sich angemeldet.");
                RefreshChatWindow();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!String.IsNullOrEmpty(userName)) ms.SendMessage("system", userName + " hat sich abgemeldet.");
        }

        private void RefreshChatWindow()
        {
            string oldText = textBox1.Text;
            textBox1.Text = ms.LoadMessages();
            if (textBox1.Text == oldText) return;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
    }
}
