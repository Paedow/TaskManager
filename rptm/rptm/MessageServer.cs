using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;

namespace rptm
{
    internal class MessageServer
    {
        bool offline = false;
        string apiString = "";
        public MessageServer(string serverAdress)
        {
            apiString = serverAdress;
        }

        public void SendMessage(string message, string user)
        {
            if (!offline)
            {
                WebClient wClient = new WebClient();
                try
                {
                    string debugInfo = wClient.DownloadString(apiString + "sendMessage.php?user=" + user + "&message=" + HttpUtility.UrlEncode(message));
                }
                catch (WebException ex)
                {
                    offline = true;
                    MessageBox.Show("Die Verbindung zum Internet konnte nicht hergestellt werden!", "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string LoadMessages()
        {
            string lol = "";
            if (!offline)
            {
                WebClient wClient = new WebClient();
                try
                {
                    lol = wClient.DownloadString(apiString + "getMessages.php");
                }
                catch (WebException ex)
                {
                    offline = true;
                    MessageBox.Show("Die Verbindung zum Internet konnte nicht hergestellt werden!", "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return lol;
        }
    }
}
