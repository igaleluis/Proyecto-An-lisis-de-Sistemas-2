using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("documento")]
public partial class Documento
{
    [Key]
    [Column("iddocumento")]
    public int Iddocumento { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("obligatorio")]
    public bool Obligatorio { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IddocumentoNavigation")]
    public virtual ICollection<Solicituddocumento> Solicituddocumentos { get; set; } = new List<Solicituddocumento>();
}
