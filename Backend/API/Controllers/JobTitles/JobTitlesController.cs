using API.Controllers.Common;
using Application.Features.JobTitles;
using Application.Features.JobTitles.DTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers.JobTitles
{
    [ApiController]
    [Tags("Job Titles")]
    public class JobTitlesController(IJobTitleAppService jobTitleAppService) : BaseApiController
    {
        private readonly IJobTitleAppService _jobTitleAppService = jobTitleAppService;

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<JobTitleDto>>> GetAll([FromQuery] GetAllJobTitlesInput input, CancellationToken ct)
        {
            var result = await _jobTitleAppService.GetAllAsync(input, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<JobTitleDto>> GetById(int id, CancellationToken ct)
        {
            var result = await _jobTitleAppService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<JobTitleDto>> Create([FromBody] JobTitleInputDto input, CancellationToken ct)
        {
            var result = await _jobTitleAppService.CreateAsync(input, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<JobTitleDto>> Update(int id, [FromBody] JobTitleInputDto input, CancellationToken ct)
        {
            if (id != input.Id)
            {
                return BadRequest("The identifier in the URL path must match the identifier in the request body.");
            }

            var result = await _jobTitleAppService.UpdateAsync(input, ct);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _jobTitleAppService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
