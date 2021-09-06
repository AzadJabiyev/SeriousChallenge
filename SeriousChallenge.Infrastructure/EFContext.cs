using Microsoft.EntityFrameworkCore;
using SeriousChallenge.Infrastructure.DbModel;
using System;

namespace SeriousChallenge.Infrastructure
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockSymbolDbModel>()
                .HasKey(o => new { o.Name, o.InfoDate });
        }


        public DbSet<StockSymbolDbModel> StockSymbols { get; set; }
    }
}
