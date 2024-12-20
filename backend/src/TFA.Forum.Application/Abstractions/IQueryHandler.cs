﻿using CSharpFunctionalExtensions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Execute(TQuery query, CancellationToken cancellationToken = default);
}