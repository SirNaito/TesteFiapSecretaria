using Microsoft.EntityFrameworkCore;
using SecretariaFiapFluxo.Domain.Entities;

namespace SecretariaFiapFluxo.Infrastructure.Models
{
    public class SecretariaContext : DbContext
    {
        public SecretariaContext(DbContextOptions<SecretariaContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; } = null!;
        public DbSet<Turma> Turmas { get; set; } = null!;
        public DbSet<Matricula> Matriculas { get; set; } = null!;
        public DbSet<Admins> Admins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------- Aluno -----------------
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.CPF).IsUnique();
                entity.HasIndex(a => a.Email).IsUnique();
                entity.Property(a => a.Nome).HasMaxLength(100).IsRequired();
                entity.Property(a => a.Email).HasMaxLength(100).IsRequired();
                entity.Property(a => a.CPF).HasMaxLength(11).IsRequired();
                entity.Property(a => a.Senha).HasMaxLength(500).IsRequired();
            });

            // ----------------- Turma -----------------
            modelBuilder.Entity<Turma>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasIndex(t => t.Nome).IsUnique();
                entity.Property(t => t.Nome).HasMaxLength(100).IsRequired();
                entity.Property(t => t.Descricao).HasMaxLength(250).IsRequired();
            });

            // ----------------- Matricula -----------------
            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasIndex(m => new { m.AlunoId, m.TurmaId }).IsUnique();

                entity.HasOne(m => m.Aluno)
                      .WithMany(a => a.Matriculas)
                      .HasForeignKey(m => m.AlunoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Turma)
                      .WithMany(t => t.Matriculas)
                      .HasForeignKey(m => m.TurmaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ----------------- Admin -----------------
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.Email).IsUnique();
                entity.Property(a => a.Nome).HasMaxLength(100).IsRequired();
                entity.Property(a => a.Email).HasMaxLength(100).IsRequired();
                entity.Property(a => a.Senha).HasMaxLength(500).IsRequired();
            });
        }
    }
}
