using kck_api.Controller;
using Spectre.Console;

namespace kck_projekt1.View
{
    public class UserView : View
    {
        public UserModel AddNewUser()
        {

            var rule = new Rule("[aqua]Add new user:[/]");
            rule.Style = new Style(Color.Blue);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var nick = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Enter your nick:[/]"));

            var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Enter your password:[/]"));

            return new UserModel(nick, password);
        }

        public UserModel LoginUser()
        {
            var rule = new Rule("[aqua]Log in:[/]");
            rule.Style = new Style(Color.Blue); 
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            var nick = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Enter your nick:[/]"));

            var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[aqua]Enter your password:[/]").Secret());

            return new UserModel(nick, password);
        }
    }
}
