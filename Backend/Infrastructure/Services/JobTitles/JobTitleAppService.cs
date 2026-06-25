using Application.Common.Interfaces.Persistence;
using Application.Features.JobTitles;
using Application.Features.JobTitles.DTOs;
using AutoMapper;
using Domain.Entities.JobTitles;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Models;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Services.JobTitles
{
    public class JobTitleAppService(
        IRepository<JobTitle> jobTitleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IJobTitleAppService
    {
        private readonly IRepository<JobTitle> _jobTitleRepository = jobTitleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

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
                _mapper.Map<IReadOnlyList<JobTitleDto>>(await query.ToListAsync(ct)));
        }

        public async Task<JobTitleDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException("JobTitle", id);
            return _mapper.Map<JobTitleDto>(entity);
        }

        public async Task<JobTitleDto> CreateAsync(JobTitleInputDto input, CancellationToken ct = default)
        {
            var nameExists = await _jobTitleRepository.ExistsAsync(x => x.Name.ToLower() == input.Name.ToLower(), ct);
            if (nameExists)
                throw new AppException($"Job Title with name '{input.Name}' already exists.");

            var entity = _mapper.Map<JobTitle>(input);
            await _jobTitleRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<JobTitleDto>(entity);
        }

        public async Task<JobTitleDto> UpdateAsync(JobTitleInputDto input, CancellationToken ct = default)
        {
            var id     = input.Id ?? throw new AppException("Id is required for updating a Job Title.");
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException("JobTitle", id);

            var nameExists = await _jobTitleRepository.ExistsAsync(
                x => x.Name.ToLower() == input.Name.ToLower() && x.Id != id, ct);
            if (nameExists)
                throw new AppException($"Job Title with name '{input.Name}' already exists.");

            _mapper.Map(input, entity);
            _jobTitleRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<JobTitleDto>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException("JobTitle", id);
            _jobTitleRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
