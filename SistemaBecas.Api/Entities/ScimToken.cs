using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("scim_tokens", Schema = "auth")]
[Index("ExpiresAt", Name = "scim_tokens_expires_at_idx")]
[Index("RevokedAt", Name = "scim_tokens_revoked_at_idx")]
[Index("SsoProviderId", Name = "scim_tokens_sso_provider_id_idx")]
[Index("TokenHash", Name = "scim_tokens_token_hash_key", IsUnique = true)]
public partial class ScimToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sso_provider_id")]
    public Guid SsoProviderId { get; set; }

    [Column("token_hash")]
    public string TokenHash { get; set; } = null!;

    [Column("prefix")]
    public string Prefix { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [Column("revoked_at")]
    public DateTime? RevokedAt { get; set; }

    [Column("last_used_at")]
    public DateTime? LastUsedAt { get; set; }

    [ForeignKey("SsoProviderId")]
    [InverseProperty("ScimTokens")]
    public virtual SsoProvider SsoProvider { get; set; } = null!;
}
