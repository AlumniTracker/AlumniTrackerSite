using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AlumniTrackerSite.Models;
using AlumniTrackerSite.Data;
using AlumniTrackerSite.Areas.Identity;
using Microsoft.AspNetCore.Identity;

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

        public virtual DbSet<AlumniUser> AlumniUsers { get; set; } = null!;
        public virtual DbSet<IdentityUser> IdentityUsers { get; set; } = null!;

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
            modelBuilder.Entity<AlumniUser>(entity =>
            {
                entity.HasKey(e => e.StudentId)
                    .HasName("PK__AlumniUs__1788CC4CE62D00D5");

                entity.ToTable("AlumniUser");

                entity.Property(e => e.StudentId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.AdminType).HasMaxLength(150);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Degree).HasMaxLength(100);

                entity.Property(e => e.EmployerName).HasMaxLength(100);

                entity.Property(e => e.FieldofEmployment).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.State).HasMaxLength(2);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(10)
                    .HasColumnName("StudentID");

                entity.Property(e => e.YearGraduated).HasMaxLength(4);

                entity.Property(e => e.Zip).HasMaxLength(10);

                //entity.Property(e => e.Id).HasMaxLength(450);
                //entity.HasOne(d => d.User.Id)
                //    .WithOne(d => d.)
                //    .HasForeignKey(d => d.User.Id)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasContraintName("fk_AlumniUser_AspNetUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
