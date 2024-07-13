using RandomNumbers.Host.Auth;

namespace RandomNumbers.Host.MediatR.Handlers;

public class PlayMatchHandler : IRequestHandler<PlayMatchRequest, int>
{
    private readonly Data.RandomNumbersContext dbContext;
    private readonly ITokenInfo tokenInfo;

    public PlayMatchHandler(Data.RandomNumbersContext dbContext, ITokenInfo tokenInfo)
    {
        this.dbContext = dbContext;
        this.tokenInfo = tokenInfo;
    }

    public async Task<int> Handle(PlayMatchRequest request, CancellationToken cancellationToken)
    {
        User user = tokenInfo.User;
        await ValidateMatch(request.MatchId, cancellationToken);

        await ValidateUserParticipation(request.MatchId, user.Id, cancellationToken);

        int generatedNumber = new Random().Next(1, 101);

        dbContext.UserMatches.Add(new UserMatch { Score = generatedNumber, MatchId = request.MatchId, UserId = user.Id });

        await dbContext.SaveChangesAsync(cancellationToken);

        return generatedNumber;
    }

    private async Task ValidateMatch(int matchId, CancellationToken cancellationToken)
    {
        var match = await dbContext.Matches.FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken);

        if (match == null || match.IsDone)
        {
            throw new CustomException { ErrorMessage = ErrorMessages.ERR111001, ErrorCode = "ERR111001" };
            throw new InvalidOperationException("Match does not exist or has already expired.");
        }
    }

    private async Task ValidateUserParticipation(int matchId, int userId, CancellationToken cancellationToken)
    {
        var hasPlayed = await dbContext.UserMatches.AsNoTracking()
            .AnyAsync(userMatch => userMatch.MatchId == matchId && userMatch.UserId == userId, cancellationToken);

        if (hasPlayed)
        {
            throw new CustomException { ErrorMessage = ErrorMessages.ERR111000, ErrorCode = "ERR111000" };
        }
    }
}





