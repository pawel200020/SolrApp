using System.Configuration;
using Microsoft.EntityFrameworkCore;
using SqlData.Models;

namespace SqlData.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AppParameter> AppParameters { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
            //optionsBuilder.UseLazyLoadingProxies();
        }

        public AppDbContext()
        {

        }
    }
}
