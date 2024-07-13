namespace RandomNumbers.Host.MediatR.Responses;

public class MatchResponse
{
    public int Id { get; set; }
    public string WinnerName { get; set; }
    public TimeSpan ExpiryTimestamp { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDone { get; set; }
    public int CurrentUserScore { get; set; }
}
