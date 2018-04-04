using Newtonsoft.Json;
using RFUpdater.Models.Exceptions;
using System;
using System.IO;

namespace RFUpdater.Models
{
    public class Utils
    {
        public static Mod ParseModFile(string fileName)
        {
            Mod mod;
            string stringifiedMod;
            using (StreamReader file = File.OpenText(fileName))
            {
                stringifiedMod = file.ReadToEnd();
            }
            try
            {
                mod = JsonConvert.DeserializeObject<Mod>(stringifiedMod);
                mod.ModStorePath = Directory.GetParent(fileName).Parent.FullName;
                return mod;
            }
            catch (Exception ex)
            {
                throw new InvalidFormatException("Error while parsing the mod file. Content is invalid.", ex);
            }
        }
    }
}