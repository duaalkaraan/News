using Microsoft.AspNetCore.Mvc;

namespace ornek.Areas.Admin.Controllers
{
    public class NewsRequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
