using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository.Models;

[Table("YetkiTurler")]
public partial class YetkiTurler
{
    [Key]
    [Column("yetkiTurId")]
    public int YetkiTurId { get; set; }

    [Column("yetkiTurAd")]
    [StringLength(50)]
    public string? YetkiTurAd { get; set; }

    [InverseProperty("PersonelYetkiTur")]
    public virtual ICollection<Personeller> Personellers { get; set; } = new List<Personeller>();
}
