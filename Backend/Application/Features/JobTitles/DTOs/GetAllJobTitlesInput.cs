using Shared.Models;

namespace Application.Features.JobTitles.DTOs
{
    public class GetAllJobTitlesInput : FilterRequestDto
    {
        public string? Filter { get; set; }
    }
}
