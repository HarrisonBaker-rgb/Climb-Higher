using SQLite;
namespace climb_higher;
[Table("userData")]
public class userData
{

    [Unique]
    public string userName { get; set; }
    public string password { get; set; }

    public userData(string userName, string password)
    {
        this.userName = userName;
        this.password = password;
    }

    public override string ToString()
    {
        return userName;
    }
}
