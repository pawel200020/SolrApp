using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SearchEngine.Context;

namespace SqlData.Context
{
    public class AppContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            options.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
            return new AppDbContext(options.Options);
        }
    }
}
