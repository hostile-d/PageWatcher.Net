using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using PageWatcher.Models;
using System.Collections.Generic;
using System.Xml;

namespace PageWatcher.Controllers
{
    public class LoggerController : Controller
    {
        public ActionResult Index()
        {
            XmlDocument NLogConfig = new XmlDocument();

            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentDirectory, "Logs", "Log.json");

            var model = new List<LogItemViewModel>();
            using (StreamReader file = System.IO.File.OpenText(filePath))
            {
                while (!file.EndOfStream)
                {
                    var json = file.ReadLine();
                    if (!String.IsNullOrEmpty(json))
                    {
                        model.Add(JsonConvert.DeserializeObject<LogItemViewModel>(json));
                    }
                }
            }

            return View(model);
        }
    }
}