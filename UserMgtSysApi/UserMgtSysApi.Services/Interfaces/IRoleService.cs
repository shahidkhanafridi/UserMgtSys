using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgtSysApi.Services.Interfaces
{
	public interface IRoleService
	{
		Task<IEnumerable<IdentityRole>> GetAllRoles();
		Task<IdentityRole> GetRoleById(string roleId);
		Task<IdentityResult> CreateRole(IdentityRole role);
		Task<IdentityResult> UpdateRole(IdentityRole role);
		Task<IdentityResult> DeleteRole(string roleId);
	}
}
