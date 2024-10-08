using Spectre.Console;

public abstract class View
{

}

public class BookView : View
{
    public void Add()
    {
        AnsiConsole.Markup("[underline red]Title:[/]");
        var title = Console.ReadLine();

        Console.WriteLine("Author:");
        var author = Console.ReadLine();

        Console.WriteLine("Publish date:");
        var publishDate = Convert.ToInt32(Console.ReadLine());

        var book = new BookModel(title, author, publishDate);
    }

}
