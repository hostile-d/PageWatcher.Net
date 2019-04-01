using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PageWatcher.Utils;

namespace PageWatcher.Tasks
{
    public class ServiceTask
    {
        public static bool TicketsAvaliable { get; set; }
        public static string LastUpdateTime { get; set; }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string ResourceURL;
        private static string TargetCssClass;
        private static string SuccessText;
        private static string TelegramUrl;
        private static string TelegramBotToken;
        private static string TelegramGroupId;
        private static bool NotSentYet;
        public ServiceTask()
        {
            ResourceURL = ConfigUtils.ReadSetting("ResourceURL");
            TargetCssClass = ConfigUtils.ReadSetting("TargetCssClass");
            SuccessText = ConfigUtils.ReadSetting("SuccessText");
            TelegramUrl = ConfigUtils.ReadSetting("TelegramUrl");
            TelegramBotToken = ConfigUtils.ReadSetting("TelegramBotToken");
            TelegramGroupId = ConfigUtils.ReadSetting("TelegramGroupId");
            NotSentYet = true;
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

        private void LoadPage()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string body = webClient.DownloadString(new Uri(ResourceURL));
                    LastUpdateTime = DateTime.UtcNow.ToString("o"); ;
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
                if (NotSentYet)
                {
                    SendMessage(GetSuccessUrlString());
                    NotSentYet = false;
                }
            }
            else
            {
                TicketsAvaliable = false;
                NotSentYet = true;
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