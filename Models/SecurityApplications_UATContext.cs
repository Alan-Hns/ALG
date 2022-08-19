using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CrudSecApp.Models
{
    public partial class SecurityApplications_UATContext : DbContext
    {
        public SecurityApplications_UATContext()
        {
        }

        public SecurityApplications_UATContext(DbContextOptions<SecurityApplications_UATContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<ApplicationModule> ApplicationModules { get; set; } = null!;
        public virtual DbSet<ApplicationPermission> ApplicationPermissions { get; set; } = null!;
        public virtual DbSet<ApplicationSection> ApplicationSections { get; set; } = null!;
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public virtual DbSet<ApplicationUserPermission> ApplicationUserPermissions { get; set; } = null!;
        public virtual DbSet<ApplicationUserRol> ApplicationUserRols { get; set; } = null!;
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; } = null!;
        public virtual DbSet<DetalleRol> DetalleRols { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UserExtendedPermission> UserExtendedPermissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Applications", "Authentication");

                entity.HasIndex(e => e.Name, "IX_Name")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");
            });

            modelBuilder.Entity<ApplicationModule>(entity =>
            {
                entity.ToTable("ApplicationModules", "Authentication");

                entity.HasIndex(e => new { e.Module, e.ApplicationId }, "IX_Module_ApplicationId")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Module)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationModules)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_Authentication.ApplicationModules_Authentication.Applications_ApplicationId");
            });

            modelBuilder.Entity<ApplicationPermission>(entity =>
            {
                entity.ToTable("ApplicationPermissions", "Authentication");

                entity.HasIndex(e => new { e.Permission, e.ApplicationSectionId }, "IX_Permission_ApplicationSectionId")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.Group)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Permission)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("RowVersion"); 

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.HasOne(d => d.ApplicationSection)
                    .WithMany(p => p.ApplicationPermissions)
                    .HasForeignKey(d => d.ApplicationSectionId)
                    .HasConstraintName("FK_Authentication.ApplicationPermissions_Authentication.ApplicationSections_ApplicationSectionId");
            });

            modelBuilder.Entity<ApplicationSection>(entity =>
            {
                entity.ToTable("ApplicationSections", "Authentication");

                entity.HasIndex(e => new { e.Section, e.ApplicationModuleId }, "IX_Section_ApplicationModuleId")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.IconName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Section)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.HasOne(d => d.ApplicationModule)
                    .WithMany(p => p.ApplicationSections)
                    .HasForeignKey(d => d.ApplicationModuleId)
                    .HasConstraintName("FK_Authentication.ApplicationSections_Authentication.ApplicationModules_ApplicationModuleId");
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("ApplicationUsers", "Authentication");

                entity.HasIndex(e => e.UserName, "IX_UserName")
                    .IsUnique();

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.LastDateStartSession).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(8000)
                    .HasConversion<byte[]>();

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationUserRole)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.ApplicationUserRoleId)
                    .HasConstraintName("FK_ApplicationUsers_ApplicationUserRoles");
            });

            modelBuilder.Entity<ApplicationUserPermission>(entity =>
            {
                entity.ToTable("ApplicationUserPermissions", "Authentication");

                entity.HasIndex(e => e.ApplicationId, "IX_ApplicationId");

                entity.HasIndex(e => new { e.ApplicationUserId, e.ApplicationPermissionId }, "IX_ApplicationUserId_ApplicationPermissionId")
                    .IsUnique();

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Site)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.HasOne(d => d.ApplicationPermission)
                    .WithMany(p => p.ApplicationUserPermissions)
                    .HasForeignKey(d => d.ApplicationPermissionId)
                    .HasConstraintName("FK_Authentication.ApplicationUserPermissions_Authentication.ApplicationPermissions_ApplicationPermissionId");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.ApplicationUserPermissions)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .HasConstraintName("FK_Authentication.ApplicationUserPermissions_Authentication.ApplicationUsers_ApplicationUserId");
            });

            modelBuilder.Entity<ApplicationUserRol>(entity =>
            {
                entity.ToTable("ApplicationUserRol");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.ApplicationUserRols)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUserRol_ApplicationUsers");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.ApplicationUserRols)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUserRol_Roles");
            });

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.Property(e => e.Roles)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetalleRol>(entity =>
            {
                entity.ToTable("DetalleRol");

                entity.HasOne(d => d.ApplicationPermission)
                    .WithMany(p => p.DetalleRols)
                    .HasForeignKey(d => d.ApplicationPermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleRol_ApplicationPermissions");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.DetalleRols)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleRol_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserExtendedPermission>(entity =>
            {
                entity.ToTable("UserExtendedPermissions", "Authentication");

                entity.HasIndex(e => new { e.ApplicationUserPermissionId, e.AcessCode, e.BusinessEntity }, "IX_ApplicationUserPermissionId_AcessCode_BusinessEntity")
                    .IsUnique();

                entity.Property(e => e.AcessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessEntity)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccionUtc).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.TipoTransaccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionUid).HasColumnName("TransaccionUId");

                entity.HasOne(d => d.ApplicationUserPermission)
                    .WithMany(p => p.UserExtendedPermissions)
                    .HasForeignKey(d => d.ApplicationUserPermissionId)
                    .HasConstraintName("FK_Authentication.UserExtendedPermissions_Authentication.ApplicationUserPermissions_ApplicationUserPermissionId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
