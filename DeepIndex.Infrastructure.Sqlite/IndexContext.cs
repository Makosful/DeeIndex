using System;
using System.IO;
using DeepIndex.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeepIndex.Infrastructure.Sqlite
{
    public class IndexContext: DbContext
    {
        public IndexContext(DbContextOptions<IndexContext> options) : base(options) { }

        public DbSet<Occurrence> Occurrences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/deepindex";
            Directory.CreateDirectory(folder);
            optionsBuilder.UseSqlite($"Data Source={folder}/database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Occurrence>()
                .HasKey(x => new {x.File, x.Term});
        }
    }
}