using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("beca")]
[Index("Idsolicitud", Name = "ix_beca_idsolicitud")]
public partial class Beca
{
    [Key]
    [Column("idbeca")]
    public int Idbeca { get; set; }

    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("fechainicio")]
    public DateOnly Fechainicio { get; set; }

    [Column("fechafin")]
    public DateOnly? Fechafin { get; set; }

    [Column("monto")]
    [Precision(12, 2)]
    public decimal Monto { get; set; }

    [Column("estado")]
    [StringLength(30)]
    public string Estado { get; set; } = null!;

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("fechacreacion", TypeName = "timestamp without time zone")]
    public DateTime Fechacreacion { get; set; }

    [ForeignKey("Idsolicitud")]
    [InverseProperty("Becas")]
    public virtual Solicitud IdsolicitudNavigation { get; set; } = null!;
}
