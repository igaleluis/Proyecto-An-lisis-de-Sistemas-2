using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("solicituddocumento")]
[Index("Iddocumento", Name = "ix_solicituddocumento_iddocumento")]
[Index("Idsolicitud", Name = "ix_solicituddocumento_idsolicitud")]
[Index("Idsolicitud", "Iddocumento", Name = "uq_solicituddocumento", IsUnique = true)]
public partial class Solicituddocumento
{
    [Key]
    [Column("idsolicituddocumento")]
    public int Idsolicituddocumento { get; set; }

    [Column("idsolicitud")]
    public int Idsolicitud { get; set; }

    [Column("iddocumento")]
    public int Iddocumento { get; set; }

    [Column("rutaarchivo")]
    [StringLength(500)]
    public string? Rutaarchivo { get; set; }

    [Column("fechacarga", TypeName = "timestamp without time zone")]
    public DateTime? Fechacarga { get; set; }

    [Column("estado")]
    [StringLength(20)]
    public string Estado { get; set; } = null!;

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [ForeignKey("Iddocumento")]
    [InverseProperty("Solicituddocumentos")]
    public virtual Documento IddocumentoNavigation { get; set; } = null!;

    [ForeignKey("Idsolicitud")]
    [InverseProperty("Solicituddocumentos")]
    public virtual Solicitud IdsolicitudNavigation { get; set; } = null!;
}
