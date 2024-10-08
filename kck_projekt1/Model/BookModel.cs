using System.ComponentModel.DataAnnotations;

public class BookModel
{
    [Key]
	public int Id {get; set; }
    public string Title {get; set; }
    public string Author {get; set; }
    public int PublishDate {get; set; }

    public BookModel() { }

    public BookModel(string title, string author, int publishDate)
    {
        this.Title = title;
        this.Author = author;
        this.PublishDate = publishDate;
    }
}
