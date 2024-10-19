namespace TFA.Forum.Domain.Exceptions;

public class ForumNotFoundException(Guid forumId) : DomainException(ErrorCode.Gone, $"Forum with id {forumId} not found.");