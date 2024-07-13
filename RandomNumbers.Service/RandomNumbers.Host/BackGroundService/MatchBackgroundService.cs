using Microsoft.Extensions.Hosting;

namespace RandomNumbers.Host.BackGroundService;

public class MatchBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private Timer timer;
    private const int GameTimeMinutes = 3;

    public MatchBackgroundService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(stoppingToken);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<RandomNumbersContext>();

            Match activeMatch = await dbContext.Matches.AsNoTracking().Where(m => !m.IsDone).FirstOrDefaultAsync(stoppingToken);

            if (activeMatch == null)
            {
                var newMatch = new Match
                {
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(GameTimeMinutes),
                    ExpiryTimestamp = TimeSpan.FromMinutes(GameTimeMinutes)
                };

                dbContext.Matches.Add(newMatch);
                await dbContext.SaveChangesAsync(stoppingToken);
                var waitTime = newMatch.EndDate - DateTime.UtcNow;
                await Task.Delay(waitTime, stoppingToken);
            }
            else
            {
                var waitTime = activeMatch.EndDate - DateTime.UtcNow;
                if (waitTime.TotalMilliseconds > 0)
                {
                    await Task.Delay(waitTime, stoppingToken);
                }

                var winner = await dbContext.UserMatches
                    .Where(um => um.MatchId == activeMatch.Id)
                    .Include(x => x.User)
                    .OrderByDescending(p => p.Score)
                    .FirstOrDefaultAsync(stoppingToken);

                if (winner != null)
                {
                    activeMatch.WinnerUserId = winner.User.Id;
                }

                activeMatch.IsDone = true;
                dbContext.Matches.Update(activeMatch);
                await dbContext.SaveChangesAsync(stoppingToken);

                var newMatch = new Match
                {
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(GameTimeMinutes),
                    ExpiryTimestamp = TimeSpan.FromMinutes(GameTimeMinutes)
                };

                dbContext.Matches.Add(newMatch);
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }

    public override void Dispose()
    {
        timer?.Dispose();
        base.Dispose();
    }
}
