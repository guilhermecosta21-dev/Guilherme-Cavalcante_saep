using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace appClinica.Models
{
    public class DbClinicaContext : DbContext
    {
        public DbClinicaContext(DbContextOptions<DbClinicaContext> options)
            : base(options) { }

        public virtual DbSet<Agendamento> Agendamento { get; set; }

        public virtual DbSet<Medico> Medico { get; set; }

        public virtual DbSet<Pet> Pet { get; set; }

        public virtual DbSet<Tutor> Tutor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agendamento>(entity =>
            {
                entity.ToTable("Agendamento");
                entity.Property(a => a.DataAgendamento).HasColumnType("datetime");
                entity.HasOne(a => a.Tutor).WithMany(t => t.Agendamentos)
                    .HasForeignKey(a => a.TutorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Pet).WithMany(p => p.Agendamentos)
                    .HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Medico).WithMany(m => m.Agendamentos)
                    .HasForeignKey(a => a.MedicoId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Medico>(entity =>
            {
                entity.ToTable("Medico");
                entity.Property(m => m.Nome).HasMaxLength(100).IsUnicode(false);
                entity.Property(m => m.Crmv).HasMaxLength(20).IsUnicode(false);
                entity.HasIndex(m => m.Crmv).IsUnique();
                entity.Property(m => m.Especialidade).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");
                entity.Property(p => p.Nome).HasMaxLength(100).IsUnicode(false);
                entity.Property(p => p.Especie).HasMaxLength(50).IsUnicode(false);
                entity.Property(p => p.Raca).HasMaxLength(50).IsUnicode(false);
                entity.HasOne(p => p.Tutor).WithMany(t => t.Pets)
                    .HasForeignKey(p => p.TutorId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.ToTable("Tutor");
                entity.Property(p => p.Nome).HasMaxLength(100).IsUnicode(false);
                entity.Property(p => p.Cpf).HasMaxLength(64).IsUnicode(false);
                entity.HasIndex(p => p.Cpf).IsUnique();
                entity.Property(p => p.Telefone).HasMaxLength(20).IsUnicode(false);
                entity.Property(p => p.DataNascimento).HasColumnType("date");
            });
        }
    }
}