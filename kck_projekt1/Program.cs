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
            ApplicationDbContext context = new ApplicationDbContext();
            var userController = new UserController(context);

            var menuView = new MenuView();
            var userView = new UserView();

            while (true)
            {
                var choice = menuView.ShowStartMenu();
                switch (choice)
                {
                    case "Log in":
                        var user = userView.LoginUser();
                        AnsiConsole.Status()
                        .Spinner(Spinner.Known.BouncingBar)
                        .SpinnerStyle(Style.Parse("blue"))
                        .Start("[aqua]Loading[/]", ctx =>
                        {
                            user = userController.GetUser(user);
                        });
                        if (user == null)
                        {
                            Console.WriteLine("Wrong nick or password, press anything to try again");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            menuView.ShowActionMenu(user);
                        }
                        break;

                    case "Register":
                        userController.AddUser(userView.AddNewUser());
                        Console.WriteLine("New user added");
                        Console.ReadLine();
                        break;

                    case "Graphic Mode":
                        SwitchToGraphicMode();
                        break;

                    case "Exit":
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }

        public static void SwitchToGraphicMode()
        {
            string exePath = Path.Combine(Directory.GetCurrentDirectory(), "kck_projekt2.exe");
            Process process = new Process();
            process.StartInfo.FileName = exePath;
            process.Start();
            Environment.Exit(0);
        }
    }
}
