using kck_api.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class NoteModel
{
    [Key]
	public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Content {get; set; }
    public int CategoryId { get; set; }
    public DateTime ModifiedDate { get; set; }
    [ForeignKey("CategoryId")]
    public CategoryModel Category { get; set; }
    public NoteModel() { }

    public NoteModel(int authorId, string title, string content, int category)
    {
        this.AuthorId = authorId;
        this.Title = title;
        this.Content = content;
        this.CategoryId = category;
        this.ModifiedDate = DateTime.Now;
    }
}
