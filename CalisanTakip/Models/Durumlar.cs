using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Models;

[Table("Durumlar")]
public partial class Durumlar
{
    [Key]
    [Column("durumId")]
    public int DurumId { get; set; }

    [Column("durumAd")]
    [StringLength(50)]
    public string? DurumAd { get; set; }
}
