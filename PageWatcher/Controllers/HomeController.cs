using System;
using System.Web.Mvc;
using PageWatcher.Models;
using PageWatcher.Tasks;
using PageWatcher.Utils;

namespace PageWatcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var Url = new Uri(ConfigUtils.ReadSetting("ResourceURL"));
            var model = new HomePageViewModel
            {
                TicketsAvaliable = ServiceTask.TicketsAvaliable,
                Url = Url
            };

            return View(model);
        }
    }
}