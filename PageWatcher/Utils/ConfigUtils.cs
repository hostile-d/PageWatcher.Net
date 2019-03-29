using System.Configuration;

namespace PageWatcher.Utils
{
    public class ConfigUtils
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var result = appSettings[key];
                return result;
            }
            catch (ConfigurationErrorsException e)
            {
                logger.Error(e, "Error reading app settings");
                throw;
            }

        }
    }
}