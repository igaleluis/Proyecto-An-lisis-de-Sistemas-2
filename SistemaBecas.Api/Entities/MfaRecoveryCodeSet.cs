using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("mfa_recovery_code_sets", Schema = "auth")]
[Index("MfaFactorId", Name = "mfa_recovery_code_sets_mfa_factor_id_key", IsUnique = true)]
[Index("UserId", Name = "mfa_recovery_code_sets_user_id_key", IsUnique = true)]
public partial class MfaRecoveryCodeSet
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("mfa_factor_id")]
    public Guid MfaFactorId { get; set; }

    [Column("failed_verification_count")]
    public int FailedVerificationCount { get; set; }

    [Column("verification_locked_until")]
    public DateTime? VerificationLockedUntil { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("MfaFactorId")]
    [InverseProperty("MfaRecoveryCodeSet")]
    public virtual MfaFactor MfaFactor { get; set; } = null!;

    [InverseProperty("MfaRecoveryCodeSet")]
    public virtual ICollection<MfaRecoveryCode> MfaRecoveryCodes { get; set; } = new List<MfaRecoveryCode>();

    [ForeignKey("UserId")]
    [InverseProperty("MfaRecoveryCodeSet")]
    public virtual User User { get; set; } = null!;
}
