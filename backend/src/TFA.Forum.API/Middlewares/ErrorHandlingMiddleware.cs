using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context, ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            var problemDetails = exception switch
            {
                IntentionManagerException intentionManagerException =>
                    problemDetailsFactory.CreateFrom(context, intentionManagerException),
                ValidationException validationException =>
                    problemDetailsFactory.CreateFrom(context, validationException),
                DomainException domainException =>
                    problemDetailsFactory.CreateFrom(context, domainException),
                _ => problemDetailsFactory.CreateProblemDetails(context, StatusCodes.Status500InternalServerError,
                    "Unhandled error! Please contact us.", detail: exception.Message),
            };

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                await context.Response.WriteAsJsonAsync(validationProblemDetails);
            }
            else
            {
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}