using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CalisanTakip.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Models;


[Table("Isler")]
public partial class Isler
{
    [Key]
    [Column("isId")]
    public int? IsId { get; set; }

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

    [ForeignKey("IsDurumId")]
    public virtual Durumlar? IsDurum { get; set; }

    [ForeignKey("IsPersonelId")]
    public virtual Personeller? IsPersonel { get; set; }
}
