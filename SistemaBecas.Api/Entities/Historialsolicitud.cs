using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("historialsolicitud")]
[Index("Idsolicitud", Name = "ix_historialsolicitud_idsolicitud")]
[Index("Idusuario", Name = "ix_historialsolicitud_idusuario")]
public partial class Historialsolicitud
{
    [Key]
    [Column("idhistorial")]
    public int Idhistorial { get; set; }

    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("estadoanterior")]
    [StringLength(30)]
    public string? Estadoanterior { get; set; }

    [Column("estadonuevo")]
    [StringLength(30)]
    public string Estadonuevo { get; set; } = null!;

    [Column("fechacambio", TypeName = "timestamp without time zone")]
    public DateTime Fechacambio { get; set; }

    [Column("idusuario")]
    public int Idusuario { get; set; }

    [Column("comentario")]
    public string? Comentario { get; set; }

    [ForeignKey("Idsolicitud")]
    [InverseProperty("Historialsolicituds")]
    public virtual Solicitud IdsolicitudNavigation { get; set; } = null!;

    [ForeignKey("Idusuario")]
    [InverseProperty("Historialsolicituds")]
    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
