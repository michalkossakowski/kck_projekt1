using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kck_projekt1.View
{
    public class UserView : View
    {
        public UserModel GetUserInfo()
        {
            AnsiConsole.Markup("Nick:");
            var nick = Console.ReadLine();

            AnsiConsole.Markup("Passowrd:");
            var password = Console.ReadLine();

            return new UserModel(nick, password);
        }

    }
}
