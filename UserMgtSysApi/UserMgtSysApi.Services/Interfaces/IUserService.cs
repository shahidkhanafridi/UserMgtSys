using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgtSysApi.Models;

namespace UserMgtSysApi.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserProfileDto> GetUserProfile(string userId);
		Task<IdentityResult> UpdateUserProfile(UserProfileUpdateDto model);
		// Add other methods as needed
	}
}
