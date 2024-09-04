using Microsoft.AspNetCore.Mvc;

namespace CalisanTakip.Controllers
{
    public class LogOut : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Login");
        }
    }
}
