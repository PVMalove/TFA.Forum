﻿using TFA.Forum.Domain.DTO.User;

namespace TFA.Forum.Persistence.Storage.User;

public interface ISignInStorage
{
    Task<ExistsUserDto?> FindUserByLogin(string login, CancellationToken cancellationToken);
    Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
}