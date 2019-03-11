using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading.Tasks;

namespace PageWatcher.Tasks
{
    public class ServiceTask
    {
        public static bool TicketsAvaliable { get; set; }

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

        public async void Start()
        {
            var task = Task.Run(() =>
            {
                while (true)
                {
                    LoadPage();
                    Task.Delay(20000);
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
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "Not Found";
            }

        }

        private void LoadPage()
        {
            Console.WriteLine("LoadPage");
            using (WebClient webClient = new WebClient())
            {
                string body = webClient.DownloadString(new Uri(ResourceURL));
                ParseBody(body);
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
                var json = webClient.DownloadString(url);
            }
        }
    }
}