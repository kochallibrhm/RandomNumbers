using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace RandomNumbers.Host.Filter;

public class CustomExceptionFilter : IExceptionFilter, IFilterMetadata
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CustomException customException)
        {
            context.Result = new JsonResult(new
            {
                ErrorCode = customException.ErrorCode,
                ErrorMessage = customException.ErrorMessage
            });

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.ExceptionHandled = true;
        }
    }

}
