namespace RandomNumbers.Host.Filter;

public class AsyncValidationFilter : IAsyncActionFilter
{
    private readonly IValidatorFactory _validatorFactory;

    public AsyncValidationFilter(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

        if (descriptor != null)
        {
            var parameters = descriptor.MethodInfo.GetParameters();

            foreach (var parameter in parameters)
            {
                var validator = _validatorFactory.GetValidator(parameter.ParameterType);

                if (validator != null)
                {
                    var value = context.ActionArguments[parameter.Name];

                    var validationContext = new ValidationContext<object>(value);

                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(new CustomException()
                        {
                            ErrorCode = validationResult.Errors.First().ErrorCode,
                            ErrorMessage = validationResult.Errors.First().ErrorMessage
                        });
                        return;
                    }
                }
            }
        }

        await next();
    }
}