using Microsoft.AspNetCore.Mvc;

namespace Taak.Controllers.Customer
{
    public class CustomerCMSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
