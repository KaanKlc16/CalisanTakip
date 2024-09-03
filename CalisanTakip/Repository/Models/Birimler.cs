using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository.Models;

[Table("Birimler")]
public partial class Birimler
{
    [Key]
    [Column("birimId")]
    public int BirimId { get; set; }

    [Column("birimAd")]
    [StringLength(50)]
    public string? BirimAd { get; set; }

    [InverseProperty("PersonlBirim")]
    public virtual ICollection<Personeller> Personellers { get; set; } = new List<Personeller>();
}
