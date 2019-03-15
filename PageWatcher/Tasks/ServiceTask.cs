using System;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PageWatcher.Tasks
{
    public class ServiceTask
    {
        public static bool TicketsAvaliable { get; set; }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string ResourceURL;
        private static string TargetCssClass;
        private static string SuccessText;
        private static string TelegramUrl;
        private static string TelegramBotToken;
        private static string TelegramGroupId;

        public ServiceTask()
        {
            ResourceURL = ReadSetting("ResourceURL");
            TargetCssClass = ReadSetting("TargetCssClass");
            SuccessText = ReadSetting("SuccessText");
            TelegramUrl = ReadSetting("TelegramUrl");
            TelegramBotToken = ReadSetting("TelegramBotToken");
            TelegramGroupId = ReadSetting("TelegramGroupId");
        }

        public void Start()
        {
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(20000);
                    LoadPage();
                }
            });
        }


        private static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                return result;
            }
            catch (ConfigurationErrorsException e)
            {
                logger.Error(e, "Error reading app settings");
                return "Not Found";
            }

        }

        private void LoadPage()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string body = webClient.DownloadString(new Uri(ResourceURL));
                    logger.Info("Resource URL was downloaded");
                    ParseBody(body);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }

        private void ParseBody(string body)
        {
            var regex = new Regex(@"" + TargetCssClass);
            Match match = regex.Match(body);
            if (match.Success)
            {
                TicketsAvaliable = true;
                SendMessage(GetSuccessUrlString());
            }
            else
            {
                TicketsAvaliable = false;
            }
        }

        private static string GetSuccessUrlString()
        {
            var ReplacedUrl = TelegramUrl.Replace("@TelegramBotToken", TelegramBotToken).Replace("@TelegramGroupId", TelegramGroupId);
            return ReplacedUrl + "&text=" + Uri.EscapeUriString(SuccessText);
        }

        private void SendMessage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    var json = webClient.DownloadString(url);
                    logger.Info("Message was sent via Telegram");
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }
    }
}