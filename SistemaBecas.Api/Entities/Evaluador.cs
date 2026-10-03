using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("evaluador")]
[Index("Idusuario", Name = "ix_evaluador_idusuario")]
public partial class Evaluador
{
    [Key]
    [Column("idevaluador")]
    public int Idevaluador { get; set; }

    [Column("idusuario")]
    public int Idusuario { get; set; }

    [Column("cargo")]
    [StringLength(100)]
    public string Cargo { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdevaluadorNavigation")]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();

    [ForeignKey("Idusuario")]
    [InverseProperty("Evaluadors")]
    public virtual Usuario IdusuarioNavigation { get; set; } = null!;

    [ForeignKey("Idevaluador")]
    [InverseProperty("Idevaluadors")]
    public virtual ICollection<Comite> Idcomites { get; set; } = new List<Comite>();
}
