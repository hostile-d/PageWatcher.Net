using PageWatcher.Models;
using PageWatcher.Tasks;
using System.Web.Mvc;

namespace PageWatcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var model = new PageViewModel
            {
                TicketsAvaliable = ServiceTask.TicketsAvaliable
            };

            return View(model);
        }
    }
}