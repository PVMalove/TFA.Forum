namespace TFA.Forum.Domain.Exceptions;

public class ForumNotFoundException(Guid forumId) : Exception($"Forum with id {forumId} not found.");