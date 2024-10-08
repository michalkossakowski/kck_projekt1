using Microsoft.EntityFrameworkCore;

namespace kck_projekt1.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=kckDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }
    }
}
