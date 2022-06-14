using Microsoft.EntityFrameworkCore;
using PredicaTask.Domain;

namespace PredicaTask.Data
{
    public class PredicaTaskDbContext : DbContext
    {
        public DbSet<Weather> Weathers { get; set; }
        public DbSet<User> Users { get; set; }

        public PredicaTaskDbContext(DbContextOptions<PredicaTaskDbContext> options):base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}