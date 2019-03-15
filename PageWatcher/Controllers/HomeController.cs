using System.Web.Mvc;
using PageWatcher.Models;
using PageWatcher.Tasks;

namespace PageWatcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var model = new HomePageViewModel
            {
                TicketsAvaliable = ServiceTask.TicketsAvaliable
            };

            return View(model);
        }
    }
}