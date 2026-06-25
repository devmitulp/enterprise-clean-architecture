using Shared.Models;
using Application.Features.JobTitles.DTOs;

namespace Application.Features.JobTitles
{
    public interface IJobTitleAppService
    {
        Task<PagedResultDto<JobTitleDto>> GetAllAsync(GetAllJobTitlesInput input, CancellationToken ct = default);

        Task<JobTitleDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<JobTitleDto> CreateAsync(JobTitleInputDto input, CancellationToken ct = default);

        Task<JobTitleDto> UpdateAsync(JobTitleInputDto input, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
