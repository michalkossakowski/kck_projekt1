using kck_api.Controller;
using Spectre.Console;

namespace kck_projekt1.View
{
    public class NoteView
    {
        public NoteModel WriteNote(UserModel user)
        {

            var rule = new Rule("[aqua]Write a new note:[/]");
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var title = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Title:[/]"));

            var note = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Note:[/]"));

            var category = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.Aqua)
            .Title("[blue]Select Category:[/]")
            .AddChoices(new[] {
                            "Studies",
                            "Work",
                            "Home",
                            "Hobby",
                            "Other",
                            "Custom"
             }));

            if(category == "Custom")
            {
                category = AnsiConsole.Prompt(
                new TextPrompt<string>("[aqua]Custom category:[/]"));
            }

            return new NoteModel(user.Id, title, note, category);
        }

        public void ShowLatestNotes(int userId)
        {
            Console.Clear();
            AnsiConsole.Write(
            new FigletText("Latest:")
                .LeftJustified()
                .Color(Color.Aqua));

            var rule = new Rule();
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetLatestNotesByUserId(userId,3);

            const int maxWidth = 30;

            foreach (var note in notes)
            {
                var content = note.Content.Length > maxWidth ? note.Content.Substring(0, maxWidth - 3) + "..." : note.Content;
                content = content.PadLeft((maxWidth + content.Length)/2).PadRight(maxWidth);

                var noteContent = new Panel(new Markup(content))
                    .Header($"[aqua]{note.Title}[/]")
                    .Border(BoxBorder.Rounded);

                AnsiConsole.Write(noteContent);

            }

            AnsiConsole.Markup("[green]Here are your notes, press anything to continue[/]");

            Console.ReadKey();
        }

        public int ExploreNotes(int userId)
        {
            Console.Clear();
            AnsiConsole.Write(
            new FigletText("Select:")
                .LeftJustified()
                .Color(Color.Aqua));

            var rule = new Rule();
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserId(userId);
            notes.Reverse();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose note to show:")
                .PageSize(10)
                .AddChoices(notes.Select(n => $"{n.Id}: {n.Title} - {n.ModifiedDate}").ToList()));

            var id = int.Parse(choice.Split(':')[0]);

            return id;
        }

        public void ShowNote(int noteId)
        {
            Console.Clear();
            AnsiConsole.Write(
            new FigletText("Note:")
                .LeftJustified()
                .Color(Color.Aqua));

            var rule = new Rule();
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();
            var note = noteController.GetNoteById(noteId);

            var table = new Table();

            table.AddColumn($"[aqua]{note.Title}[/]");

            table.AddRow($"[yellow]{note.ModifiedDate.ToString("g")}[/]");
            table.AddRow($"[blue]Category: {note.Category}[/]");
            table.AddRow(new Panel(note.Content));

            AnsiConsole.Write(table);

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.Aqua)
            .Title("[blue]Select action:[/]")
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

        public void ShowCalendar()
        {
            var calendar = new Calendar(2020, 10);
            calendar.AddCalendarEvent(2020, 10, 11);
            calendar.HighlightStyle(Style.Parse("yellow bold"));
            AnsiConsole.Write(calendar);

            AnsiConsole.Markup("[green]Here are the calendar, press anything to continue[/]");
            Console.ReadKey();
        }


    }
}
