using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Forum.API.Response;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, ProblemDetailsFactory problemDetailsFactory,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error has happened with {RequestPath}, the message is {ErrorMessage}",
                context.Request.Path.Value, exception.Message);

            ProblemDetails problemDetails;
            switch (exception)
            {
                case IntentionManagerException intentionManagerException:
                    problemDetails = problemDetailsFactory.CreateFrom(context, intentionManagerException);
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(context, validationException);
                    logger.LogInformation(validationException, "Somebody sent invalid request, oops");
                    break;
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(context, domainException);
                    logger.LogError(domainException, "Domain exception occured");
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(context,
                        StatusCodes.Status500InternalServerError, "Unhandled error! Please contact us.");
                    logger.LogError(exception, "Unhandled exception occured");
                    break;
            }

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            
            var error = Error.Failure(problemDetails.Status.ToString()!, problemDetails.Title!, problemDetails.GetType().ToString() );
            var envelope = Envelope.Error(error);
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}