using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserMgtSysApi.Services.Interfaces;

namespace UserMgtSysApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _roleService.GetAllRoles();
			return Ok(roles);
		}

		[HttpGet("{roleId}")]
		public async Task<IActionResult> GetRoleById(string roleId)
		{
			var role = await _roleService.GetRoleById(roleId);
			if (role == null)
			{
				return NotFound("Role not found.");
			}
			return Ok(role);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole([FromBody] IdentityRole role)
		{
			var result = await _roleService.CreateRole(role);
			if (result.Succeeded)
			{
				return Ok("Role created successfully.");
			}
			return BadRequest(result.Errors);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateRole([FromBody] IdentityRole role)
		{
			var result = await _roleService.UpdateRole(role);
			if (result.Succeeded)
			{
				return Ok("Role updated successfully.");
			}
			return BadRequest(result.Errors);
		}

		[HttpDelete("{roleId}")]
		public async Task<IActionResult> DeleteRole(string roleId)
		{
			var result = await _roleService.DeleteRole(roleId);
			if (result.Succeeded)
			{
				return Ok("Role deleted successfully.");
			}
			return BadRequest(result.Errors);
		}
	}
}
