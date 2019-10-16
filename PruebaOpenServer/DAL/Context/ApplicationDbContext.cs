using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PokemonRankResult> RankResults { get; set; }
        public DbSet<PokemonBattleRecord> BattleRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PokemonRankResult>()
                .HasKey(r => r.Id);
            builder.Entity<PokemonRankResult>().
                Property(r => r.Id).UseSqlServerIdentityColumn();

            builder.Entity<PokemonBattleRecord>()
                .HasKey(r => r.Id);
            builder.Entity<PokemonBattleRecord>()
                .Property(r => r.Id).UseSqlServerIdentityColumn();

            builder.Entity<PokemonRankResult>()
                .HasMany(res => res.BattleRecords)
                .WithOne(rec => rec.PokemonRankResult)
                .HasForeignKey(entity => entity.PokemonRankResultId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
