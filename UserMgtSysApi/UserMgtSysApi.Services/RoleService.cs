using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgtSysApi.Services.Interfaces;

namespace UserMgtSysApi.Services
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleService(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<IEnumerable<IdentityRole>> GetAllRoles()
		{
			return await Task.FromResult(_roleManager.Roles.ToList());
		}

		public async Task<IdentityRole> GetRoleById(string roleId)
		{
			return await _roleManager.FindByIdAsync(roleId);
		}

		public async Task<IdentityResult> CreateRole(IdentityRole role)
		{
			return await _roleManager.CreateAsync(role);
		}

		public async Task<IdentityResult> UpdateRole(IdentityRole role)
		{
			var existingRole = await _roleManager.FindByIdAsync(role.Id);
			if (existingRole == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
			}

			existingRole.Name = role.Name;

			return await _roleManager.UpdateAsync(existingRole);
		}

		public async Task<IdentityResult> DeleteRole(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
			}

			return await _roleManager.DeleteAsync(role);
		}
	}
}
