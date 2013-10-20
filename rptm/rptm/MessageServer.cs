using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Linq;

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
                    string debugInfo = wClient.DownloadString(apiString + "sendMessage.php?user=" + user + "&message=" + message.To64());
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString(), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string LoadMessages()
        {
            if (!offline)
            {
                string output = "";
                WebClient wClient = new WebClient();
                try
                {
                    string messages = wClient.DownloadString(apiString + "getMessages.php");
                    foreach (string row in messages.Split('\n'))
                    {
                        string[] msg = row.Split(':');
                        if (msg.Length == 1)
                        {
                            output += msg[0].From64() + "\r\n";
                        }
                        else if (msg.Length == 2)
                        {
                            output += msg[0];
                            output += ":" + msg[1].From64() + "\r\n";
                        }
                    }
                    return output;
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString(), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ex.ToString();
                }
            }

            return ""; //Just to make the compiler happy
        }
    }
}
