using kck_api.Controller;
using kck_api.Database;
using kck_projekt1.View;
using Spectre.Console;
using System.Diagnostics;

namespace kck_projekt1
{
    public class Program
    {

        static void Main(string[] args)
        {
            var menuView = new MenuView();
            menuView.MainMenu();
        }

    }
}
