using kck_api.Database;

namespace kck_api.Controller
{
    public class UserController
    {

        private static UserController _instance;

        private readonly ApplicationDbContext _context;

        public UserController()
        {
            _context = ApplicationDbContext.GetInstance();
        }
        public static UserController GetInstance()
        {
            if (_instance == null)
                _instance = new UserController();
            return _instance;
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
