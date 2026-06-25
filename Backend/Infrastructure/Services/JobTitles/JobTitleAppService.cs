using Application.Common.Interfaces.Persistence;
using Application.Features.JobTitles;
using Application.Features.JobTitles.DTOs;
using AutoMapper;
using Domain.Entities.JobTitles;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Models;

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
            var query = _jobTitleRepository.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => x.Name.Contains(input.Filter) ||
                                         (x.Description != null && x.Description.Contains(input.Filter)));
            }

            var totalCount = await query.CountAsync(ct);

            var sortBy = input.SortBy?.Trim().ToLowerInvariant() ?? "description";
            var ascending = input.SortAscending ?? true;
            IOrderedQueryable<JobTitle> orderedQuery;

            if (sortBy == "name")
            {
                orderedQuery = ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (sortBy == "isactive")
            {
                orderedQuery = ascending ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
            }
            else
            {
                orderedQuery = ascending ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }

            var itemsQuery = orderedQuery.AsQueryable();

            if (input.ApplyPagination == true)
            {
                int page = input.CurrentPage ?? 1;
                if (page < 1) page = 1;

                int size = input.PageSize ?? 10;
                if (size <= 0) size = 10;

                int skipCount = (page - 1) * size;
                itemsQuery = itemsQuery.Skip(skipCount).Take(size);
            }

            var items = await itemsQuery.ToListAsync(ct);
            var dtos = _mapper.Map<IReadOnlyList<JobTitleDto>>(items);

            return new PagedResultDto<JobTitleDto>(totalCount, dtos);
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
            {
                throw new AppException($"Job Title with name '{input.Name}' already exists.");
            }

            var entity = _mapper.Map<JobTitle>(input);
            await _jobTitleRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<JobTitleDto>(entity);
        }

        public async Task<JobTitleDto> UpdateAsync(JobTitleInputDto input, CancellationToken ct = default)
        {
            var id = input.Id ?? throw new AppException("Id is required for updating a Job Title.");
            var entity = await _jobTitleRepository.GetByIdAsync(id, ct) ?? throw new NotFoundException("JobTitle", id);
            var nameExists = await _jobTitleRepository.ExistsAsync(x => x.Name.ToLower() == input.Name.ToLower() && x.Id != id, ct);
            if (nameExists)
            {
                throw new AppException($"Job Title with name '{input.Name}' already exists.");
            }

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
