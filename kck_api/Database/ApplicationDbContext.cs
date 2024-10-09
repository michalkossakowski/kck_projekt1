using Microsoft.EntityFrameworkCore;

namespace kck_api.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<NoteModel> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=kckDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }
    }
}
