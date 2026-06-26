using System.Linq.Dynamic.Core;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.Persistence;
using Application.Features.JobTitles;
using Application.Features.JobTitles.DTOs;
using Domain.Entities.JobTitles;
using Infrastructure.Services.Common.Base;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Models;

namespace Infrastructure.Services.JobTitles
{
    public class JobTitleAppService : ApplicationBaseService, IJobTitleAppService
    {
        private readonly IRepository<JobTitle> _jobTitleRepository;

        public JobTitleAppService(IServiceContext context, IRepository<JobTitle> jobTitleRepository) : base(context)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        #region Public Methods

        public async Task<PagedResultDto<JobTitleDto>> GetAllAsync(GetAllJobTitlesInput input, CancellationToken ct = default)
        {
            IQueryable<JobTitle> query = _jobTitleRepository
                .AsQueryable()
                .Where(x => string.IsNullOrWhiteSpace(input.Filter) ||
                            x.Name.Contains(input.Filter) ||
                            (x.Description != null && x.Description.Contains(input.Filter)))
                .OrderBy(string.IsNullOrWhiteSpace(input.Sorting) ? "Name ASC" : input.Sorting);

            var totalCount = await query.CountAsync(ct);

            if (input.ApplyPagination == true)
            {
                int page = Math.Max(input.CurrentPage ?? 1, 1);
                int size = input.PageSize is > 0 ? input.PageSize.Value : 10;
                query = query.Skip((page - 1) * size).Take(size);
            }

            return new PagedResultDto<JobTitleDto>(
                totalCount,
                ObjectMapper.Map<IReadOnlyList<JobTitleDto>>(await query.ToListAsync(ct)));
        }

        public async Task<JobTitleDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(Localization.L("EntityNotFound", "JobTitle", id));
            return ObjectMapper.Map<JobTitleDto>(entity);
        }

        public async Task<JobTitleDto> CreateOrUpdateJobTitleAsync(JobTitleInputDto input, CancellationToken ct = default)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                return await Update(input, ct);
            }
            else
            {
                return await Create(input, ct);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(Localization.L("EntityNotFound", "JobTitle", id));
            _jobTitleRepository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(ct);
        }

        #endregion

        #region Private Methods

        private async Task<JobTitleDto> Create(JobTitleInputDto input, CancellationToken ct = default)
        {
            var nameExists = await _jobTitleRepository.ExistsAsync(x => x.Name.ToLower() == input.Name.ToLower(), ct);
            if (nameExists)
                throw new AppException(Localization.L("JobTitleAlreadyExists", input.Name));

            var entity = ObjectMapper.Map<JobTitle>(input);
            await _jobTitleRepository.AddAsync(entity, ct);
            await UnitOfWork.SaveChangesAsync(ct);

            return ObjectMapper.Map<JobTitleDto>(entity);
        }

        private async Task<JobTitleDto> Update(JobTitleInputDto input, CancellationToken ct = default)
        {
            var id = input.Id ?? throw new AppException(Localization.L("JobTitleIdRequiredForUpdate"));
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException(Localization.L("EntityNotFound", "JobTitle", id));

            var nameExists = await _jobTitleRepository.ExistsAsync(
                x => x.Name.ToLower() == input.Name.ToLower() && x.Id != id, ct);
            if (nameExists)
                throw new AppException(Localization.L("JobTitleAlreadyExists", input.Name));

            ObjectMapper.Map(input, entity);
            _jobTitleRepository.Update(entity);
            await UnitOfWork.SaveChangesAsync(ct);

            return ObjectMapper.Map<JobTitleDto>(entity);
        }

        #endregion
    }
}
