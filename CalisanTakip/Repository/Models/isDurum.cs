namespace CalisanTakip.Repository.Models
{
    public class IsDurumModel
    {
        public List<IsDurum> isDurumlar { get; set; } = new List<IsDurum>();
    }

    public class IsDurum
    {
        public string? isPersonel { get; set; }
        public string? isAciklama { get; set; }
        public string? isBaslik { get; set; }
        public DateTime? iletilenTarih { get; set; }
        public DateTime? yapilanTarih { get; set; }

        public string? isYorum { get; set; }

        public int tahminiSure { get; set; }
        public string? durumAd { get; set; }
        public DateTime? isBaslangic { get; set; } 
        public DateTime? isBitirmeSure { get; set; } 

    }
}
