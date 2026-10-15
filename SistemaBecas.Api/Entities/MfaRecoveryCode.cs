using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("mfa_recovery_codes", Schema = "auth")]
[Index("MfaRecoveryCodeSetId", Name = "mfa_recovery_codes_set_id_idx")]
public partial class MfaRecoveryCode
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("mfa_recovery_code_set_id")]
    public Guid MfaRecoveryCodeSetId { get; set; }

    [Column("code_hash")]
    public string CodeHash { get; set; } = null!;

    [Column("consumed_at")]
    public DateTime? ConsumedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("MfaRecoveryCodeSetId")]
    [InverseProperty("MfaRecoveryCodes")]
    public virtual MfaRecoveryCodeSet MfaRecoveryCodeSet { get; set; } = null!;
}
