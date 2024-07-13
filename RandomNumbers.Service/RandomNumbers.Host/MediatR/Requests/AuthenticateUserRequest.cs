using FluentValidation;

namespace RandomNumbers.Host.MediatR.Requests;

public class AuthenticateUserRequest : IRequest<string>
{
    /// <summary>
    /// Kullanıcı Adı
    /// </summary>
    public string UserName { get; set;}

    /// <summary>
    /// Kullanıcı Şifresi
    /// </summary>
    public string Password { get; set;}

}

public class AuthenticateUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public AuthenticateUserRequestValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage(ErrorMessages.ERR101000).WithErrorCode("ERR101000");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(ErrorMessages.ERR101001).WithErrorCode("ERR101001");
    }
}
