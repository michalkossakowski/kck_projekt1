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

        public bool AddUser(UserModel user)
        {
            if(GetUserByNick(user.Nick) == null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserModel GetUser(UserModel user)
        {
            return _context.Users.FirstOrDefault(u => (u.Nick == user.Nick) && (u.Password == user.Password));
        }

        public UserModel GetUserByNick(string nick)
        {
            return _context.Users.FirstOrDefault(u => u.Nick == nick);
        }
    }
}
