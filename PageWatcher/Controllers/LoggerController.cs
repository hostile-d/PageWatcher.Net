using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PageWatcher.Models;


namespace PageWatcher.Controllers
{
    public class LoggerController : Controller
    {
        public ActionResult Index()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentDirectory, "Logs", "InfoLog.json");

            using (StreamReader file = System.IO.File.OpenText(filePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject parsedData = (JObject)JToken.ReadFrom(reader);
                var model = new LoggerViewModel
                {
                    json = parsedData
                };
                return View(model);
            };
        }
    }
}