using System;
using JobManagerMVC_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace JobManagerMVC_2
{
    public partial class TaskDBContext : DbContext
    {
        public TaskDBContext()
        {
        }

        public TaskDBContext(DbContextOptions<TaskDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=TaskDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.Asignee).HasColumnType("text");

                entity.Property(e => e.CompleteDate).HasColumnType("date");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.RegDate).HasColumnType("date");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Job_Job");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Job_Status");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
