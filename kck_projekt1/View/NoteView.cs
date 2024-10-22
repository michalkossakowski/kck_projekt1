using kck_api.Controller;
using Spectre.Console;
using TextCopy;
using Calendar = Spectre.Console.Calendar;

namespace kck_projekt1.View
{
    public class NoteView
    {
        public NoteModel WriteNote(UserModel user)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,"Create")
            .Centered()
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
                            "[grey50]Custom[/]"
             }));

            if(category == "[grey50]Custom[/]")
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
        public void ShowNote(int noteId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font, "Note")
                .Centered()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Your chosen note:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.Centered();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var note = noteController.GetNoteById(noteId);

            ShowNotes(new List<NoteModel> { note });

            Console.WriteLine();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.DarkOrange)
            .AddChoices(new[] {
                "Back",
                "Edit",
                "Delete"
             }));

            switch (choice)
            {
                case "Back":
                    return;
                case "Edit":
                    ShowEditNote(note);
                    return;
                case "Delete":
                    noteController.RemoveNote(noteId);
                    return;
            }
        }

        public void ShowEditNote(NoteModel note)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,"Edit")
                .Centered()
                .Color(Color.Gold1));

            var rule = new Rule($"[gold1]Editing[/] [darkorange]'{note.Title}'[/] [gold1]({note.Category})[/] [darkorange]{note.ModifiedDate}:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.Centered();
            AnsiConsole.Write(rule);


            AnsiConsole.Markup($"[gold1]\nOld[/] [darkorange]content:[/] [grey70]{note.Content}[/]\n");
            ClipboardService.SetText(note.Content);
            AnsiConsole.Markup("[grey35]\n(Press {Ctrl+V} to use old content)[/]");
            var newContent = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]\nEnter new[/] [darkorange]content:[/]"));

            var noteController = NoteController.GetInstance();
            noteController.EditNoteContent(note.Id, newContent);
        }

        public void ShowLatestNotes(int userId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,"Latest")
                .Centered()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Your latest notes:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.Centered();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetLatestNotesByUserId(userId,3);

            if (notes.Count == 0)
            {
                AnsiConsole.Markup("[red1]\nYou don't have any notes, press anything to continue[/]");
            }
            else
            {
                ShowNotes(notes);
                AnsiConsole.Markup("[green1]\nHere are your notes, press anything to continue[/]");
            }

            Console.ReadKey();
        }
        public int ExploreNotes(int userId)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font, "Explore")
            .Centered()
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

        public void ShowCalendar(int userId)
        {
            while (true)
            {
                AnsiConsole.Clear();

                AnsiConsole.Write(
                new FigletText(Program.font,"Calendar")
                    .Centered()
                    .Color(Color.Gold1));

                var titleRule = new Rule("[gold1]Notes in current month:[/]");
                titleRule.Style = new Style(Color.Gold1);
                titleRule.Centered();
                AnsiConsole.Write(titleRule);

                Console.WriteLine();

                var currentDate = DateTime.Now;
                var calendar = new Calendar(currentDate.Year, currentDate.Month).Centered();

                var noteController = NoteController.GetInstance();
                var currentMonthNotes = noteController.GetCurrentMonthNotesByUserId(userId, currentDate);
            
                var days = new HashSet<int>();
                foreach ( var note in currentMonthNotes)
                {
                    calendar.AddCalendarEvent(note.ModifiedDate.Year, note.ModifiedDate.Month, note.ModifiedDate.Day);
                    days.Add(note.ModifiedDate.Day);
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

                    var list = days.OrderBy(day => day).Select(day => new DateTime(currentDate.Year, currentDate.Month, day).ToString("dd.MM.yyyy")).ToList();
                    list.Add("Back");
                    list.Reverse();


                    var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("")
                    .HighlightStyle(Color.DarkOrange)
                    .PageSize(4)
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
            new FigletText(Program.font,$"{day}.{date.Month}.{date.Year}")
                .Centered()
                .Color(Color.Gold1));

            var rule = new Rule($"[gold1]Your notes from[/] [darkorange]{day}.{date.Month}.{date.Year}:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.Centered();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndDay(userId, date, day);

            ShowNotes(notes);

            AnsiConsole.Markup("[green1]\nHere are your notes from chosen day, press anything to continue[/]");
            Console.ReadKey();
            AnsiConsole.Clear();
        }

        public string SearchNotes()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font,"Search")
            .Centered()
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
            new FigletText(Program.font,"Results")
            .Centered()
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

        public string ChooseCategoryToFilter()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font, "Filter")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Select a category to filter:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var category = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.DarkOrange)
            .AddChoices(new[] {
                                        "Studies",
                                        "Work",
                                        "Home",
                                        "Hobby",
                                        "Other",
                                        "[grey50]Custom[/]"
             }));

            if (category == "[grey50]Custom[/]")
            {
                category = AnsiConsole.Prompt(
                new TextPrompt<string>("[gold1]Enter custom[/] [darkorange]category:[/]"));
            }

            return category;
        }

        public int ShowNotesByCategory(int userId, string categoryFilter)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font, "Results")
            .Centered()
                .Color(Color.Gold1));

            var rule = new Rule("[gold1]Choose note to show:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            AnsiConsole.Markup("");
            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndCategory(userId, categoryFilter);
            if (notes.Count == 0)
            {
                AnsiConsole.Markup($"[red1]\nYou dont have notes from[/] [darkorange]'{categoryFilter}'[/] [red1]category, press anything to continue[/]");
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

        public void ShowNotes(List<NoteModel> notes)
        {
            var columns = new List<Table>();

            int columnWidth = 35;

            foreach (var note in notes)
            {
                var panel = new Panel(note.Content)
                    .Expand()
                    .BorderColor(Color.Black);

                var table = new Table()
                    .BorderColor(Color.Grey35)
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn($"[darkorange]{note.Title}[/] [gold1]({note.Category})[/]")
                    .Centered()
                    .Width(columnWidth))
                    .AddRow($"[grey50]{note.ModifiedDate}[/]")
                    .AddRow(panel)
                    .Centered();

                columns.Add(table);
            }

            AnsiConsole.Write(new Columns(columns));
        }

        
    }
}
