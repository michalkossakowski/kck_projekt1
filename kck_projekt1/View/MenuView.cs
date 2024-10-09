using Spectre.Console;

namespace kck_projekt1.View
{
    public class MenuView : View
    {

        public string ShowStartMenu()
        {
            AnsiConsole.Write(
            new FigletText("Notes")
                .LeftJustified()
                .Color(Color.Aqua));

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Witaj w aplikacji Notes and Reminders: ")
            .PageSize(3)
            .AddChoices(new[] {
                "Zaloguj się",
                "Zarejestruj się",
                "Wyjdź"
             }));

            return choice;
        }

        public string ShowActionMenu()
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Wybierz akcję: ")
            .PageSize(5)
            .AddChoices(new[] {
                "Notatki",
                "Wydarzenia", 
                "Kalendarz",
                "Wyloguj",
                "Wyjdź"
             }));
            return choice;
        }
    }
}
