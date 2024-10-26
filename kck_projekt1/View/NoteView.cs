﻿using kck_api.Controller;
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
            while (true)
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

                var actionRule = new Rule("[gold1]Choose action:[/]");
                actionRule.Style = new Style(Color.Gold1);
                actionRule.Centered();
                AnsiConsole.Write(actionRule);

                Console.WriteLine();

                var colWidth = 30;
                var table = new Table()
                    .Centered()
                    .BorderColor(Color.White)
                    .AddColumn(new TableColumn("[darkorange]Back -> {B}[/]").Centered().Width(colWidth)) 
                    .AddColumn(new TableColumn("[darkorange]Edit -> {E}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]Delete -> {D}[/]").Centered().Width(colWidth))
                    .Expand(); 

                AnsiConsole.Write(table);


                var pressedKey = Console.ReadKey();
                var choice = pressedKey.Key.ToString();
                switch (choice)
                {
                    case "B":
                        return;
                    case "E":
                        ShowEditNote(note);
                        Console.WriteLine();
                        var editedNoteRule = new Rule("[gold1]Edited note:[/]");
                        editedNoteRule.Style = new Style(Color.Gold1);
                        editedNoteRule.Centered();
                        AnsiConsole.Write(editedNoteRule);
                        Console.WriteLine();
                        ShowNotes(new List<NoteModel> {note});
                        AnsiConsole.Markup("[green1]\nNote successful edited, press anything to continue[/]");
                        Console.ReadKey();
                        return;
                    case "D":
                        noteController.RemoveNote(noteId);
                        return;
                }
            }
        }

        public void ShowEditNote(NoteModel note)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,"Edit")
                .Centered()
                .Color(Color.Gold1));

            var oldNoteRule = new Rule("[gold1]You are editing note:[/]");
            oldNoteRule.Style = new Style(Color.Gold1);
            oldNoteRule.Centered();
            AnsiConsole.Write(oldNoteRule);

            Console.WriteLine();
            ShowNotes(new List<NoteModel> { note });
            Console.WriteLine();

            var rule = new Rule($"[gold1]Edit your note:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);
            Console.WriteLine();

            AnsiConsole.MarkupLine("[grey35]Leave blank and press {Enter} to use old title[/]");
            var newTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]title[/]")
            .DefaultValue(note.Title)
            .HideDefaultValue());

            AnsiConsole.MarkupLine("[grey35]\nLeave blank and press {Enter} to use old category[/]");
            var newCategory = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter new[/] [darkorange]category[/]")
            .DefaultValue(note.Category)
            .HideDefaultValue());

            ClipboardService.SetText(note.Content);
            AnsiConsole.MarkupLine("[grey35]\nPress {Ctrl+V} to edit old content[/]");
            var newContent = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter new[/] [darkorange]content:[/]"));

            var noteController = NoteController.GetInstance();
            noteController.EditNote(note.Id, newTitle, newCategory, newContent);
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
            var notes = noteController.GetLatestNotesByUserId(userId,6);

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
            string category = "";
            while(category.Length <= 1)
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

                Console.WriteLine("");

                var colWidth = 30;
                var table = new Table()
                    .Centered()
                    .BorderColor(Color.White)
                    .AddColumn(new TableColumn("[darkorange]Studies -> {S}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]Work -> {W}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]Home -> {H}[/]").Centered().Width(colWidth))
                    .Expand();

                table.AddRow("[darkorange]Hobby -> {F}[/]", "[darkorange]Other -> {O}[/]", "[darkorange]Custom -> {C}[/]");

                AnsiConsole.Write(table);


                var pressedKey = Console.ReadKey(true);
                category = pressedKey.Key.ToString();

                switch (category)
                {
                    case "S":
                        category = "Studies";
                        break;
                    case "W":
                        category = "Work";
                        break;
                    case "H":
                        category = "Home";
                        break;
                    case "F":
                        category = "Hobby";
                        break;
                    case "O":
                        category = "Other";
                        break;
                    case "C":
                        category = AnsiConsole.Prompt(
                        new TextPrompt<string>("[gold1]\nEnter custom[/] [darkorange]category:[/]"));
                        break;
                }
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

        public DateTime ChooseMonth(int userId)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font, "MONTHS")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Select a month:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();

            var allNotes = noteController.GetNotesByUserId(userId);
            var yearAndMonth = new HashSet<(int,int)>();
            foreach (var note in allNotes)
            {
                yearAndMonth.Add((note.ModifiedDate.Year,note.ModifiedDate.Month));
            }
            var list = yearAndMonth.OrderBy(date => date).Select(date => new DateTime(date.Item1, date.Item2, 1).ToString("MM.yyyy")).ToList();
            list.Add("Back");
            list.Reverse();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("")
            .HighlightStyle(Color.DarkOrange)
            .PageSize(4)
            .AddChoices(list));


            DateTime date;
            switch (choice)
            {
                case "Back":
                    AnsiConsole.Clear();
                    return DateTime.MinValue;
                default:
                    var month = int.Parse(choice.Split('.')[0]);
                    var year = int.Parse(choice.Split('.')[1]);
                    date = new DateTime(year, month, 1);
                    break;
            }
            return date;
        }

        public int ShowNotesByMonth(int userId, DateTime date)
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

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserIdAndMonth(userId, date);
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
