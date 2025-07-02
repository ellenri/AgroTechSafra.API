using Microsoft.EntityFrameworkCore;
using AgroTechSafra.API.Models;

namespace AgroTechSafra.API.Data
{
    public class AgroTechSafraContext : DbContext
    {
        public AgroTechSafraContext(DbContextOptions<AgroTechSafraContext> options)
            : base(options)
        {
        }

        public DbSet<AvaliacaoPraga> AvaliacoesPragas { get; set; }
        public DbSet<DetalhePraga> DetalhesPragas { get; set; }
        public DbSet<InimigoNatural> InimigosNaturais { get; set; }
        public DbSet<FatorAmbiental> FatoresAmbientais { get; set; }
        public DbSet<RecomendacaoControle> RecomendacoesControle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de precisão para campos decimal
            modelBuilder.Entity<AvaliacaoPraga>(entity =>
            {
                entity.Property(e => e.TemperaturaGraus)
                    .HasPrecision(5, 2);
                entity.Property(e => e.UmidadePercentual)
                    .HasPrecision(5, 2);
                entity.Property(e => e.DensidadePlantio)
                    .HasPrecision(10, 2);
                entity.Property(e => e.DistanciaEntrePontos)
                    .HasPrecision(10, 2);
                entity.Property(e => e.AreaTotalAvaliada)
                    .HasPrecision(10, 2);
            });

            modelBuilder.Entity<DetalhePraga>(entity =>
            {
                entity.Property(e => e.PercentualPlantasInfestadas)
                    .HasPrecision(5, 2);
                entity.Property(e => e.PercentualDano)
                    .HasPrecision(5, 2);
                entity.Property(e => e.SeveridadeDano)
                    .HasPrecision(5, 2);
                entity.Property(e => e.ImpactoProducaoEstimado)
                    .HasPrecision(5, 2);

                // Relacionamento com AvaliacaoPraga
                entity.HasOne(d => d.AvaliacaoPraga)
                    .WithMany(a => a.DetalhesPragas)
                    .HasForeignKey(d => d.AvaliacaoPragaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InimigoNatural>(entity =>
            {
                entity.Property(e => e.PercentualControleNatural)
                    .HasPrecision(5, 2);

                // Relacionamento com AvaliacaoPraga
                entity.HasOne(i => i.AvaliacaoPraga)
                    .WithMany(a => a.InimigosNaturais)
                    .HasForeignKey(i => i.AvaliacaoPragaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FatorAmbiental>(entity =>
            {
                // Relacionamento com AvaliacaoPraga
                entity.HasOne(f => f.AvaliacaoPraga)
                    .WithMany()
                    .HasForeignKey(f => f.AvaliacaoPragaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecomendacaoControle>(entity =>
            {
                entity.Property(e => e.DosagemRecomendada)
                    .HasPrecision(10, 3);

                // Relacionamento com AvaliacaoPraga
                entity.HasOne(r => r.AvaliacaoPraga)
                    .WithMany()
                    .HasForeignKey(r => r.AvaliacaoPragaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}