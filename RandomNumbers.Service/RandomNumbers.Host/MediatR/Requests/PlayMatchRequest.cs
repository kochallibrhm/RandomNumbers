namespace RandomNumbers.Host.MediatR.Requests;

public class PlayMatchRequest : IRequest<int>
{
    public int MatchId { get; set; }
}

public class PlayMatchRequestValidator : AbstractValidator<PlayMatchRequest>
{
    public PlayMatchRequestValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0).WithMessage("Invalid match ID.");
    }
}


