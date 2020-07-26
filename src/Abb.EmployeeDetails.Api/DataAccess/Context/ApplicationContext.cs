using Abb.EmployeeDetails.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Abb.EmployeeDetails.Api.DataAccess.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(options =>
            {
                options.HasKey(x => x.Id);
                options.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
