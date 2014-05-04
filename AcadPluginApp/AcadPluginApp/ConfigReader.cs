using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace AcadPluginApp
{
    class ConfigReader
    {
        //static private string Username = ReadConfig("OpenApiUsername");
        //static private string Password = ReadConfig("OpenApiPassword");
        //static private string ConsumerKey = ReadConfig("identityOAuthConsumerKey");
        //static private string ConsumerSecret = ReadConfig("identityOAuthConsumerSecret");
        //static private string OpenApiUri = ReadConfig("OpenAPiUri");

        public static string ReadConfig(string key)
        {
            Assembly assembly;
            ExeConfigurationFileMap map;
            Uri uri;
            map = new ExeConfigurationFileMap();
            assembly = Assembly.GetCallingAssembly();
            uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            map.ExeConfigFilename = Path.Combine(uri.LocalPath, assembly.GetName().Name + ".dll.config");
            string str = ConfigurationManager.OpenMappedExeConfiguration(map, 0).AppSettings.Settings[key].Value;
            return str;

        }




    }
}
