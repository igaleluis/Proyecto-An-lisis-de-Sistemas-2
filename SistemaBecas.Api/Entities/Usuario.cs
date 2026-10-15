using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("usuario")]
[Index("Correo", Name = "usuario_correo_key", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("idusuario")]
    public int Idusuario { get; set; }

    [Column("nombres")]
    [StringLength(100)]
    public string Nombres { get; set; } = null!;

    [Column("apellidos")]
    [StringLength(100)]
    public string Apellidos { get; set; } = null!;

    [Column("correo")]
    [StringLength(150)]
    public string Correo { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(255)]
    public string Passwordhash { get; set; } = null!;

    [Column("rol")]
    [StringLength(50)]
    public string Rol { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [Column("fechacreacion", TypeName = "timestamp without time zone")]
    public DateTime Fechacreacion { get; set; }

    [InverseProperty("IdusuarioregistroNavigation")]
    public virtual ICollection<Decision> Decisions { get; set; } = new List<Decision>();

    [InverseProperty("IdusuarioNavigation")]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    [InverseProperty("IdusuarioNavigation")]
    public virtual ICollection<Evaluador> Evaluadors { get; set; } = new List<Evaluador>();

    [InverseProperty("IdusuarioNavigation")]
    public virtual ICollection<Historialsolicitud> Historialsolicituds { get; set; } = new List<Historialsolicitud>();

    [InverseProperty("IdusuarioNavigation")]
    public virtual ICollection<RecuperacionPassword> RecuperacionPasswords { get; set; } = new List<RecuperacionPassword>();
}
