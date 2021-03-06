using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AlumniTrackerSite.Models;
using AlumniTrackerSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AlumniTrackerSite.Contexts
{
    public partial class TrackerContext : AlumniIdentityContext
    {
        public static IConfiguration Configuration;

        public TrackerContext(DbContextOptions<TrackerContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }
        public List<Alumnis> GetAlumnis()
        {
            List<Alumnis> Alumnees = new List<Alumnis>();
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("conn")))
            {
                SqlCommand cmd = new SqlCommand("GetAlumnis", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var alum = new Alumnis();
                    alum.AlumniId = dr.GetInt32(0);
                    alum.StudentId = dr.GetString(1);
                    alum.Name = dr.GetString(2);

                    if (!dr.IsDBNull(3)) alum.EmployerName = dr.GetString(3);
                    if (!dr.IsDBNull(4)) alum.FieldofEmployment = dr.GetString(4);
                    if (!dr.IsDBNull(5)) alum.YearGraduated = dr.GetString(5);
                    if (!dr.IsDBNull(6)) alum.Degree = dr.GetString(6);
                    if (!dr.IsDBNull(7)) alum.Notes = dr.GetString(7);

                    alum.DateModified = dr.GetDateTime(8);

                    if (!dr.IsDBNull(9)) alum.Address = dr.GetString(9);
                    if (!dr.IsDBNull(10)) alum.City = dr.GetString(10);
                    if (!dr.IsDBNull(11)) alum.State = dr.GetString(11);
                    if (!dr.IsDBNull(12)) alum.Zip = dr.GetString(12);

                    alum.Email = dr.GetString(13);
                    alum.Id = dr.GetString(14);
                    Alumnees.Add(alum);
                }
                dr.Close();
            }
            return Alumnees;
                
        } 

        public virtual DbSet<AdminUser> AdminUsers { get; set; } = null!;
        public virtual DbSet<AlumniUser> AlumniUsers { get; set; } = null!;

        //public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        //public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        //public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("conn"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.ToTable("AdminUser");

                entity.Property(e => e.AdminName).HasMaxLength(50);

                entity.Property(e => e.AdminType).HasMaxLength(50);

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.AdminUsers)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_AdminUser_AspNetUsers1");
            });

            modelBuilder.Entity<AlumniUser>(entity =>
            {
                entity.HasKey(e => e.AlumniId)
                    .HasName("PK__AlumniUs__1788CC4CE62D00D5");

                entity.ToTable("AlumniUser");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Degree).HasMaxLength(100);

                entity.Property(e => e.EmployerName).HasMaxLength(100);

                entity.Property(e => e.FieldofEmployment).HasMaxLength(50);

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.State).HasMaxLength(2);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(10)
                    .HasColumnName("StudentID");

                entity.Property(e => e.YearGraduated).HasMaxLength(4);

                entity.Property(e => e.Zip).HasMaxLength(10);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.AlumniUsers)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_AlumniUser_AspNetUsers1");
            });

            //modelBuilder.Entity<AspNetRole>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Name).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetRoleClaim>(entity =>
            //{
            //    entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetRoleClaims)
            //        .HasForeignKey(d => d.RoleId);
            //});

            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("[NormalizedUserName] IS NOT NULL");

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);

            //    entity.HasMany(d => d.Roles)
            //        .WithMany(p => p.Users)
            //        .UsingEntity<Dictionary<string, object>>(
            //            "AspNetUserRole",
            //            l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
            //            r => r.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
            //            j =>
            //            {
            //                j.HasKey("UserId", "RoleId");

            //                j.ToTable("AspNetUserRoles");

            //                j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
            //            });
            //});

            //modelBuilder.Entity<AspNetUserClaim>(entity =>
            //{
            //    entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId);
            //});

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
