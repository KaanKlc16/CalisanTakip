﻿using CalisanTakip.Models;
using CalisanTakip.Repository;
using CalisanTakip.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CalisanTakip.Controllers
{
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

            model.IsOkunma = false;
            model.IletilenTarih = DateTime.Now;
            model.IsDurumId = 1;

            _context.Islers.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Takip", "Yonetici");
        }

        public IActionResult Takip()
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
                RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Takip(int selectPer)
        {
            var secilenPersonel = _context.Personellers
                                   .FirstOrDefault(p => p.PersonelId == selectPer);

            HttpContext.Session.SetString("SecilenPersonel", JsonConvert.SerializeObject(secilenPersonel));

            return RedirectToAction("Listele", "Yonetici");
        }

        [HttpGet]
        public IActionResult Listele()
        {
            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");
            var secilenPersonelJson = HttpContext.Session.GetString("SecilenPersonel");
            var secilenPersonel = JsonConvert.DeserializeObject<Personeller>(secilenPersonelJson);

            if (personelYetkiTurID == 1)
            {
                try
                {
                    var isler = _context.Islers
                    .Where(i => i.IsPersonelId == secilenPersonel.PersonelId)
                    .OrderByDescending(i => i.IletilenTarih)
                    .ToList();

                    ViewBag.Isler = isler;
                    ViewBag.Personeller = secilenPersonel;
                    ViewBag.IsSayisi = isler.Count();

                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Takip", "Yonetici");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult GetCalendarEvents()
        {
            var personelBirimId = HttpContext.Session.GetInt32("PersonelBirimId");

            if (personelBirimId == null)
            {
                return Json(new { success = false, message = "Personel Birim ID bulunamadı." });
            }

            var isDurumlar = _context.Islers
                .Include(i => i.IsPersonel)
                .Include(i => i.IsDurum)
                .Where(i => i.IsPersonel.PersonlBirimId == personelBirimId)
                .Select(i => new
                {
                    i.IsId,
                    i.IsBaslik,
                    i.IsAciklama,
                    i.IsBaslangic,
                    i.IsBitirmeSure,
                    personelAdSoyad = i.IsPersonel.PersonelAdSoyad,
                    i.TahminiSure
                })
                .ToList();

            var events = isDurumlar.Select(d => new
            {
                id = d.IsId,
                calendarId = "1",
                title = $"{d.IsBaslik} - {d.personelAdSoyad}",
                category = "time",
                start = d.IsBaslangic.HasValue ? d.IsBaslangic.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                end = d.IsBitirmeSure.HasValue ? d.IsBitirmeSure.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                description = d.IsAciklama,
                PersonelAdSoyad = d.personelAdSoyad,
                tahminiSure = $"{d.TahminiSure} saat"
            });

            return Json(events);
        }

        [HttpPost]
        public IActionResult UpdateEvent([FromBody] TakvimGuncelle model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Model verisi alınamadı." });
            }

            var isler = _context.Islers.FirstOrDefault(i => i.IsId == model.Id);

            if (isler == null)
            {
                return Json(new { success = false, message = "Görev bulunamadı." });
            }

            
            if (model.Start != DateTime.MinValue)
            {
                isler.IsBaslangic = model.Start;
            }


            if (model.End != DateTime.MinValue)
            {
                isler.IsBitirmeSure = model.End;
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Görev başarıyla güncellendi." });
        }

    }
}
