using RandomNumbers.Host.Auth;

namespace RandomNumbers.Host.MediatR.Handlers;

public class GetMathcesHandler : IRequestHandler<GetMatchesRequest, List<MatchResponse>>
{
    private readonly Data.RandomNumbersContext dbContext;
    private readonly ITokenInfo tokenInfo;

    public GetMathcesHandler(Data.RandomNumbersContext dbContext, ITokenInfo tokenInfo)
    {
        this.dbContext = dbContext;
        this.tokenInfo = tokenInfo;
    }

    public async Task<List<MatchResponse>> Handle(GetMatchesRequest request, CancellationToken cancellationToken)
    {
        List<Match> matches = await dbContext.Matches.AsNoTracking()
            .Include(m => m.WinnerUser)
            .OrderByDescending(m => m.StartDate)
            .Take(10)
            .ToListAsync(cancellationToken);

        List<MatchResponse> matchResponseList = matches.Select(match => new MatchResponse
        {
            Id = match.Id,
            WinnerName = match.WinnerUser?.UserName,
            ExpiryTimestamp =  match.EndDate - DateTime.UtcNow,
            StartDate = match.StartDate,
            EndDate = match.EndDate,
            IsDone = match.IsDone,
            CurrentUserScore = match.IsDone ? 0 : GetUserScore(match.Id)
        }).ToList();
        return matchResponseList;
    }

    private int GetUserScore(int matchId)
    {
        var user = tokenInfo.User;
        if (user != null && user.Id > 0)
        {
            UserMatch match = dbContext.UserMatches.AsNoTracking().FirstOrDefault(userMatch => userMatch.MatchId == matchId && userMatch.UserId == user.Id);
            return match is null ? 0 : match.Score;
        }
        return 0;
    }
}