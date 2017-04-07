using System.Collections.Generic;
using System.Windows.Forms;

namespace RFUpdater.Patcher.Source_files
{
    class Globals
    {
        public static string ServerURL      = "http://ams-patches.racing-france.fr/";
        public static string SelectedPackageFolder  = string.Empty;
        public static string BinaryName     = "AMS";

        public static string selectedPackage = string.Empty;

        public static PackageDownloader packageDownloader;
        public static PackageSelector packageSelector;
        public static ToolStripStatusLabel toolStrip = null;

        public static List<File>    Files    = new List<File>();
        public static List<string>  OldFiles = new List<string>();

        public static long FullSize;
        public static long CompleteSize;

        public struct File
        {
            public string Name;
            public string Hash;
            public long   Size;
        }
    }
}
