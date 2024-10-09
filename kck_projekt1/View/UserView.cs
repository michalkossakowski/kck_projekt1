using kck_api.Controller;
using Spectre.Console;

namespace kck_projekt1.View
{
    public class UserView : View
    {
        public UserModel AddNewUser()
        {
            AnsiConsole.Markup("Nick:");
            var nick = Console.ReadLine();

            AnsiConsole.Markup("Passowrd:");
            var password = Console.ReadLine();

            return new UserModel(nick, password);
        }

        public UserModel LoginUser()
        {
            var rule = new Rule("[red]Logowanie:[/]");
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Podaj Nick:"));

            var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Podaj Hasło:").Secret());

            return new UserModel(name, password);
        }
    }
}
