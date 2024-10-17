using kck_api.Controller;
using Spectre.Console;
using System.Diagnostics;
using System.Windows.Controls;
using Panel = Spectre.Console.Panel;

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
                var layout = new Layout("Root")
                    .SplitRows(
                        new Layout("Title"), 
                        new Layout("Actions")
                            .SplitColumns(
                                new Layout("Left")
                                    .SplitRows(
                                        new Layout("Top"),
                                        new Layout("Bottom")),
                                new Layout("Right")
                                    .SplitRows(
                                        new Layout("Top"),
                                        new Layout("Bottom"))));

                layout["Title"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new FigletText("Notes App")
                                    .LeftJustified()
                                    .Color(Color.Gold1),
                                new Markup("\n"),
                                new Rule("[gold1]Press the[/] [darkorange]{KEY}[/] [gold1]on your keyboard to select the action[/]").RuleStyle("gold1")
                            ),
                            VerticalAlignment.Middle))
                    .Expand());

                layout["Left"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]LOGIN -> {L}[/]"),
                                new Markup("[gold1]Login into your account[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Left"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]REGISTER -> {R}[/]"),
                                new Markup("[gold1]Create a new user account[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]GRAPHIC MODE -> {G}[/]"),
                                new Markup("[gold1]Open graphic version of the application[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                           new Rows(
                                new Markup("[darkorange]EXIT -> {ESC}[/]"),
                                new Markup("[gold1]Close the application[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                AnsiConsole.Write(layout);

                var pressedKey = Console.ReadKey();
                var choice = pressedKey.Key.ToString(); 
                switch (choice)
                {
                    case "L":
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

                    case "R":
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
                            AnsiConsole.Markup("[green1]\nNew user added you can now login into your account, press anything to continue[/]");
                        else
                            AnsiConsole.Markup("[red1]\nThis nick is occupied, press anything to continue[/]");
                        Console.ReadKey();
                        break;

                    case "G":
                        SwitchToGraphicMode();
                        break;

                    case "Escape":
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }

        public void ShowActionMenu()
        {
            AnsiConsole.Clear();
            var noteView = new NoteView();
            var noteController = NoteController.GetInstance();
            while (true)
            {
                AnsiConsole.Clear();
                var layout = new Layout("Root")
                    .SplitRows(
                        new Layout("Title"),
                        new Layout("Actions")
                            .SplitColumns(
                                new Layout("Left")
                                    .SplitRows(
                                        new Layout("Top"),
                                        new Layout("Middle"),
                                        new Layout("Bottom")),
                                new Layout("Right")
                                    .SplitRows(
                                        new Layout("Top"),
                                        new Layout("Middle"),
                                        new Layout("Bottom"))));

                layout["Title"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                            new FigletText("Menu")
                                .LeftJustified()
                                .Color(Color.Gold1),
                            new Markup("\n"),
                            new Rule("[gold1]Press the[/] [darkorange]{KEY}[/] [gold1]on your keyboard to select the action[/]").RuleStyle("gold1"),
                            new Markup("[gold1]Press[/] [darkorange]{ESC}[/] [gold1]to close the application[/]")
                            ),
                            VerticalAlignment.Middle))
                    .Expand());

                layout["Left"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]LATEST NOTES -> {N}[/]"),
                                new Markup("[gold1]Explore latest written notes[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Left"]["Middle"].Update(
                new Panel(
                    Align.Center(
                        new Rows(
                            new Markup("[darkorange]EXPLORE NOTES -> {E}[/]"),
                            new Markup("[gold1]Explore all your notes[/]")
                        ),
                        VerticalAlignment.Middle))
                    .Expand());

                layout["Left"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]CALENDAR -> {C}[/]"),
                                new Markup("[gold1]Explore notes by date[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]ADD NOTE -> {A}[/]"),
                                new Markup("[gold1]Open graphic version of the application[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Middle"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]SEARCH -> {S}[/]"),
                                new Markup("[gold1]Find note by the title[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                           new Rows(
                                new Markup("[darkorange]LOG OUT -> {L}[/]"),
                                new Markup("[gold1]Log out of the current account[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                AnsiConsole.Write(layout);



                var pressedKey = Console.ReadKey();
                var choice = pressedKey.Key.ToString();

                switch (choice)
                {
                    case "A":
                        noteController.AddNote(noteView.WriteNote(_loggedUser));
                        AnsiConsole.Markup("[green1]\nNew note added, press anything to continue[/]");
                        Console.ReadKey();
                        break;

                    case "N":
                        noteView.ShowLatestNotes(_loggedUser.Id);
                        break;

                    case "E":
                        while (true)
                        {
                            var noteId = noteView.ExploreNotes(_loggedUser.Id);
                            if (noteId == -1)
                                break;
                            noteView.ShowNote(noteId);
                        }
                        break;

                    case "C":
                        noteView.ShowCalendar(_loggedUser.Id);
                        break;

                    case "S":
                        var searchingTitle = noteView.SearchNotes();
                        while (true)
                        {
                            var searchingNoteId = noteView.ShowNotesBySearch(_loggedUser.Id, searchingTitle);
                            if (searchingNoteId == -1)
                                break;
                            noteView.ShowNote(searchingNoteId);
                        }
                        break;

                    case "L":
                        return;

                    case "Escape":
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
