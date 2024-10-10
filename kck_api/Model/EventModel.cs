using System.ComponentModel.DataAnnotations;

public class EventModel
{
    [Key]
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }

    public EventModel() { }

    public EventModel(int authorId, string content, string category)
    {
        this.AuthorId = authorId;
        this.Description = content;
        this.EventDate = DateTime.Now;
    }
}
