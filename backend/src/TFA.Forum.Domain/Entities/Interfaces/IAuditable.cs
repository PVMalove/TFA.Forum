namespace TFA.Forum.Domain.Entities.Interfaces;

public interface IAuditable
{
    DateTime CreateAt { get; set; }
    DateTime? UpdateAt { get; set; }
}