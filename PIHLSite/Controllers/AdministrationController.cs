using Microsoft.AspNetCore.Mvc;

namespace PIHLSite.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
