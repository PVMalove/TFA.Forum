using AutoMapper;
using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.Application.Mapping;

public class ForumMapping : Profile
{
    public ForumMapping()
    {
        CreateMap<Domain.Entities.Forum, ForumCreateDto>()
            .ForMember(dest => dest.Title, opt => opt.Ignore())
            .ConstructUsing(src => new ForumCreateDto(src.Title.Value, src.CreatedAt));
    }
}