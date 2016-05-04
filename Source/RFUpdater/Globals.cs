using System.Collections.Generic;

namespace RFUpdater
{
    class Globals
    {
		/**
		 * These settings sets up the remote store url and folders.
		 * ServerURL is the server on which you will store your modules.
		 * ModuleDefinitionFolder is the folder on the remote server that holds the modules description files.
		 * ModuleFolder is the folder on the remote server that will hold the modules.
		 **/
        public static string ServerURL      = "http://ams-patches.racing-france.fr/";
        public static string RemoteModuleDefinitionFolder = "module_list";
        public static string RemoteModuleFolder = "modules";
		public static string LocalModuleDefinitionFolder = "modules";
        public static string BinaryName     = "binary.bin";
        public static string GameBasePath = "C:/";

		public static int ACTION_DOWNLOAD_DEFINITIONS = 0;
		public static int ACTION_DOWNLOAD_MODULES = 1;

        public static List<string>  ModuleNameList   = new List<string>();
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
