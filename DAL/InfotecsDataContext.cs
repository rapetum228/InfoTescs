using InfoTecs.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfoTecs.DAL
{
    public class InfotecsDataContext : DbContext
    {
        public InfotecsDataContext(DbContextOptions<InfotecsDataContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlServer(b => b.MigrationsAssembly("InfoTecs.Api"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Result>().HasIndex(n => n.FileName).IsUnique();
        }

        public DbSet<Result> Results { get; set; }
        public DbSet<Value> Values { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}
