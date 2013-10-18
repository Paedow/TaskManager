using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace rptm
{
    internal class MessageServer
    {
        string apiString = "";
        public MessageServer(string serverAdress)
        {
            apiString = serverAdress;
        }

        public void SendMessage(string message, string user)
        {
            WebClient wClient = new WebClient();
            wClient.DownloadString(apiString + "sendMessage.php?user=" + user + "&message=" + message);
        }

        public string LoadMessages()
        {
            WebClient wClient = new WebClient();
            return wClient.DownloadString(apiString+"getMessages.php");
        }
    }
}
