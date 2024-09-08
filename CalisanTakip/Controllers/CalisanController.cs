using CalisanTakip.Repository;
using CalisanTakip.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Controllers
{
    

    public class CalisanController : Controller
    {
        private readonly IsTakipDbContext _context;

        public CalisanController()
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

            if (personelYetkiTurID == 2)
            {
                ViewBag.PersonelAdSoyad = personelAdSoyad;
                ViewBag.PersonelId = personelId;
                ViewBag.PersonelBirimId = personelBirimId;
                ViewBag.PersonelYetkiTurID = personelYetkiTurID;
                ViewBag.BirimAd = birimAd;

                var isler = _context.Islers
     .Where(i => i.IsPersonelId == personelId && i.IsOkunma == false)
     .OrderByDescending(i => i.IletilenTarih)
     .ToList();

                ViewBag.Isler = isler;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Calisan");
            }
        }
        [HttpPost]
        public IActionResult Index(int isId)
        {
            var tekIs = _context.Islers
                .FirstOrDefault(i => i.IsId == isId);

            if (tekIs != null)
            {
                tekIs.IsOkunma = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Yap", "Calisan");
        }
        public IActionResult GetCalendarEvents()
        {
            var birimId = HttpContext.Session.GetInt32("PersonelBirimId");
            // İşlerinizi alıp IsDurum modeline dönüştürün
            var isDurumlar = _context.Islers
                 .Include(i => i.IsPersonel)  // Personeller tablosunu dahil ediyoruz
                 .Include(i => i.IsDurum)     // Durumlar tablosunu dahil ediyoruz
                 .Where(i => i.IsPersonel.PersonlBirimId == birimId)
                 .Select(i => new
                 {
                     i.IsId,                // IsId'yi burada alıyoruz
                     i.IsBaslik,
                     i.IsAciklama,
                     i.IsBaslangic,
                     i.IsBitirmeSure,
                     personelAdSoyad = i.IsPersonel.PersonelAdSoyad,
                     i.TahminiSure

                 })
                 .ToList();


            // Takvim olayları için gerekli verileri hazırlıyoruz
            var events = isDurumlar.Select(d => new
            {
                id = d.IsId,  // Burada IsId'yi kullanıyoruz
                calendarId = "1",
                title = d.IsBaslik + " - " + d.personelAdSoyad,
                category = "time",
                start = d.IsBaslangic?.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = d.IsBitirmeSure?.ToString("yyyy-MM-ddTHH:mm:ss"),

                description = d.IsAciklama + " - Tahmini Süre: " + d.TahminiSure,
                PersonelAdSoyad = d.personelAdSoyad
            });


            return Json(events);  // JSON formatında döndürüyoruz
        }

        public IActionResult Yap()
        {
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");

            if (personelYetkiTurID == 2)
            {
                var personelId = HttpContext.Session.GetInt32("PersonelId");
                var isler = _context.Islers
                    .Where(i => i.IsPersonelId == personelId && i.IsDurumId == 1)
                    .OrderByDescending(i => i.IletilenTarih)
                    .ToList();

                ViewBag.isler = isler;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Yap(int isId,string isYorum, int tahminiSureSaat, int tahminiSureDakika)
        {
            var tekIs = _context.Islers
                .Where(i => i.IsId == isId)
                .FirstOrDefault();

            if (tekIs != null)
            {
                if (isYorum == "") isYorum = "Çalışan Yorum Yapmadı";
                tekIs.YapilanTarih = DateTime.Now;
                tekIs.IsDurumId = 2;

                TimeSpan tahminiSure = new TimeSpan(tahminiSureSaat, tahminiSureDakika, 0);
                tekIs.TahminiSure = (int)tahminiSure.TotalMinutes;



                tekIs.IsYorum = isYorum;
                
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    
                    Console.WriteLine(ex.InnerException?.Message);
                    throw; 
                }

            }

            return RedirectToAction("Index", "Calisan");
        }

        public IActionResult Takip()
        {
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");

            if (personelYetkiTurID == 2)
            {
                var personelId = HttpContext.Session.GetInt32("PersonelId");
                

                var isler = (from i in _context.Islers
                              join d in _context.Durumlars on i.IsDurumId equals d.DurumId
                              where i.IsPersonelId == personelId
                              orderby i.IletilenTarih descending
                              select new
                              {
                                  i.IsBaslik,
                                  i.IsAciklama,
                                  i.IletilenTarih,
                                  i.YapilanTarih,
                                   d.DurumAd,
                                   i.IsYorum
                              }).ToList();


                IsDurumModel model = new IsDurumModel();
                 
                foreach (var i in isler)
                {
                    IsDurum isDurum = new IsDurum
                    {
                        isBaslik = i.IsBaslik,
                        isAciklama = i.IsAciklama,
                        iletilenTarih = i.IletilenTarih,
                        yapilanTarih = i.YapilanTarih,
                        durumAd = i.DurumAd,
                        isYorum = i.IsYorum


                    };
                    model.isDurumlar.Add(isDurum);
                }

                ViewBag.isler = isler;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
