using Spectre.Console;

namespace kck_projekt1.View
{
    public class MenuView : View
    {

        public string ShowStartMenu(UserModel user)
        {
            AnsiConsole.Write(
            new FigletText("Notes")
                .LeftJustified()
                .Color(Color.Aqua));

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Welcome in Notes and Reminders App "+)
            .PageSize(4)
            .AddChoices(new[] {
                "Log in",
                "Register",
                "Graphic Mode",
                "Exit"
             }));

            return choice;
        }

        public string ShowActionMenu()
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose Action: ")
            .PageSize(5)
            .AddChoices(new[] {
                "Notes",
                "Events", 
                "Calendar",
                "Log out",
                "Exit"
             }));
            return choice;
        }
    }
}
