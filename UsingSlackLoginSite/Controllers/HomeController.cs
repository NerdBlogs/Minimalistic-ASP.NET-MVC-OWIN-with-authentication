using System.Web.Mvc;

namespace UsingSlackLoginSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }
    }
}