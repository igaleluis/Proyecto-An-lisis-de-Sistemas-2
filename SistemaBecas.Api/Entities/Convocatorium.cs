using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("convocatoria")]
public partial class Convocatorium
{
    [Key]
    [Column("idconvocatoria")]
    public int Idconvocatoria { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("fechainicio")]
    public DateOnly Fechainicio { get; set; }

    [Column("fechafin")]
    public DateOnly Fechafin { get; set; }

    [Column("cupos")]
    public int Cupos { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [Column("fechacreacion", TypeName = "timestamp without time zone")]
    public DateTime Fechacreacion { get; set; }

    [InverseProperty("IdconvocatoriaNavigation")]
    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
