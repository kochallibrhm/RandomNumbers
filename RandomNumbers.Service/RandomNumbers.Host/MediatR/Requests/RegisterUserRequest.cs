namespace RandomNumbers.Host.MediatR.Requests;

public class RegisterUserRequest : IRequest<bool>
{
    /// <summary>
    /// Kullanıcı Adı
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Kullanıcı Şifresi
    /// </summary>
    public string Password { get; set; }

}

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage(ErrorMessages.ERR101000).WithErrorCode("ERR101000");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(ErrorMessages.ERR101001).WithErrorCode("ERR101001");
    }
}
