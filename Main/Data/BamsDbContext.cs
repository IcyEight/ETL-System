using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Main.Models;

namespace Main.Data
{
    public class BamsDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<TaskQueue> TaskQueues { get; set; }

        public BamsDbContext(DbContextOptions<BamsDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>().ToTable("Asset");
            modelBuilder.Entity<TaskQueue>().ToTable("TaskQueue");
        }
    }
}
