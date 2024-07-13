namespace RandomNumbers.Data.Models;

public class UserMatch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MatchId { get; set; }
    public int Score { get; set; }

    public virtual User User { get; set; }
    public virtual Match Match { get; set; }
}
