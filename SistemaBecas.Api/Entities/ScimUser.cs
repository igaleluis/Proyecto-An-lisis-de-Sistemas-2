using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SistemaBecas.Api.Entities;

[Table("scim_users", Schema = "auth")]
[Index("DeletedAt", Name = "scim_users_deleted_at_idx")]
[Index("SsoProviderId", Name = "scim_users_sso_provider_id_idx")]
[Index("UserId", Name = "scim_users_user_id_idx")]
public partial class ScimUser
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sso_provider_id")]
    public Guid SsoProviderId { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("resource", TypeName = "jsonb")]
    public string Resource { get; set; } = null!;

    [Column("user_name")]
    public string UserName { get; set; } = null!;

    [Column("external_id")]
    public string? ExternalId { get; set; }

    [Column("active")]
    public bool Active { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("SsoProviderId")]
    [InverseProperty("ScimUsers")]
    public virtual SsoProvider SsoProvider { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ScimUsers")]
    public virtual User? User { get; set; }
}
