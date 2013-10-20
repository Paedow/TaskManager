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
                    offline = true;
                    MessageBox.Show("Die Verbindung zum Internet konnte nicht hergestellt werden!", "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public string LoadMessages()
        {
            if (!offline)
            {
                WebClient wClient = new WebClient();
                try
                {
                    var r = wClient.DownloadString(apiString + "getMessages.php");
                    List<string> list = new List<string>();
                    foreach (string row in r.Replace("\r", "").Split('\n'))
                    {
                        string[] b = row.Split(':');
                        var v = ((b.Count() > 1) ? String.Join(":", b.Except(new[] { b.First() }).ToArray()).From64() : "");
                        string s = string.Format("{0}:{1}\n", b.First(), v);
                        list.Add(s);
                    }
                    return r.Contains(":") ? String.Join("\n", list.ToArray()).Replace("\n", "\n\r") : r.From64();
                }
                catch (WebException ex)
                {
                    offline = true;
                    MessageBox.Show("Die Verbindung zum Internet konnte nicht hergestellt werden!", "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ex.ToString();
                }
            }

            return ""; //Just to make the compiler happy
        }
    }
}
