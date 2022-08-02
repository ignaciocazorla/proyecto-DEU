using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProyectoDEU_API
{
    public partial class ProyectoDEUContext : DbContext
    {
        public ProyectoDEUContext()
        {
        }

        public ProyectoDEUContext(DbContextOptions<ProyectoDEUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Curso> Cursos { get; set; } = null!;
        public virtual DbSet<Docente> Docentes { get; set; } = null!;
        public virtual DbSet<EnlaceRecurso> EnlaceRecursos { get; set; } = null!;
        public virtual DbSet<Estudiante> Estudiantes { get; set; } = null!;
        public virtual DbSet<Pregunta> Pregunta { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<Recurso> Recursos { get; set; } = null!;
        public virtual DbSet<Respuesta> Respuesta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer(
//            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>(entity =>
            {
                entity.ToTable("Curso");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.HasOne(d => d.Docente)
                    .WithMany(p => p.Cursos)
                    .HasForeignKey(d => d.IdDocente)
                    .HasConstraintName("FK_Curso_Docente1");
            });

            modelBuilder.Entity<Docente>(entity =>
            {
                entity.ToTable("Docente");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre).HasMaxLength(256);

                entity.HasMany(d => d.IdQuizzes)
                    .WithMany(p => p.IdDocentes)
                    .UsingEntity<Dictionary<string, object>>(
                        "DocenteQuiz",
                        l => l.HasOne<Quiz>().WithMany().HasForeignKey("IdQuiz").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DocenteQuiz_Quiz"),
                        r => r.HasOne<Docente>().WithMany().HasForeignKey("IdDocente").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DocenteQuiz_Docente"),
                        j =>
                        {
                            j.HasKey("IdDocente", "IdQuiz");

                            j.ToTable("DocenteQuiz");
                        });
            });

            modelBuilder.Entity<EnlaceRecurso>(entity =>
            {
                entity.ToTable("EnlaceRecurso");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdRecursoNavigation)
                    .WithMany(p => p.EnlaceRecursos)
                    .HasForeignKey(d => d.IdRecurso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnlaceRecurso_Recurso");
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.ToTable("Estudiante");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre).HasMaxLength(256);

                entity.HasMany(d => d.Cursos)
                    .WithMany(p => p.Estudiantes)
                    .UsingEntity<Dictionary<string, object>>(
                        "EstudianteCurso",
                        l => l.HasOne<Curso>().WithMany().HasForeignKey("IdCurso").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstudianteCurso_Curso1"),
                        r => r.HasOne<Estudiante>().WithMany().HasForeignKey("IdEstudiante").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstudianteCurso_Estudiante1"),
                        j =>
                        {
                            j.HasKey("IdEstudiante", "IdCurso");

                            j.ToTable("EstudianteCurso");
                        });
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdQuizNavigation)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.IdQuiz)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pregunta_Quiz");

                entity.HasOne(d => d.IdRecursoNavigation)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.IdRecurso)
                    .HasConstraintName("FK_Pregunta_Recurso");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quiz");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Titulo).HasMaxLength(100);

                entity.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.IdCurso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quiz_Curso1");
            });

            modelBuilder.Entity<Recurso>(entity =>
            {
                entity.ToTable("Recurso");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Titulo).HasMaxLength(100);

                entity.HasOne(d => d.Curso)
                    .WithMany(p => p.Recursos)
                    .HasForeignKey(d => d.IdCurso)
                    .HasConstraintName("FK_Recurso_Curso");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdPreguntaNavigation)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.IdPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Respuesta_Pregunta");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
