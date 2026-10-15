using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("estudiante")]
[Index("Dpi", Name = "estudiante_dpi_key", IsUnique = true)]
[Index("Idusuario", Name = "ix_estudiante_idusuario")]
public partial class Estudiante
{
    [Key]
    [Column("idestudiante")]
    public int Idestudiante { get; set; }

    [Column("idusuario")]
    public int Idusuario { get; set; }

    [Column("dpi")]
    [StringLength(20)]
    public string Dpi { get; set; } = null!;

    [Column("fechanacimiento")]
    public DateOnly Fechanacimiento { get; set; }

    [Column("telefono")]
    [StringLength(30)]
    public string? Telefono { get; set; }

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [ForeignKey("Idusuario")]
    [InverseProperty("Estudiantes")]
    public virtual Usuario IdusuarioNavigation { get; set; } = null!;

    [InverseProperty("IdestudianteNavigation")]
    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
