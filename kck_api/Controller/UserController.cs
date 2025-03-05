using kck_api.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
                user.Password = HashPassword(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddUserAsync(UserModel user)
        {
            try
            {

                if (GetUserByNick(user.Nick) == null)
                {
                    user.Password = HashPassword(user.Password);
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public UserModel GetUser(UserModel user)
        {
            string hashedPassword = HashPassword(user.Password);
            return _context.Users.FirstOrDefault(u => u.Nick == user.Nick && u.Password == hashedPassword);
        }

        public async Task<UserModel> GetUserAsync(UserModel user)
        {
            string hashedPassword = HashPassword(user.Password);
            return await _context.Users.FirstOrDefaultAsync(u => u.Nick == user.Nick && u.Password == hashedPassword);
        }

        private UserModel GetUserByNick(string nick)
        {
            return _context.Users.FirstOrDefault(u => u.Nick == nick);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
