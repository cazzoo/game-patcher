using System.Collections.Generic;
using System.Windows.Forms;

namespace RFUpdater.Patcher.Source_files
{
    class Globals
    {
        public static string ServerURL      = "http://ams-patches.racing-france.fr/";
        public static string ModulesListFolder = "module_list";
        public static string ModulesFolder = "modules";
        public static string BinaryName     = "binary.bin";
        public static string GameBasePath = "C:/";

        public static string NETWORK_DOWNLOAD_LIST = "LIST_DOWNLOAD";
        public static string NETWORK_DOWNLOAD_FILES = "FILE_DOWNLOAD";

        public static Form currentForm;
        public static loginForm loginForm;
        public static pForm pForm;
        public static moduleList moduleList;

        public static List<string>  Patchlist   = new List<string>();
        public static List<File>    Files       = new List<File>();
        public static List<string>  OldFiles    = new List<string>();

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
