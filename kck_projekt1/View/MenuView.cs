﻿using kck_api.Controller;
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
                Console.Clear();
                var choice = ShowLoginMenu();
                switch (choice)
                {
                    case "Log in":
                        var user = userView.LoginUser();
                        AnsiConsole.Status()
                        .Spinner(Spinner.Known.BouncingBar)
                        .SpinnerStyle(Style.Parse("blue"))
                        .Start("[aqua]Loading[/]", ctx =>
                        {
                            user = userController.GetUser(user);
                        });
                        if (user == null)
                        {
                            AnsiConsole.Markup("[red]Wrong nick or password, press anything to continue[/]");
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
                        if (userController.AddUser(userView.RegisterUser()))
                            AnsiConsole.Markup("[green]New user added, press anything to continue[/]");
                        else
                            AnsiConsole.Markup("[yellow]This nick is occupied, press anything to continue[/]");
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
                .Color(Color.Aqua));

            var rule = new Rule();
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.Aqua)
            .Title("[blue]Welcome in Notes App choose action:[/]")
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
            Console.Clear();
            var noteView = new NoteView();
            var noteController = NoteController.GetInstance();
            while (true)
            {
                Console.Clear();
                AnsiConsole.Write(
                new FigletText("Menu")
                    .LeftJustified()
                    .Color(Color.Aqua));

                var rule = new Rule();
                rule.Style = new Style(Color.Blue);
                rule.LeftJustified();
                AnsiConsole.Write(rule);

                var choice = ActionSelect();
                switch (choice)
                {
                    case "Latest notes":
                        noteView.ShowLatestNotes(_loggedUser.Id);
                        break;

                    case "Explore notes":
                        noteView.ShowNote(noteView.ExploreNotes(_loggedUser.Id));
                        break;

                    case "Add note":
                        noteController.AddNote(noteView.WriteNote(_loggedUser));
                        AnsiConsole.Markup("[green]New note added, press anything to continue[/]");
                        Console.ReadKey();
                        break;

                    case "Calendar":
                        noteView.ShowCalendar();
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
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[blue]Hello {_loggedUser.Nick} what do you want to do?/-[/]")
            .HighlightStyle(Color.Aqua)
            .AddChoices(new[] {
                            "Latest notes",
                            "Explore notes",
                            "Add note",
                            "Calendar",
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
