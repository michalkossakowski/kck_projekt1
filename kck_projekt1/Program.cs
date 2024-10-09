using kck_projekt1.View;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace kck_projekt1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userView = new UserView();

            var user = userView.GetUserInfo();



        }
    }
}
