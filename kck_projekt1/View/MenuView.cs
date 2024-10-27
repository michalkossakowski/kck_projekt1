using kck_api.Controller;
using Spectre.Console;
using System.Diagnostics;

namespace kck_projekt1.View
{
    public class MenuView
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
                                new FigletText(Program.font, "Notes App")
                                    .LeftJustified()
                                    .Color(Color.Gold1),
                                new Rule("[gold1]Press the[/] [darkorange]{KEY}[/] [gold1]on your keyboard to select the action[/]").RuleStyle("gold1")
                            ),
                            VerticalAlignment.Middle))
                    .Expand());

                layout["Left"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🔒 LOGIN ▶ {L}[/]"),
                                new Markup("[gold1]Log in to your account[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Left"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🖥️ GRAPHIC MODE ▶ {G}[/]"),
                                new Markup("[gold1]Open graphic version of the application[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]✍  REGISTER ▶ {R}[/]"),
                                new Markup("[gold1]Create a new user account[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                           new Rows(
                                new Markup("[darkorange]❌ EXIT ▶ {ESC}[/]"),
                                new Markup("[gold1]Close the application[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                try
                {
                    AnsiConsole.Write(layout);
                }
                catch
                {
                    AnsiConsole.Markup("[red1]⛔ Window is to small, expand the window please...[/]");
                }

                var pressedKey = Console.ReadKey(true);
                var choice = pressedKey.Key.ToString();
                switch (choice)
                {
                    case "L":
                        var user = userView.LoginUser();
                        AnsiConsole.Status()
                        .Spinner(Spinner.Known.Moon)
                        .SpinnerStyle(Style.Parse("darkorange"))
                        .Start("[gold1]Logging in[/]", ctx =>
                        {
                            user = userController.GetUser(user);
                            Thread.Sleep(1500);
                        });

                        if (user == null)
                        {
                            AnsiConsole.Markup("[red1]\n⛔ Wrong nick or password, press anything to continue...[/]");
                            Console.ReadKey(true);
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
                            AnsiConsole.Markup("[red1]\n⛔ Passwords don't match, press anything to continue...[/]");
                            Console.ReadKey(true);
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
                            AnsiConsole.Markup("[green1]\n✅ New user added you can now login into your account, press anything to continue...[/]");
                        else
                            AnsiConsole.Markup("[red1]\n⛔ This nick is occupied, press anything to continue...[/]");
                        Console.ReadKey(true);
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
                                        new Layout("MiddleTop"),
                                        new Layout("MiddleBottom"),
                                        new Layout("Bottom")),
                                new Layout("Right")
                                    .SplitRows(
                                        new Layout("Top"),
                                        new Layout("MiddleTop"),
                                        new Layout("MiddleBottom"),
                                        new Layout("Bottom"))));

                layout["Title"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                            new FigletText(Program.font, "Menu")
                                .LeftJustified()
                                .Color(Color.Gold1),
                            new Rule("[gold1]Press the[/] [darkorange]{KEY}[/] [gold1]on your keyboard to select the action[/]").RuleStyle("gold1"),
                            new Markup("[gold1]Press[/] [darkorange]{ESC}[/] [gold1]to log out[/]")
                            ),
                            VerticalAlignment.Middle))
                    .Expand()).Size(11);

                layout["Left"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🖋️ ADD NOTE ▶ {A}[/]"),
                                new Markup("[gold1]Create a new note[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Left"]["MiddleTop"].Update(
                new Panel(
                    Align.Center(
                        new Rows(
                            new Markup("[darkorange]🔍 SEARCH ▶ {S}[/]"),
                            new Markup("[gold1]Find notes by the title[/]")
                        ),
                        VerticalAlignment.Middle))
                    .Expand());

                layout["Left"]["MiddleBottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                            new Markup("[darkorange]🌍 EXPLORE ALL NOTES ▶ {E}[/]"),
                            new Markup("[gold1]Explore all your notes[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Left"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🔠 FIND BY CATEGORY ▶ {F}[/]"),
                                new Markup("[gold1]Find notes by chosen category[/]")

                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Top"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🆕 LATEST NOTES ▶ {L}[/]"),
                                new Markup("[gold1]Your latest notes preview[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["MiddleTop"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]🗓️ CALENDAR ▶ {C}[/]"),
                                new Markup("[gold1]Current month notes preview[/]")

                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["MiddleBottom"].Update(
                    new Panel(
                        Align.Center(
                            new Rows(
                                new Markup("[darkorange]📌 EXPLORE BY MONTH ▶ {M}[/]"),
                                new Markup("[gold1]Explore notes by chosen month[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                layout["Right"]["Bottom"].Update(
                    new Panel(
                        Align.Center(
                           new Rows(
                                new Markup("[darkorange]📅 FIND BY DATE ▶ {D}[/]"),
                                new Markup("[gold1]Find notes by chosen date[/]")
                            ),
                            VerticalAlignment.Middle))
                        .Expand());

                try
                {
                    AnsiConsole.Write(layout);
                }
                catch
                {
                    AnsiConsole.Markup("[red1]⛔ Window is to small, expand the window please...[/]");
                }

                var pressedKey = Console.ReadKey(true);
                var choice = pressedKey.Key.ToString();

                switch (choice)
                {
                    case "A":
                        var newNote = noteView.CreateNote(_loggedUser);
                        noteController.AddNote(newNote);
                        Console.WriteLine();
                        var newNoteRule = new Rule("[gold1]Added note:[/]");
                        newNoteRule.Style = new Style(Color.Gold1);
                        newNoteRule.Centered();
                        AnsiConsole.Write(newNoteRule);
                        Console.WriteLine();
                        noteView.ShowNotes(new List<NoteModel> { newNote });
                        AnsiConsole.Markup("[green1]\n✅ New note added, press anything to continue...[/]");
                        Console.ReadKey(true);
                        break;

                    case "L":
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

                    case "F":
                        var categoryFilter = noteView.ChooseCategoryToFilter();
                        while (true)
                        {
                            var chosenNoteId = noteView.ShowNotesByCategory(_loggedUser.Id, categoryFilter);
                            if (chosenNoteId == -1)
                                break;
                            noteView.ShowNote(chosenNoteId);
                        }
                        break;

                    case "D":
                        DateTime chosenDate = noteView.ChooseDate();
                        while (chosenDate!= DateTime.MinValue)
                        {
                            var chosenNoteId = noteView.ExploreNotesByDate(_loggedUser.Id, chosenDate);
                            if (chosenNoteId == -1)
                                break;
                            noteView.ShowNote(chosenNoteId);
                        }
                        break;

                    case "M":
                        var chosenMonth = noteView.ChooseMonth(_loggedUser.Id);
                        while (chosenMonth != DateTime.MinValue)
                        {
                            var chosenNoteId = noteView.ShowNotesByMonth(_loggedUser.Id, chosenMonth);
                            if (chosenNoteId == -1)
                                break;
                            noteView.ShowNote(chosenNoteId);
                        }
                        break;

                    case "Escape":
                        return;
                }
            }

        }

        public static void SwitchToGraphicMode()
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "kck_projekt2.exe");
            process.Start();
            Environment.Exit(0);
        }
    }
}
