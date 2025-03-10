﻿using kck_api.Controller;
using Spectre.Console;
using System.Globalization;
using System;
using TextCopy;
using Calendar = Spectre.Console.Calendar;
using Microsoft.VisualBasic.ApplicationServices;

namespace kck_projekt1.View
{
    public class NoteView
    {
        protected readonly NoteController _noteController;
        protected readonly CategoryController _categoryController;
        public NoteView()
        {
            _noteController = NoteController.GetInstance();
        }
        public NoteModel CreateNote(UserModel user)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,"ADD NOTE")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Create a new note:[/]");
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
           int CategoryId = _categoryController.GetOrCreateCategoryIdAsync(category).Result;
            return new NoteModel(user.Id, title, note, CategoryId);
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

                var note = _noteController.GetNoteById(noteId);

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
                    .AddColumn(new TableColumn("[darkorange]Back ▶ {B}[/]").Centered().Width(colWidth)) 
                    .AddColumn(new TableColumn("[darkorange]Edit ▶ {E}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]Delete ▶ {D}[/]").Centered().Width(colWidth))
                    .Expand(); 

                AnsiConsole.Write(table);


                ConsoleKeyInfo pressedKey;
                string choice = "";
                var choices = new List<string> { "B", "E", "D"};
                while (!choices.Contains(choice))
                {
                    pressedKey = Console.ReadKey(true);
                    choice = pressedKey.Key.ToString();
                }

