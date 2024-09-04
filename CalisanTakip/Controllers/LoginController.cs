using Microsoft.AspNetCore.Mvc;
using CalisanTakip.Models;
using CalisanTakip.Repository;
using Microsoft.AspNetCore.Http;

namespace CalisanTakip.Controllers
{
    public class LoginController : Controller
    {
        IsTakipDbContext Entity = new IsTakipDbContext();

        public IActionResult Index()
        {
            ViewBag.mesaj = null;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string kullaniciAd, string parola)
        {
            // BURAYA PASSWORD HASHER GELECEK!!!!!!!!!!!!!!!!
            var personel= Entity.Personellers
                            .FirstOrDefault(p => p.PersonelKullaniciAd == kullaniciAd && p.PersonelParola == parola);

            if (personel != null)//personel null değilse giriş yapabilir nullsa giremez 
            {
                HttpContext.Session.SetString("PersonelAdSoyad", personel.PersonelAdSoyad ?? string.Empty);
                HttpContext.Session.SetInt32("PersonelId", personel.PersonelId);
                HttpContext.Session.SetInt32("PersonelBirimId", personel.PersonlBirimId ?? 0);
                HttpContext.Session.SetInt32("PersonelYetkiTurID", personel.PersonelYetkiTurId ?? 0);
                switch(personel.PersonelYetkiTurId)
                {
                    case 1:
                        return RedirectToAction("Index", "Yonetici");
                    case 2:
                        return RedirectToAction("Index", "Calisan");
                    default:
                        ViewBag.mesaj = "Geçersiz yetki türü.";
                        return View();
                        




                }

            }
            else
            {
                ViewBag.mesaj = "Kullanıcı adı veya parola yanlış";
                return View();
            }
        }
    }
}
