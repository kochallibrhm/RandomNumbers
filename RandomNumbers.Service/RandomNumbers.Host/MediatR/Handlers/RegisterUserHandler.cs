namespace RandomNumbers.Host.MediatR.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, bool>
{
    private readonly Data.RandomNumbersContext dbContext;
    private readonly ApplicationSettings applicationSettings;
    private readonly IHashService hashService;

    public RegisterUserHandler(Data.RandomNumbersContext dbContext, ApplicationSettings applicationSettings, IHashService hashService)
    {
        this.dbContext = dbContext;
        this.applicationSettings = applicationSettings;
        this.hashService = hashService;
    }
    public async Task<bool> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await CheckUserNameAvailable(request.UserName);

        try
        {
            var hashedPassword = await hashService.HashText(request.Password);
            User user = new() { UserName = request.UserName, Password = hashedPassword };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new CustomException() { ErrorMessage = ex.Message, ErrorCode = ErrorMessages.ERR101104 };
        }
    }

    private async Task<bool> CheckUserNameAvailable(string userName)
    {
        User user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserName.Equals(userName));
        if (user is not null)
        {
            throw new CustomException() { ErrorMessage = string.Format(ErrorMessages.ERR101105, userName), ErrorCode = "ERR101105" };
        }
        return true;
    }
}
