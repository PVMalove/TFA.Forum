﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.API.Middlewares;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        IntentionManagerException intentionManagerException) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status403Forbidden,
            "Authorization failed",
            detail: intentionManagerException.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        DomainException domainException) =>
        factory.CreateProblemDetails(httpContext,
            domainException.ErrorCode switch
            {
                ErrorCode.Gone => StatusCodes.Status410Gone,
                _ => StatusCodes.Status500InternalServerError
            },
            domainException.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }

        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Validation failed");
    }
}