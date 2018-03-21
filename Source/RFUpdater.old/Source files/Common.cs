using Cyclic.Redundancy.Check;
using System;
using System.Diagnostics;
using System.IO;

namespace RFUpdater.Patcher.Source_files
{
    internal class Common
    {
        public static void ChangeStatus(Texts.Keys Key, params string[] Arguments)
        {
            Globals.toolStrip.Text = Texts.GetText(Key, Arguments);
        }

        public static void UpdateCompleteProgress(long Value)
        {
            if (Value < 0 || Value > 100)
                return;

            Globals.packageDownloader.completeProgress.Value = Convert.ToInt32(Value);
            Globals.packageDownloader.completeProgressText.Text = Texts.GetText(Texts.Keys.COMPLETEPROGRESS, Value);
        }

        public static void UpdateCurrentProgress(long Value, double Speed)
        {
            if (Value < 0 || Value > 100)
                return;

            Globals.packageDownloader.currentProgress.Value = Convert.ToInt32(Value);
            Globals.packageDownloader.currentProgressText.Text = Texts.GetText(Texts.Keys.CURRENTPROGRESS, Value, Speed.ToString("0.00"));
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

        public static void EnableStart()
        {
            Globals.packageDownloader.startAMS.Enabled = true;
            Globals.packageDownloader.selectMods.Enabled = true;
            Globals.packageDownloader.quit.Enabled = true;
        }

        public static bool IsGameRunning()
        {
            Process[] pname = Process.GetProcessesByName(Globals.BinaryName);
            if (pname.Length == 0)
            {
                return false;
            }
            else
                return true;
        }
    }
}