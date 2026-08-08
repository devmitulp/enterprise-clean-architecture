using API.Controllers.Common;
using Application.Features.Roles;
using Application.Features.Roles.DTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers.Roles
{
    [ApiController]
    [Tags("Roles")]
    public class RolesController : BaseApiController
    {
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<RoleDto>>> GetAll([FromQuery] GetAllRolesInput input, CancellationToken ct)
        {
            var result = await _roleAppService.GetAllAsync(input, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleDto>> GetById(int id, CancellationToken ct)
        {
            var result = await _roleAppService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleInputDto input, CancellationToken ct)
        {
            var result = await _roleAppService.CreateOrUpdateRoleAsync(input, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] RoleInputDto input, CancellationToken ct)
        {
            if (id != input.Id)
            {
                return BadRequest("The identifier in the URL path must match the identifier in the request body.");
            }

            var result = await _roleAppService.CreateOrUpdateRoleAsync(input, ct);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _roleAppService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
