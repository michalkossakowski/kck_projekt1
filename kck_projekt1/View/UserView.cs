using Spectre.Console;

namespace kck_projekt1.View
{
    public class UserView
    {
        public UserModel RegisterUser()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font,"Register")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Add new user:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine("");

            var nick = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter your[/] [darkorange]nick:[/]"));

            var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter your[/] [darkorange]password:[/]").Secret());

            var confirm = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Confirm your[/] [darkorange]password:[/]").Secret());

            if(confirm != password)
                return null;
            return new UserModel(nick, password);
        }

        public UserModel LoginUser()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
            new FigletText(Program.font,"Login")
            .Centered()
            .Color(Color.Gold1));

            var rule = new Rule("[gold1]Enter your login information:[/]");
            rule.Style = new Style(Color.Gold1);
            rule.LeftJustified();
            AnsiConsole.Write(rule);

            Console.WriteLine("");

            var nick = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter your[/] [darkorange]nick:[/]"));

            var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[gold1]Enter your[/] [darkorange]password:[/]").Secret());

            return new UserModel(nick, password);
        }
    }
}
