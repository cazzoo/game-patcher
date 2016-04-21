using System;
using System.Diagnostics;
using System.IO;

namespace RFUpdater.Utils
{
    class ApplicationStarter
    {
        public static void Start()
        {
            if (!File.Exists(Globals.BinaryName))
            {
				MainWindow.statusbar.Push(1, Texts.GetText(Texts.Keys.MISSINGBINARY, Globals.BinaryName));
                return;
            }

            try
            {
                Process startProcess = new Process();
                startProcess.StartInfo.FileName = Globals.BinaryName;
                startProcess.StartInfo.UseShellExecute = false;
                startProcess.Start();
            }
            catch
            {
				Common.ShowMessageBox(Texts.Keys.CANNOTSTART, Globals.BinaryName);
            }
        }
    }
}
