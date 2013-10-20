using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Updater
{
    static class Program
    {
        static void Main()
        {
            Thread.Sleep(2500);
            File.Delete(Application.StartupPath + "\\rptm.exe");
            byte[] b = Properties.Resources.rptm;
            File.WriteAllBytes(Application.StartupPath + "\\rptm.exe", b);
            Process.Start(Application.StartupPath + "\\rptm.exe");
            return;
        }
    }
}