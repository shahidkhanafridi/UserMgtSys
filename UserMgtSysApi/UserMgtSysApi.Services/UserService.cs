using Microsoft.AspNetCore.Identity;
using UserMgtSysApi.Data;
using UserMgtSysApi.Models;
using UserMgtSysApi.Services.Interfaces;

namespace UserMgtSysApi.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<UserProfileDto> GetUserProfile(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				// Handle not found case
				return null;
			}

			var userProfile = new UserProfileDto
			{
				UserId = user.Id,
				Email = user.Email,
				// Add other profile information as needed
			};

			return userProfile;
		}

		public async Task<IdentityResult> UpdateUserProfile(UserProfileUpdateDto model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user == null)
			{
				// Handle not found case
				return null;
			}

			// Update user profile details
			user.Email = model.Email;
			// Update other profile information as needed

			var result = await _userManager.UpdateAsync(user);

			return result;
		}

		// Implement other methods from IUserService
	}
}
