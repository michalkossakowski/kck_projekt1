using kck_api.Controller;
using Microsoft.VisualBasic.ApplicationServices;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
             }));

            return new NoteModel(user.Id, note, category);
        }

        public void ShowNotes(int userId)
        {
            AnsiConsole.Write(
            new FigletText("My Notes")
                .LeftJustified()
                .Color(Color.Aqua));

            var rule = new Rule();
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var noteController = NoteController.GetInstance();
            var notes = noteController.GetNotesByUserId(userId);

            foreach (var note in notes) 
            {
                var noteContent = new Panel(new Markup($"{note.ModifiedDate}\n{note.Content}"))
                .Header($"[yellow]{note.Category} Id:{note.Id}[/]")
                .Border(BoxBorder.Rounded)
                .Padding(2, 2);
                AnsiConsole.Write(noteContent);
            }

            AnsiConsole.Markup("[green]Here are your notes, press anything to continue[/]");


            Console.ReadKey();
        }
    }
}
