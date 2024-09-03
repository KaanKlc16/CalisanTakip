using CalisanTakip.Repository;
using CalisanTakip.Repository.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalisanTakip.Controllers
{ 
    public class IsDurum
    {
        public String isAciklama { get; set; }
        public String isBaslik { get; set; }

        public DateTime? iletilenTarih { get; set; } 

        public DateTime? yapilanTarih { get; set; }
        
        public String durumAd { get; set; }



    }
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

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
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
        public IActionResult Yap(int isId)
        {
            var tekIs = _context.Islers
                .Where(i => i.IsId == isId)
                .FirstOrDefault();

            if (tekIs != null)
            {
                tekIs.YapilanTarih = DateTime.Now;
                tekIs.IsDurumId = 2;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Calisan");
        }



        public IActionResult Takip() {

            var personelYetkiTurID = HttpContext.Session.GetInt32("PersonelYetkiTurID");

            if (personelYetkiTurID == 2)
            {
                var personelId = HttpContext.Session.GetInt32("PersonelId");
                var isler = _context.Islers
                    .Where(i => i.IsPersonelId == personelId && i.IsDurumId == 1)
                    .ToList()
                    .OrderByDescending(i=>i.IletilenTarih);

                IsDurumModel model = new IsDurumModel();

                List<IsDurum>list= new List<IsDurum>();

                foreach(var i in isler)
                {
                    IsDurumModel isDurum = new IsDurumModel();

                    isDurum.isBaslik = i.IsBaslik;
                    isDurum.isAciklama=i.IsAciklama

                }

                ViewBag.isler = isler;
                return View(isler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}
