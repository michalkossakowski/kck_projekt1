using kck_api.Controller;
using Spectre.Console;
using static Azure.Core.HttpHeader;

namespace kck_projekt1.View
{
    public class NoteView
    {
        public NoteModel WriteNote(UserModel user)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Create")
            .LeftJustified()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Write a new note:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine("");

            var title = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]title:[/]"));

            var note = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]note:[/]"));

            var category = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.DarkOrange)
            .Title("[gold1]Select[/] [darkorange]category:[/]")
            .AddChoices(new[] {
                            "Studies",
                            "Work",
                            "Home",
                            "Hobby",
                            "Other",
                            "[gray50]Custom[/]"
             }));

            if(category == "[gray50]Custom[/]")
            {
                category = AnsiConsole.Prompt(
                new TextPrompt<string>("[gold1]Enter[/] [darkorange]category:[/]"));
            }
            else
            {
                AnsiConsole.Markup("[gold1]Choosen[/] [darkorange]category: [/]" + category);
                Console.WriteLine("");
            }
           
            return new NoteModel(user.Id, title, note, category);
        }
        public int ExploreNotes(int userId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Explore")
            .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Choose note to show:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserId(userId);


            if (notes.Count == 0)
            {
                AnsiConsole.Markup("[red1]\nYou don't have any notes, press anything to continue[/]");
                Console.ReadKey();
                return -1;
            }
            else
            {
                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.Category})[/] - {n.ModifiedDate}").ToList();
                list.Add("Back");
                list.Reverse();

                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("")
                .HighlightStyle(Color.DarkOrange)
                .PageSize(10)
                .AddChoices(list));

                var id = choice == "Back" ? -1 : int.Parse(choice.Split(':')[0]);

                return id;
            }

        }

        public void ShowNote(int noteId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Note")
                .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Your chosen note:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var note = noteController.GetNoteById(noteId);

            var table = new Table()
                .BorderColor(Color.Grey70)
                .Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn($"[darkorange]{note.Title}[/] [gold1]({note.Category})[/]").Centered());
            table.AddRow($"[darkorange]{note.ModifiedDate}[/]");
            var panel = new Panel(note.Content)
                .Expand()
                .BorderColor(Color.Grey50);
            table.AddRow(panel);
            AnsiConsole.Write(table);

            Console.WriteLine();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.DarkOrange)
            .AddChoices(new[] {
                "Back",
                "Delete"
             }));

            switch (choice)
            {
                case "Back":
                    return;
                case "Delete":
                    noteController.RemoveNote(noteId);
                    return;
            }
        }

        public void ShowLatestNotes(int userId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Latest")
                .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Your latest notes:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetLatestNotesByUserId(userId,4);

            if (notes.Count == 0)
            {
                AnsiConsole.Markup("[red1]\nYou don't have any notes, press anything to continue[/]");
            }
            else
            {
                ShowNotesBasics(notes);
                AnsiConsole.Markup("[green1]\nHere are your notes, press anything to continue[/]");
            }

            Console.ReadKey();
        }

        public void ShowCalendar(int userId)
        {
            while (true)
            {
                AnsiConsole.Clear();

                AnsiConsole.Write(
                new FigletText("Calendar")
                    .LeftJustified()
                    .Color(Color.Gold1));

                var titleRule = new Rule("[gold1]Notes in current month:[/]");
                titleRule.Style = new Style(Color.Gold1);
                titleRule.LeftJustified();
                AnsiConsole.Write(titleRule);

                Console.WriteLine();

                var currentDate = DateTime.Now;
                var calendar = new Calendar(currentDate.Year, currentDate.Month);

                var noteController = NoteController.GetInstance();
                var currentMonthNotes = noteController.GetCurrentMonthNotesByUserId(userId, currentDate);
            
                var days = new HashSet<string>();
                foreach ( var note in currentMonthNotes)
                {
                    calendar.AddCalendarEvent(note.ModifiedDate.Year, note.ModifiedDate.Month, note.ModifiedDate.Day);
                    days.Add($"{note.ModifiedDate.Day.ToString()}.{note.ModifiedDate.Month}.{note.ModifiedDate.Year}");
                }
            
           
                calendar.HighlightStyle(Style.Parse("gold1 bold"));
                calendar.HeaderStyle(Style.Parse("darkorange bold"));
                AnsiConsole.Write(calendar);

                if (currentMonthNotes.Count == 0)
                {
                    AnsiConsole.Markup($"[red1]\nYou dont have any notes in current month, press anything to continue[/]");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("");
                    var rule = new Rule("[gold1]Select day:[/]");
                    rule.Style = new Style(Color.Gold1);
                    rule.LeftJustified();
                    AnsiConsole.Write(rule);

                    var list = days.ToList();
                    list.Add("Back");
                    list.Reverse();


                    var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("")
                    .HighlightStyle(Color.DarkOrange)
                    .PageSize(10)
                    .AddChoices(list));

                    switch (choice)
                    {
                        case "Back":
                            AnsiConsole.Clear();
                            return;
                        default:
                            var day = int.Parse(choice.Split('.')[0]);
                            ShowNotesByDay(userId, currentDate, day);
                            break;
                    }
                }
            }
        }
        public void ShowNotesByDay(int userId,DateTime date, int day)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText($"{day}.{date.Month}.{date.Year}")
                .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule($"[gold1]Your notes from[/] [darkorange]{day}.{date.Month}.{date.Year}:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndDay(userId, date, day);

            ShowNotesBasics(notes);

            AnsiConsole.Markup("[green1]\nHere are your notes from chosen day, press anything to continue[/]");
            Console.ReadKey();
            AnsiConsole.Clear();
        }

        public void ShowNotesBasics(List<NoteModel> notes)
        {
            foreach (var note in notes)
            {
                var table = new Table()
                    .BorderColor(Color.Grey70)
                    .Border(TableBorder.Rounded);
                table.AddColumn(new TableColumn($"[darkorange]{note.Title}[/] [gold1]({note.Category})[/]").Centered());
                table.AddRow(note.Content);
                AnsiConsole.Write(table);

                Thread.Sleep(200);
            }
        }

        public string SearchNotes()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText("Search")
            .LeftJustified()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Search in your notes:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine("");

            var searchingTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]title[/] [gold1]of the note:[/]"));

            return searchingTitle;
        }


        public int ShowNotesBySearch(int userId, string searchingTitle)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Results")
            .LeftJustified()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Choose note to show:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            AnsiConsole.Markup("");
            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndTitle(userId, searchingTitle);
            if (notes.Count == 0)
            {
                AnsiConsole.Markup($"[red1]\nYou dont have notes with[/] [darkorange]'{searchingTitle}'[/] [red1]in the title, press anything to continue[/]");
                Console.ReadKey();
                return -1;
            }
            else
            {
                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.Category})[/] - {n.ModifiedDate}").ToList();
                list.Add("Back");
                list.Reverse();

                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("")
                .HighlightStyle(Color.DarkOrange)
                .PageSize(10)
                .AddChoices(list));

                var id = choice == "Back" ? -1 : int.Parse(choice.Split(':')[0]);

                return id;
            }
        }
    }
}
