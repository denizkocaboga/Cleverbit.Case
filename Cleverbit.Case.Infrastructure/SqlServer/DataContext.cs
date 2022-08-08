using Cleverbit.Case.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Cleverbit.Case.Infrastructure.SqlServer
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {            
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Region> Regions { get; set; }
    }
}