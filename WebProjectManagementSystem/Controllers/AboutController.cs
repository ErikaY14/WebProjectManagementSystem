using Microsoft.AspNetCore.Mvc;

namespace WebProjectManagementSystem.Controllers
{
    public class AboutController : Controller
    {
        // GET: About/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
