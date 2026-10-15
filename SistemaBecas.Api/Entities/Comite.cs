using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("comite")]
public partial class Comite
{
    [Key]
    [Column("idcomite")]
    public int Idcomite { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdcomiteNavigation")]
    public virtual ICollection<Decision> Decisions { get; set; } = new List<Decision>();

    [ForeignKey("Idcomite")]
    [InverseProperty("Idcomites")]
    public virtual ICollection<Evaluador> Idevaluadors { get; set; } = new List<Evaluador>();
}
