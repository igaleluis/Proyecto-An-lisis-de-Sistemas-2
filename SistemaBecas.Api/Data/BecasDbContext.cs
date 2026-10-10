using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaBecas.Api.Entities;

namespace SistemaBecas.Api.Data;

public partial class BecasDbContext : DbContext
{
    public BecasDbContext()
    {
    }

    public BecasDbContext(DbContextOptions<BecasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Beca> Becas { get; set; }

    public virtual DbSet<Comite> Comites { get; set; }

    public virtual DbSet<Convocatorium> Convocatoria { get; set; }

    public virtual DbSet<Decision> Decisions { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Evaluacion> Evaluacions { get; set; }

    public virtual DbSet<Evaluador> Evaluadors { get; set; }

    public virtual DbSet<Historialsolicitud> Historialsolicituds { get; set; }

    public virtual DbSet<RecuperacionPassword> RecuperacionPasswords { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<Solicituddocumento> Solicituddocumentos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-0-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.awcdpoyhdkrexttswkzo;Password=CllrJlllLall119;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone", "recovery_code" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in", "like", "ilike", "is", "match", "imatch", "isdistinct" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Beca>(entity =>
        {
            entity.HasKey(e => e.Idbeca).HasName("beca_pkey");

            entity.Property(e => e.Fechacreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Becas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_beca_solicitud");
        });

        modelBuilder.Entity<Comite>(entity =>
        {
            entity.HasKey(e => e.Idcomite).HasName("comite_pkey");

            entity.HasMany(d => d.Idevaluadors).WithMany(p => p.Idcomites)
                .UsingEntity<Dictionary<string, object>>(
                    "Comiteevaluador",
                    r => r.HasOne<Evaluador>().WithMany()
                        .HasForeignKey("Idevaluador")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_comiteevaluador_evaluador"),
                    l => l.HasOne<Comite>().WithMany()
                        .HasForeignKey("Idcomite")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_comiteevaluador_comite"),
                    j =>
                    {
                        j.HasKey("Idcomite", "Idevaluador").HasName("comiteevaluador_pkey");
                        j.ToTable("comiteevaluador");
                        j.IndexerProperty<int>("Idcomite").HasColumnName("idcomite");
                        j.IndexerProperty<int>("Idevaluador").HasColumnName("idevaluador");
                    });
        });

        modelBuilder.Entity<Convocatorium>(entity =>
        {
            entity.HasKey(e => e.Idconvocatoria).HasName("convocatoria_pkey");

            entity.Property(e => e.Fechacreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(e => e.Iddecision).HasName("decision_pkey");

            entity.Property(e => e.Fechadecision).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdcomiteNavigation).WithMany(p => p.Decisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_decision_comite");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Decisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_decision_solicitud");

            entity.HasOne(d => d.IdusuarioregistroNavigation).WithMany(p => p.Decisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_decision_usuario");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.Iddocumento).HasName("documento_pkey");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.Idestudiante).HasName("estudiante_pkey");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Estudiantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estudiante_usuario");
        });

        modelBuilder.Entity<Evaluacion>(entity =>
        {
            entity.HasKey(e => e.Idevaluacion).HasName("evaluacion_pkey");

            entity.Property(e => e.Fechaevaluacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdevaluadorNavigation).WithMany(p => p.Evaluacions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evaluacion_evaluador");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Evaluacions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evaluacion_solicitud");
        });

        modelBuilder.Entity<Evaluador>(entity =>
        {
            entity.HasKey(e => e.Idevaluador).HasName("evaluador_pkey");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Evaluadors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evaluador_usuario");
        });

        modelBuilder.Entity<Historialsolicitud>(entity =>
        {
            entity.HasKey(e => e.Idhistorial).HasName("historialsolicitud_pkey");

            entity.Property(e => e.Fechacambio).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Historialsolicituds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_historialsolicitud_solicitud");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Historialsolicituds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_historialsolicitud_usuario");
        });

        modelBuilder.Entity<RecuperacionPassword>(entity =>
        {
            entity.HasKey(e => e.Idrecuperacion).HasName("recuperacion_password_pkey");

            entity.Property(e => e.Fechacreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.RecuperacionPasswords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recuperacion_password_usuario_fkey");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Idsolicitud).HasName("solicitud_pkey");

            entity.Property(e => e.Fechasolicitud).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdconvocatoriaNavigation).WithMany(p => p.Solicituds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicitud_convocatoria");

            entity.HasOne(d => d.IdestudianteNavigation).WithMany(p => p.Solicituds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicitud_estudiante");
        });

        modelBuilder.Entity<Solicituddocumento>(entity =>
        {
            entity.HasKey(e => e.Idsolicituddocumento).HasName("solicituddocumento_pkey");

            entity.HasOne(d => d.IddocumentoNavigation).WithMany(p => p.Solicituddocumentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicituddocumento_documento");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Solicituddocumentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solicituddocumento_solicitud");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("usuario_pkey");

            entity.Property(e => e.Fechacreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
