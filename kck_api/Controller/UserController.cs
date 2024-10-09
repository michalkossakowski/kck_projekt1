using kck_api.Database;

namespace kck_api.Controller
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserModel GetUser(UserModel user)
        {
            return _context.Users.FirstOrDefault(u => (u.Nick == user.Nick) && (u.Password == user.Password));
        }
    }
}
