using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository.Models;

[Table("Durumlar")]
public partial class Durumlar
{
    [Key]
    [Column("durumId")]
    public int DurumId { get; set; }

    [Column("durumAd")]
    [StringLength(50)]
    public string? DurumAd { get; set; }

    [InverseProperty("IsDurum")]
    public virtual ICollection<Isler> Islers { get; set; } = new List<Isler>();
}
