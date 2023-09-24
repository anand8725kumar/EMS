using Microsoft.EntityFrameworkCore;
using NormalForm.Models.Doman;

namespace NormalForm.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext()
        {
        }

        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
