using kck_api.Controller;
using kck_api.Database;
using kck_projekt1.View;
using kck_projekt2;
using System.Diagnostics;

namespace kck_projekt1
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            var x = Console.ReadLine();

            if (x == "1")
            {
                RunWPF();
            }

            if(x== "2")
            {
                string exePath = Path.Combine(Directory.GetCurrentDirectory(), "kck_projekt2.exe");

                    // Tworzenie nowego procesu do uruchomienia exe
                    Process process = new Process();
                    process.StartInfo.FileName = exePath;

                    // Uruchomienie procesu
                    process.Start();

                    // Zakończenie aplikacji konsolowej
                    Environment.Exit(0); // lub `return;` aby zakończyć metodę Main
            }



            ApplicationDbContext context = new ApplicationDbContext();
            var userController = new UserController(context);

            var menuView = new MenuView();
            var userView = new UserView();

            var choice = menuView.ShowStartMenu();
            switch (choice)
            {
                case "Zaloguj się":
                    var user = userView.LoginUser();
                    user = userController.GetUser(user);
                    if (user == null)
                    {
                        Console.WriteLine("Błędne dane logowania");
                    }
                    else
                    {
                        Console.WriteLine("Zalogowano pomyślnie");
                        Console.Clear();
                        menuView.ShowActionMenu();
                    }
                    break;

                case "Zarejestruj się":
                    userController.AddUser(userView.AddNewUser());
                    Console.WriteLine("Dodano nowego użytkownika");
                    break;


                case "Wyjście":
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }


        static void RunWPF()
        {
            var wpfApp = new App();
            var mainWindow = new MainWindow();
            mainWindow.InitializeComponent();
            wpfApp.Run(mainWindow);
        }

    }
}
