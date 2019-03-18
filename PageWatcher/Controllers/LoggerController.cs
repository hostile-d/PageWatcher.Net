using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PageWatcher.Models;
using System.Collections.Generic;

namespace PageWatcher.Controllers
{
    public class LoggerController : Controller
    {
        public ActionResult Index()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentDirectory, "Logs", "InfoLog.json");

            var model = new List<InfoLogItemViewModel>();
            using (StreamReader file = System.IO.File.OpenText(filePath))
            {
                while (!file.EndOfStream)
                {
                    var json = file.ReadLine();
                    model.Add(JsonConvert.DeserializeObject<InfoLogItemViewModel>(json));
                }
            }

            return View(model);
        }
    }
}