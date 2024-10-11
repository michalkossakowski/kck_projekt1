using System.ComponentModel.DataAnnotations;

public class NoteModel
{
    [Key]
	public int Id {get; set; }
    public int AuthorId {get; set; }
    public string Title { get; set; }
    public string Content {get; set; }
    public string Category { get; set; }
    public DateTime ModifiedDate { get; set; }

    public NoteModel() { }

    public NoteModel(int authorId, string title, string content, string category)
    {
        this.AuthorId = authorId;
        this.Title = title;
        this.Content = content;
        this.Category = category;
        this.ModifiedDate = DateTime.Now;
    }
}
