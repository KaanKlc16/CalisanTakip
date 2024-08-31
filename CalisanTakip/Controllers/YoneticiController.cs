using CalisanTakip.Models;
using CalisanTakip.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CalisanTakip.Controllers
{  //IsTakipDbContext Entity = new IsTakipDbContext(); böyle tanımlamıştım en başta
    public class YoneticiController : Controller
    {
        private readonly IsTakipDbContext _context;
       
        public YoneticiController()
        {
            _context = new IsTakipDbContext();
        }

        public IActionResult Index()
        {
            var personelAdSoyad = HttpContext.Session.GetString("PersonelAdSoyad");
            var personelId = HttpContext.Session.GetInt32("PersonelId");
            var personelBirimId = HttpContext.Session.GetInt32("PersonelBirimId");
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");

            var birimAd = _context.Birimlers
                .Where(b => b.BirimId == personelBirimId)
                .Select(b => b.BirimAd)
                .FirstOrDefault();

            if (personelYetkiTurID == 1)
            {
                ViewBag.PersonelAdSoyad = personelAdSoyad;
                ViewBag.PersonelId = personelId;
                ViewBag.PersonelBirimId = personelBirimId;
                ViewBag.PersonelYetkiTurID = personelYetkiTurID;
                ViewBag.BirimAd = birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Ata()
        {
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");

            if (personelYetkiTurID == 1)
            {
                var birimId = HttpContext.Session.GetInt32("PersonelBirimId");

                var calisanlar = _context.Personellers
                    .Where(p => p.PersonlBirimId == birimId && p.PersonelYetkiTurId == 2)
                    .ToList();

                ViewBag.personeller = calisanlar;

                var birimAd = _context.Birimlers
                    .Where(b => b.BirimId == birimId)
                    .Select(b => b.BirimAd)
                    .FirstOrDefault();

                ViewBag.BirimAd = birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult Ata(Isler model)
        {
            if (!ModelState.IsValid)
            {
                var birimId = HttpContext.Session.GetInt32("PersonelBirimId");

                var calisanlar = _context.Personellers
                    .Where(p => p.PersonlBirimId == birimId && p.PersonelYetkiTurId == 2)
                    .ToList();

                ViewBag.personeller = calisanlar;

                var birimAd = _context.Birimlers
                    .Where(b => b.BirimId == birimId)
                    .Select(b => b.BirimAd)
                    .FirstOrDefault();

                ViewBag.BirimAd = birimAd;

                return View(model);
            }
            /* Böyle kullanmak yerine model olarak kullanmak yerine model. kullandım
             
                Isler yeniIs = new Isler
            {
                IsBaslik = isBaslik,
                IsAciklama = isAciklama,
                IsPersonelId = (int)secilenPersonelId,
                IletilenTarih = DateTime.Now,
                IsDurumId = 1
            };
             
             */
            model.IletilenTarih = DateTime.Now;
            model.IsDurumId = 1;

            _context.Islers.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Takip", "Yonetici");
        }

    }
}