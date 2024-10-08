using kck_projekt1.Database;

namespace kck_projekt1.Repositories
{
    public class UserRepository : Repository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChangesAsync();
        }
    }
}
