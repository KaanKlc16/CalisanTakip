using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository.Models;

[Table("Isler")]
public partial class Isler
{
    [Key]
    [Column("isId")]
    public int IsId { get; set; }

    [Column("isBaslik")]
    public string? IsBaslik { get; set; }

    [Column("isAciklama")]
    public string? IsAciklama { get; set; }

    [Column("isPersonelId")]
    public int? IsPersonelId { get; set; }

    [Column("iletilenTarih", TypeName = "datetime")]
    public DateTime? IletilenTarih { get; set; }

    [Column("yapilanTarih", TypeName = "datetime")]
    public DateTime? YapilanTarih { get; set; }

    [Column("isDurumId")]
    public int? IsDurumId { get; set; }

    [Column("isYorum")]
    public string? IsYorum { get; set; }

    [Column("tahminiSure")]
    public int? TahminiSure { get; set; }
    [Column("isBaslangic")]
    public DateTime? IsBaslangic { get; set; }
    [Column("isBitirmeSure")]
    public DateTime? IsBitirmeSure { get; set; }

    [Column("isOkunma")]
    public bool? IsOkunma { get; set; }

    [ForeignKey("IsDurumId")]
    [InverseProperty("Islers")]
    public virtual Durumlar? IsDurum { get; set; }

    [ForeignKey("IsPersonelId")]
    [InverseProperty("Islers")]
    public virtual Personeller? IsPersonel { get; set; }
}
