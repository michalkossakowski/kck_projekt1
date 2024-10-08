using System;

import BookModel
public class Views
{
	public BookAddView()
	{
        Console.WriteLine("Hello, World!");
        AnsiConsole.Markup("[underline red]Hello[/] World!");

        Console.WriteLine("Title:");
        var title = Console.ReadLine();

        Console.WriteLine("Author:");
        var author = Console.ReadLine();

        Console.WriteLine("Publish date:");
        var publishDate = Convert.ToInt32(Console.ReadLine());

        var book = new BookModel(title, author, publishDate);

        book.Save();

    }
}
