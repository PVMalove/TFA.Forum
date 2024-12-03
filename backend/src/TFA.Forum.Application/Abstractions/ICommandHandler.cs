using CSharpFunctionalExtensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Execute(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Execute(TCommand command, CancellationToken cancellationToken = default);
}