                switch (choice)
                {
                    case "B":
                        return;
                    case "E":
                        Console.WriteLine();
                        ShowEditNote(note);
                        return;
                    case "D":
                        var sure = ShowConfirmation("\nAre you sure you want to delete the note ?");
                        if (sure)
                        {
                            _noteController.RemoveNote(noteId);
                            AnsiConsole.Markup("[green1]\n✅ Note successful deleted, press anything to continue...[/]");
                        }
                        else
                        {
                            AnsiConsole.Markup("[green1]\n⏪ Deleting note canceled, press anything to continue...[/]");
                        }
                        Console.ReadKey(true);
                        return;
                }
            }
        }

        public bool ShowConfirmation(string message)
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.DarkOrange)
            .Title($"[gold1]{message}[/]")
            .AddChoices(new[] {
                "Yes",
                "No"
            }));

            if(choice == "Yes") 
                return true;
            return false;
        }

        public void ShowEditNote(NoteModel note)
        {
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
            string category = _categoryController.GetCategoryByIdAsync(note.CategoryId).Result.Name;
            AnsiConsole.MarkupLine("[grey35]\nLeave blank and press {Enter} to use old category[/]");
            var newCategory = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter new[/] [darkorange]category[/]")
            .DefaultValue(category)
            .HideDefaultValue());

            ClipboardService.SetText(note.Content);
            AnsiConsole.MarkupLine("[grey35]\nPress {Ctrl+V} to edit old content[/]");
            var newContent = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter new[/] [darkorange]content:[/]"));
            int newCategoryId = _categoryController.GetOrCreateCategoryIdAsync(newCategory).Result;
            var sure = ShowConfirmation("\nAre you sure you want to save changes ?");
            if(sure){
                _noteController.EditNote(note.Id, newTitle, newCategoryId, newContent);

                Console.WriteLine();
                var editedNoteRule = new Rule("[gold1]Edited note:[/]");
                editedNoteRule.Style = new Style(Color.Gold1);
                editedNoteRule.Centered();
                AnsiConsole.Write(editedNoteRule);
                Console.WriteLine();
                ShowNotes(new List<NoteModel> { note });
                AnsiConsole.Markup("[green1]\n✅ Note successful edited, press anything to continue...[/]");
            }
            else{
                AnsiConsole.Markup("[green1]\n⏪ Note edit canceled, press anything to continue...[/]");
            }
            Console.ReadKey(true);

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

            var notes = _noteController.GetLatestNotesByUserId(userId,6);

            if (notes.Count == 0)
            {
                AnsiConsole.Markup("[red1]⛔ You don't have any notes, press anything to continue...[/]");
            }
            else
            {
                ShowNotes(notes);
                AnsiConsole.Markup("[green1]\n✅ Here are your notes, press anything to continue...[/]");
            }

            Console.ReadKey(true);
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

            List<NoteModel> notes = null;
            AnsiConsole.Status()
            .Spinner(Spinner.Known.Moon)
            .SpinnerStyle(Style.Parse("darkorange"))
            .Start("[gold1]Loading[/]", ctx =>
            {
                notes = _noteController.GetNotesByUserId(userId);
            });

            if (notes?.Count == 0)
            {
                AnsiConsole.Markup("[red1]\n⛔ You don't have any notes, press anything to continue...[/]");
                Console.ReadKey(true);
                return -1;
            }
            else
            {
                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.CategoryId})[/] - {n.ModifiedDate}").ToList();
                list.Insert(0,"Back");


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

                var currentMonthNotes = _noteController.GetNotesByUserIdAndMonth(userId, currentDate);
            
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
                    AnsiConsole.Markup($"[red1]\n⛔ You don't have any notes in current month, press anything to continue...[/]");
                    Console.ReadKey(true);
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
                    list.Reverse();
                    list.Insert(0, "Back");


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
                            ShowNotesByDay(userId, new DateTime(currentDate.Year, currentDate.Month, day));
                            break;
                    }
                }
            }
        }

        public void ShowNotesByDay(int userId,DateTime date)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font,$"{date.Day}.{date.Month}.{date.Year}")
                .Centered()
                .Color(Color.Gold1));

            var rule = new Rule($"[gold1]Your notes from[/] [darkorange]{date.Day}.{date.Month}.{date.Year}:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.Centered();
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var notes = _noteController.GetNotesByUserIdAndDay(userId, date);

            ShowNotes(notes);

            AnsiConsole.Markup("[green1]\n✅ Here are your notes from chosen day, press anything to continue...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();
        }

        public string SearchNotes(int userId)
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

            if (!_noteController.IsUserHasAnyNotes(userId))
            {
                AnsiConsole.Markup("[red1]\n⛔ You don't have any notes, press anything to continue...[/]");
                Console.ReadKey(true);
                return null;
            }

            Console.WriteLine("");

            var searchingTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]title[/] [gold1]of the note:[/]"));

            return searchingTitle;
        }

        public int ShowNotesBySearch(int userId, string searchingTitle, bool fromBack)
        {

            List<NoteModel> notes = null;
            if (fromBack)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                new FigletText(Program.font, "Search")
                .Centered()
                .Color(Color.Gold1));
                notes = _noteController.GetNotesByUserIdAndTitle(userId, searchingTitle);
            }

            if (!fromBack)
            {
                AnsiConsole.Status()
                .Spinner(Spinner.Known.Moon)
                .SpinnerStyle(Style.Parse("darkorange"))
                .Start("[gold1]Loading[/]", ctx =>
                {
                    notes = _noteController.GetNotesByUserIdAndTitle(userId, searchingTitle);
                    Thread.Sleep(1000);
                });
                Console.WriteLine();
            }

            if (notes?.Count == 0)
            {
                AnsiConsole.Markup($"[red1]⛔ You don't have notes with[/] [darkorange]'{searchingTitle}'[/] [red1]in the title, press anything to continue...[/]");
                Console.ReadKey(true);
                return -1;
            }
            else
            {
                var rule = new Rule($"[gold1]Notes with: '[/][darkorange]{searchingTitle}[/][gold1]' in title:[/]");
                rule.Style = new Style(Color.Gold1);
                rule.LeftJustified();
                AnsiConsole.Write(rule);

                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.CategoryId})[/] - {n.ModifiedDate}").ToList();
                list.Insert(0, "Back");

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

        public string ChooseCategoryToFilter(int userId)
        {

            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font, "CATEGORIES")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Select a category to filter:[/]");
                rule.Style = new Style(Color.Gold1);
                rule.LeftJustified();
                AnsiConsole.Write(rule);

            if (!_noteController.IsUserHasAnyNotes(userId))
            {
                AnsiConsole.Markup("[red1]\n⛔ You don't have any notes, press anything to continue...[/]");
                Console.ReadKey(true);
                return null;
            }

            Console.WriteLine("");

                var colWidth = 30;
                var table = new Table()
                    .Centered()
                    .BorderColor(Color.White)
                    .AddColumn(new TableColumn("[darkorange]🎓 Studies ▶ {S}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]💼 Work ▶ {W}[/]").Centered().Width(colWidth))
                    .AddColumn(new TableColumn("[darkorange]🏠 Home ▶ {H}[/]").Centered().Width(colWidth))
                    .Expand();

                table.AddRow("[darkorange]🎨 Hobby ▶ {B}[/]", "[darkorange]🌐 Other ▶ {O}[/]", "[darkorange]✨ Custom ▶ {C}[/]");

                AnsiConsole.Write(table);


                ConsoleKeyInfo pressedKey;
                string choice = "";
                var choices = new List<string> { "S", "W", "H", "B", "O", "C" };
                while (!choices.Contains(choice))
                {
                    pressedKey = Console.ReadKey(true);
                    choice = pressedKey.Key.ToString();
                }
                            string category = "";
                switch (choice)
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
                    case "B":
                        category = "Hobby";
                        break;
                    case "O":
                        category = "Other";
                        break;
                    case "C":
                        category = AnsiConsole.Prompt(
                        new TextPrompt<string>("[gold1]\nEnter custom[/] [darkorange]category:[/]"));
                        return category;
                    default:
                        category = "";
                        break;
                }

            return category;
        }

        public int ShowNotesByCategory(int userId, string categoryFilter, bool fromBack)
        {
            if (fromBack)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                new FigletText(Program.font, "CATEGORIES")
                .Centered()
                .Color(Color.Gold1));
            }
            else
                Console.WriteLine();
            int categoryFiltredId = _categoryController.GetOrCreateCategoryIdAsync(categoryFilter).Result;
            var notes = _noteController.GetNotesByUserIdAndCategory(userId, categoryFiltredId);
            if (notes.Count == 0)
            {
                AnsiConsole.Markup($"[red1]⛔ You don't have notes from [/][darkorange]'{categoryFilter}'[/] [red1]category, press anything to continue...[/]");
                Console.ReadKey(true);
                return -1;
            }
            else
            {
                var rule = new Rule($"[gold1]Notes from category '[/][darkorange]{categoryFilter}[/][gold1]':[/]");
                rule.Style = new Style(Color.Gold1);
                rule.LeftJustified();
                AnsiConsole.Write(rule);

                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.CategoryId})[/] - {n.ModifiedDate}").ToList();
                list.Insert(0, "Back");

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

        public DateTime ChooseDate(int userId)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font, "Date")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Enter a searching date:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            if (!_noteController.IsUserHasAnyNotes(userId))
            {
                AnsiConsole.Markup("[red1]\n⛔ You don't have any notes, press anything to continue...[/]");
                Console.ReadKey(true);
                return DateTime.MinValue;
            }

            Console.WriteLine();

            DateTime chosenDate;
            var inputDate = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter[/] [darkorange]date[/] [gold1]in format ([/][darkorange]dd.MM.yyyy[/][gold1]):[/]"));

            if(!(DateTime.TryParseExact(inputDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out chosenDate)))
            {
                AnsiConsole.MarkupLine($"[red1]\n⛔ You used wrong date format, press anything to continue...[/]");
                Console.ReadKey(true);
                return DateTime.MinValue;
            }

            return chosenDate;
        }

        public int ExploreNotesByDate(int userId, DateTime date)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText(Program.font, "Date")
            .Centered()
                .Color(Color.Gold1));

            var rule = new Rule($"[gold1]Notes from[/] [darkorange]{date.ToString("dd.MM.yyyy")}[/][gold1]:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var notes = _noteController.GetNotesByUserIdAndDay(userId, date);

            if (notes.Count == 0)
            {
                AnsiConsole.Markup($"[red1]\n⛔ You don't have any notes from [/][darkorange]{date.Day}.{date.Month}.{date.Year}[/][red1], press anything to continue...[/]");
                Console.ReadKey(true);
                return -1;
            }
            else
            {
                var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.CategoryId})[/] - {n.ModifiedDate}").ToList();
                list.Insert(0, "Back");

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

            var rule = new Rule("[gold1]Select month:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);


            var allNotes = _noteController.GetNotesByUserId(userId);
            if(allNotes.Count() == 0)
            {
                AnsiConsole.Markup("[red1]\n⛔ You don't have any notes, press anything to continue...[/]");
                Console.ReadKey(true);
                return DateTime.MinValue;
            }
            var yearAndMonth = new HashSet<(int, int)>();
            foreach (var note in allNotes)
            {
                yearAndMonth.Add((note.ModifiedDate.Year, note.ModifiedDate.Month));
            }
            var list = yearAndMonth.OrderByDescending(date => date).Select(date => new DateTime(date.Item1, date.Item2, 1).ToString("MM.yyyy")).ToList();
            list.Insert(0, "Back");

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("")
            .HighlightStyle(Color.DarkOrange)
            .PageSize(10)
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
            AnsiConsole.MarkupLine($"\n[gold1]Chosen month: [/][darkorange]{choice}[/]");
            return date;
        }

        public int ShowNotesByMonth(int userId, DateTime date, bool fromBack)
        {
            if (fromBack)
            {
                AnsiConsole.Clear();

                AnsiConsole.Write(
                new FigletText(Program.font, "MONTHS")
                .Centered()
                .Color(Color.Gold1));
            }
            else
                Console.WriteLine();

            string month = date.Month < 10 ? "0" + date.Month : date.Month.ToString();

            var rule = new Rule($"[gold1]Notes from [/][darkorange]XX.{month}.{date.Year}[/][gold1]:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var notes = _noteController.GetNotesByUserIdAndMonth(userId, date);
            var list = notes.Select(n => $"{n.Id}: [darkorange]{n.Title}[/] [gold1]({n.CategoryId})[/] - {n.ModifiedDate}").ToList();
            list.Insert(0, "Back");

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
                    .AddColumn(new TableColumn($"[darkorange]{note.Title}[/] [gold1]({note.CategoryId})[/]")
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
