using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("evaluacion")]
[Index("Idevaluador", Name = "ix_evaluacion_idevaluador")]
[Index("Idsolicitud", Name = "ix_evaluacion_idsolicitud")]
public partial class Evaluacion
{
    [Key]
    [Column("idevaluacion")]
    public int Idevaluacion { get; set; }

    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("idevaluador")]
    public int Idevaluador { get; set; }

    [Column("fechaevaluacion", TypeName = "timestamp without time zone")]
    public DateTime Fechaevaluacion { get; set; }

    [Column("punteo")]
    [Precision(5, 2)]
    public decimal? Punteo { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("Idevaluador")]
    [InverseProperty("Evaluacions")]
    public virtual Evaluador IdevaluadorNavigation { get; set; } = null!;

    [ForeignKey("Idsolicitud")]
    [InverseProperty("Evaluacions")]
    public virtual Solicitud IdsolicitudNavigation { get; set; } = null!;
}
