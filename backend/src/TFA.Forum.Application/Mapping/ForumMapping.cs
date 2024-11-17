using AutoMapper;
using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.Application.Mapping;

public class ForumMapping : Profile
{
    public ForumMapping()
    { 
        CreateMap<Domain.Entities.Forum, ForumCreateDto>();
    }
}