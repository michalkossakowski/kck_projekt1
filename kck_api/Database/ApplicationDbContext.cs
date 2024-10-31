using Microsoft.EntityFrameworkCore;

namespace kck_api.Database
{
    public class ApplicationDbContext : DbContext
    {
        private static ApplicationDbContext _instance;

        public static ApplicationDbContext GetInstance()
        {
            if (_instance == null)
                _instance = new ApplicationDbContext();
            return _instance;
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<NoteModel> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=kck_projektDb;Trusted_Connection=True;");
        }
    }
}
