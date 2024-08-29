using CalisanTakip.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CalisanTakip.Controllers
{
    public class YoneticiController : Controller
    {
       
        
        public IActionResult Index()

        {   
            var personelAdSoyad = HttpContext.Session.GetString("PersonelAdSoyad");
            var personelId = HttpContext.Session.GetInt32("PersonelId");
            var personelBirimId = HttpContext.Session.GetInt32("PersonelBirimId");
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");
            
            if (personelYetkiTurID == 1)
            {
                ViewBag.PersonelAdSoyad = personelAdSoyad;
                ViewBag.PersonelId = personelId;
                ViewBag.PersonelBirimId = personelBirimId;
                ViewBag.PersonelYetkiTurID = personelYetkiTurID;


                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }

            
        }
    }
}
