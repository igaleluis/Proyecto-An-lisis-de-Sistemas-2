using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("decision")]
[Index("Idcomite", Name = "ix_decision_idcomite")]
[Index("Idsolicitud", Name = "ix_decision_idsolicitud")]
[Index("Idusuarioregistro", Name = "ix_decision_idusuarioregistro")]
public partial class Decision
{
    [Key]
    [Column("iddecision")]
    public int Iddecision { get; set; }

    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("idcomite")]
    public int Idcomite { get; set; }

    [Column("decision")]
    [StringLength(30)]
    public string Decision1 { get; set; } = null!;

    [Column("fechadecision", TypeName = "timestamp without time zone")]
    public DateTime Fechadecision { get; set; }

    [Column("justificacion")]
    public string? Justificacion { get; set; }

    [Column("idusuarioregistro")]
    public int Idusuarioregistro { get; set; }

    [ForeignKey("Idcomite")]
    [InverseProperty("Decisions")]
    public virtual Comite IdcomiteNavigation { get; set; } = null!;

    [ForeignKey("Idsolicitud")]
    [InverseProperty("Decisions")]
    public virtual Solicitud IdsolicitudNavigation { get; set; } = null!;

    [ForeignKey("Idusuarioregistro")]
    [InverseProperty("Decisions")]
    public virtual Usuario IdusuarioregistroNavigation { get; set; } = null!;
}
