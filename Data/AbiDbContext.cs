
using AbiWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AbiWebsite.Data {
    public class AbiDbContext : DbContext {
        public AbiDbContext(DbContextOptions<AbiDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<MottoSuggestion> MottoSuggestions { get; set; } = default!;
        public DbSet<Vote> Votes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.StudentId, v.MottoSuggestionId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
