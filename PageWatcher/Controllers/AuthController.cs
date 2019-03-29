using PageWatcher.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PageWatcher.Utils;

namespace PageWatcher.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var adminEmail = ConfigUtils.ReadSetting("adminEmail");
            var adminPassword = ConfigUtils.ReadSetting("adminPassword");
            // Don't do this in production!
            if (model.Email == adminEmail && model.Password == adminPassword)
            {
                var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, "Admin"),
                        new Claim(ClaimTypes.Email, adminEmail),
                        new Claim(ClaimTypes.Country, "Russia")
                    },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

    }
}
