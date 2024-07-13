namespace RandomNumbers.Host.MediatR.Handlers;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserRequest, string>
{
    private readonly Data.RandomNumbersContext dbContext;
    private readonly ApplicationSettings applicationSettings;
    private readonly IHashService hashService;

    public AuthenticateUserHandler(Data.RandomNumbersContext dbContext, ApplicationSettings applicationSettings, IHashService hashService)
    {
        this.dbContext = dbContext;
        this.applicationSettings = applicationSettings;
        this.hashService = hashService;
    }

    public async Task<string> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var hashedPassword = await hashService.HashText(request.Password);
        User? user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName != null && x.UserName.Equals(request.UserName) && x.Password == hashedPassword, cancellationToken);


        if (user is null)
            throw new CustomException { ErrorCode = "ERR101004", ErrorMessage = ErrorMessages.ERR101004 };

        string token = GenerateJwtToken(user);
        return token;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(applicationSettings.Jwt.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(CustomClaimTypes.User,JsonSerializer.Serialize(user))
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
