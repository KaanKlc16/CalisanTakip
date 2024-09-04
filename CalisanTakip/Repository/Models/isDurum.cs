﻿namespace CalisanTakip.Repository.Models
{
    public class IsDurumModel
    {
        public List<IsDurum> isDurumlar { get; set; } = new List<IsDurum>();
    }

    public class IsDurum
    {
        public string? isAciklama { get; set; }
        public string? isBaslik { get; set; }
        public DateTime? iletilenTarih { get; set; }
        public DateTime? yapilanTarih { get; set; }

        public string? isYorum { get; set; }

        public DateTime? tahminiSure { get; set; }
        public string? durumAd { get; set; }
    }
}
