using AutoMapper;
using Domain.Entities.JobTitles;
using Application.Features.JobTitles.DTOs;

namespace Application.Features.JobTitles.Mappings
{
    public class JobTitleMappingProfile : Profile
    {
        public JobTitleMappingProfile()
        {
            CreateMap<JobTitle, JobTitleDto>();

            CreateMap<JobTitleInputDto, JobTitle>()
                .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue));
        }
    }
}
