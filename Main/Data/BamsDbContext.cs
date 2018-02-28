using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Main.Models;

namespace Main.Data
{
    public class BamsDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetModule> AssetModules { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<AssetData> AssetData { get; set; }
        public DbSet<TaskQueue> TaskQueues { get; set; }
        public DbSet<DataSchema> Schemas { get; set; }
        public DbSet<Reporting> Reportings { get; set; }

        public BamsDbContext(DbContextOptions<BamsDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetModule>().HasKey(A => new { A.assetID, A.moduleID });
            modelBuilder.Entity<AssetData>().HasKey(A => new { A.assetID, A.dataEntryID, A.fieldName });

            modelBuilder.Entity<Asset>().ToTable("Asset");
            modelBuilder.Entity<AssetData>().ToTable("AssetData");
            modelBuilder.Entity<AssetModule>().ToTable("AssetModule");
            modelBuilder.Entity<Module>().ToTable("Module");
            modelBuilder.Entity<TaskQueue>().ToTable("TaskQueue");
            modelBuilder.Entity<DataSchema>().ToTable("DataSchema");
            modelBuilder.Entity<Reporting>().ToTable("Reporting");
        }
    }
}
