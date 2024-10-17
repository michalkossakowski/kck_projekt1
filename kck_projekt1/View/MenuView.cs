using kck_api.Controller;
using Spectre.Console;
using System.Diagnostics;

namespace kck_projekt1.View
{
    public class MenuView : View
    {
        UserModel _loggedUser = null;

        public void MainMenu()
        {
            var userView = new UserView();
            var userController = UserController.GetInstance();
            while (true)
            {
                AnsiConsole.Clear();
                var choice = ShowLoginMenu();
                switch (choice)
                {
                    case "Log in":
                        var user = userView.LoginUser();
                        AnsiConsole.Status()
                        .Spinner(Spinner.Known.BouncingBar)
                        .SpinnerStyle(Style.Parse("darkorange"))
                        .Start("[gold1]Loading[/]", ctx =>
                        {
                            user = userController.GetUser(user);
                            Thread.Sleep(1000);
                        });
                        if (user == null)
                        {
                            AnsiConsole.Markup("[red1]\nWrong nick or password, press anything to continue[/]");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            _loggedUser = user;
                            ShowActionMenu();
                        }
                        break;

                    case "Register":
                        var newUser = userView.RegisterUser();
                        if (newUser == null)
                        {
                            AnsiConsole.Markup("[red1]\nPasswords don't match, press anything to continue[/]");
                            Console.ReadKey();
                            break;
                        }

                        bool isAdded = false;
                        AnsiConsole.Status()
                        .Spinner(Spinner.Known.BouncingBar)
                        .SpinnerStyle(Style.Parse("darkorange"))
                        .Start("[gold1]Loading[/]", ctx =>
                        {
                            isAdded = userController.AddUser(newUser);
                            Thread.Sleep(1000);
                        });

                        if (isAdded)
                            AnsiConsole.Markup("[green1]\nNew user added, press anything to continue[/]");
                        else
                            AnsiConsole.Markup("[red1]\nThis nick is occupied, press anything to continue[/]");
                        Console.ReadKey();
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
        public string ShowLoginMenu()
        {

            AnsiConsole.Write(
            new FigletText("Notes")
                .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Welcome in Notes App choose action:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("")
            .HighlightStyle(Color.DarkOrange)
            .AddChoices(new[] {
                "Log in",
                "Register",
                "Graphic Mode",
                "Exit"
             }));

            return choice;
        }

        public void ShowActionMenu()
        {
            AnsiConsole.Clear();
            var noteView = new NoteView();
            var noteController = NoteController.GetInstance();
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                new FigletText("Menu")
                    .LeftJustified()
                    .Color(Color.Gold1));

                var choice = ActionSelect();
                switch (choice)
                {
                    case "Add note":
                        noteController.AddNote(noteView.WriteNote(_loggedUser));
                        AnsiConsole.Markup("[green1]\nNew note added, press anything to continue[/]");
                        Console.ReadKey();
                        break;

                    case "Latest notes":
                        noteView.ShowLatestNotes(_loggedUser.Id);
                        break;

                    case "Explore notes":
                        while (true)
                        {
                            var noteId = noteView.ExploreNotes(_loggedUser.Id);
                            if (noteId == -1)
                                break;
                            noteView.ShowNote(noteId);
                        }
                        break;

                    case "Calendar":
                        noteView.ShowCalendar(_loggedUser.Id);
                        break;

                    case "Search":
                        var searchingTitle = noteView.SearchNotes();
                        while (true)
                        {
                            var searchingNoteId = noteView.ShowNotesBySearch(_loggedUser.Id, searchingTitle);
                            if (searchingNoteId == -1)
                                break;
                            noteView.ShowNote(searchingNoteId);
                        }
                        break;

                    case "Log out":
                        return;

                    case "Exit":
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }

        }

        public string ActionSelect()
        {
            var rule = new Rule($"[gold1]Hello[/] [darkorange]{_loggedUser.Nick}[/] [gold1]what do you want to do?[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("")
            .HighlightStyle(Color.DarkOrange)
            .AddChoices(new[] {
                            "Add note",
                            "Latest notes",
                            "Explore notes",
                            "Calendar",
                            "Search",
                            "Log out",
                            "Exit"
             }));

            return choice;
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
