using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("recuperacion_password")]
public partial class RecuperacionPassword
{
    [Key]
    [Column("idrecuperacion")]
    public int Idrecuperacion { get; set; }

    [Column("idusuario")]
    public int Idusuario { get; set; }

    [Column("codigo")]
    [StringLength(6)]
    public string Codigo { get; set; } = null!;

    [Column("fechaexpiracion", TypeName = "timestamp without time zone")]
    public DateTime Fechaexpiracion { get; set; }

    [Column("usado")]
    public bool Usado { get; set; }

    [Column("fechacreacion", TypeName = "timestamp without time zone")]
    public DateTime Fechacreacion { get; set; }

    [ForeignKey("Idusuario")]
    [InverseProperty("RecuperacionPasswords")]
    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
