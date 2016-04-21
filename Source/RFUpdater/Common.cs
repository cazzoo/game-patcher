using Cyclic.Redundancy.Check;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gtk;

namespace RFUpdater
{
    class Common
    {
		public static void ShowMessageBox(Texts.Keys Key, params string[] Arguments) 
		{
			MessageDialog MessageBox = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, Texts.GetText (Key, Arguments));
			MessageBox.Run ();
			MessageBox.Destroy ();
			Application.Quit ();
		}

        public static void ChangeStatus(Texts.Keys Key, params string[] Arguments)
        {
			MainWindow.statusbar.Push(1, Texts.GetText(Key, Arguments));
        }

        public static void UpdateCompleteProgress(long Value)
        {
            if (Value < 0 || Value > 100)
                return;

			MainWindow.progressOverall.Fraction = Value / 100;
			MainWindow.progressOverall.Text = Texts.GetText(Texts.Keys.COMPLETEPROGRESS, Value);
        }

        public static void UpdateCurrentProgress(long Value, double Speed)
        {
            if (Value < 0 || Value > 100)
                return;

			MainWindow.progressFile.Fraction = Value / 100;
			MainWindow.progressFile.Text = Texts.GetText(Texts.Keys.CURRENTPROGRESS, Value, Speed.ToString("0.00"));
        }

        public static string GetHash(string Name)
        {
            if (Name == string.Empty)
                return string.Empty;

            CRC crc = new CRC();

            string Hash = string.Empty;

            using (FileStream fileStream = File.Open(Name, FileMode.Open))
            {
                foreach (byte b in crc.ComputeHash(fileStream))
                {
                    Hash += b.ToString("x2").ToLower();
                }
            }

            return Hash;
        }

        public static bool IsGameRunning()
        {
            return Process.GetProcessesByName(Globals.BinaryName).FirstOrDefault(p => p.MainModule.FileName.StartsWith("")) != default(Process);
        }
    }
}
