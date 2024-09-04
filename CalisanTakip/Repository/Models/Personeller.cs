using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository.Models;

[Table("Personeller")]
public partial class Personeller
{
    [Key]
    [Column("personelId")]
    public int PersonelId { get; set; }

    [Column("personelAdSoyad")]
    [StringLength(50)]
    public string? PersonelAdSoyad { get; set; }

    [Column("personelKullaniciAd")]
    [StringLength(50)]
    public string? PersonelKullaniciAd { get; set; }

    [Column("personelParola")]
    [StringLength(50)]
    public string? PersonelParola { get; set; }

    [Column("personlBirimId")]
    public int? PersonlBirimId { get; set; }

    [Column("personelYetkiTurId")]
    public int? PersonelYetkiTurId { get; set; }

    [InverseProperty("IsPersonel")]
    public virtual ICollection<Isler> Islers { get; set; } = new List<Isler>();

    [ForeignKey("PersonelYetkiTurId")]
    [InverseProperty("Personellers")]
    public virtual YetkiTurler? PersonelYetkiTur { get; set; }

    [ForeignKey("PersonlBirimId")]
    [InverseProperty("Personellers")]
    public virtual Birimler? PersonlBirim { get; set; }
}
