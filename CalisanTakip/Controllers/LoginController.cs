using Microsoft.AspNetCore.Mvc;
using CalisanTakip.Models;
using CalisanTakip.Repository;

namespace CalisanTakip.Controllers
{
    public class LoginController : Controller
    {
        IsTakipDbContext Entity = new IsTakipDbContext();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string kullaniciAd, string parola)
        {
            // BURAYA PASSORD HASHER GELECEK!!!!!!!!!!!!!!!!
            var personeller= Entity.Personellers
                            .FirstOrDefault(u => u.PersonelKullaniciAd == kullaniciAd && u.PersonelParola == parola);

            if (personeller != null)
            {
                return RedirectToAction("", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Kullanıcı adı veya parola yanlış";
                return View();
            }
        }
    }
}
