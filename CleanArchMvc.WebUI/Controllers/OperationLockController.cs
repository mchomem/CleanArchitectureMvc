using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.WebUI.Controllers
{
    public class OperationLockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
