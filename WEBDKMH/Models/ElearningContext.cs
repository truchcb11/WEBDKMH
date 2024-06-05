using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class ElearningContext : DbContext
    {
        public ElearningContext()
        {
        }

        public ElearningContext(DbContextOptions<ElearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Lecturers> Lecturers { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=Elearning;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Images).IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => new { e.Idlecturers, e.Idsubjects, e.Id });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdlecturersNavigation)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.Idlecturers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Lecturers");

                entity.HasOne(d => d.IdsubjectsNavigation)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.Idsubjects)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Subjects");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => new { e.Iduser, e.Idsub });

                entity.HasOne(d => d.IdsubNavigation)
                    .WithMany(p => p.History)
                    .HasForeignKey(d => d.Idsub)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_Subjects");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.History)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_Users");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasOne(d => d.IdsubNavigation)
                    .WithMany(p => p.Lesson)
                    .HasForeignKey(d => d.Idsub)
                    .HasConstraintName("FK_Lesson_Subjects");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdsubNavigation)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.Idsub)
                    .HasConstraintName("FK_Question_Subjects");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.Iduser, e.Idexam, e.Idques });

                entity.HasOne(d => d.IdexamNavigation)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Idexam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Exam");

                entity.HasOne(d => d.IdquesNavigation)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Idques)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Question");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Users");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.Property(e => e.Dktienquyet).IsUnicode(false);

                entity.Property(e => e.Images).IsUnicode(false);

                entity.Property(e => e.MaSubhect).IsUnicode(false);

                entity.HasOne(d => d.IdcateNavigation)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.Idcate)
                    .HasConstraintName("FK_Subjects_Category");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.HasOne(d => d.IdrollNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Idroll)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
