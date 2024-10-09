using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Key]
	public int Id {get; set; }
    public string Nick {get; set; }
    public string Password {get; set; }
    public string Type { get; set; }
    public UserModel() { }

    public UserModel(string nick, string password)
    {
        this.Nick = nick;
        this.Password = password;
        this.Type = "client";
    }
}