using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("solicitud")]
[Index("Idconvocatoria", Name = "ix_solicitud_idconvocatoria")]
[Index("Idestudiante", Name = "ix_solicitud_idestudiante")]
public partial class Solicitud
{
    [Key]
    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("idconvocatoria")]
    public int Idconvocatoria { get; set; }

    [Column("idestudiante")]
    public int Idestudiante { get; set; }

    [Column("fechasolicitud", TypeName = "timestamp without time zone")]
    public DateTime Fechasolicitud { get; set; }

    [Column("estado")]
    [StringLength(30)]
    public string Estado { get; set; } = null!;

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("fechaactualizacion", TypeName = "timestamp without time zone")]
    public DateTime? Fechaactualizacion { get; set; }

    [InverseProperty("IdsolicitudNavigation")]
    public virtual ICollection<Beca> Becas { get; set; } = new List<Beca>();

    [InverseProperty("IdsolicitudNavigation")]
    public virtual ICollection<Decision> Decisions { get; set; } = new List<Decision>();

    [InverseProperty("IdsolicitudNavigation")]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();

    [InverseProperty("IdsolicitudNavigation")]
    public virtual ICollection<Historialsolicitud> Historialsolicituds { get; set; } = new List<Historialsolicitud>();

    [ForeignKey("Idconvocatoria")]
    [InverseProperty("Solicituds")]
    public virtual Convocatorium IdconvocatoriaNavigation { get; set; } = null!;

    [ForeignKey("Idestudiante")]
    [InverseProperty("Solicituds")]
    public virtual Estudiante IdestudianteNavigation { get; set; } = null!;

    [InverseProperty("IdsolicitudNavigation")]
    public virtual ICollection<Solicituddocumento> Solicituddocumentos { get; set; } = new List<Solicituddocumento>();
}
