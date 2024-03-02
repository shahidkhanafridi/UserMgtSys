global using UserMgtSysApi.Data;
global using UserMgtSysApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMgtSysApi.Services.Interfaces;

namespace UserMgtSysApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly IUserService _userService;

		public UserController(IUserService userService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
		{
			_userService = userService;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationModel model)
		{
            // Check if there are any users in the system
            if (!_userManager.Users.Any())
			{
				// If no users exist, ensure the UserName is "admin"
				if (model.UserName.ToLower() != "admin")
				{
					return BadRequest("The first user must have UserName 'admin'.");
				}
			}

			string roleToAssign = string.Empty;

			// Check if the UserName property contains "admin"
			if (model.UserName.ToLower() == "admin")
			{
				// Check if the role "Admin" exists
				var adminRole = await _roleManager.FindByNameAsync("Admin");

				if (adminRole == null)
				{
					return BadRequest("No Admin role exists, please first create a role with the name 'Admin'.");
				}
				else
				{
					roleToAssign = "Admin";
				}
			}
			else
			{
				// Check if the role "Initial User" exists
				var initialUserRole = await _roleManager.FindByNameAsync("Initial User");

				if (initialUserRole == null)
				{
					return BadRequest("Initial User role does not exist, please first create the 'Initial User' role.");
				}
				else
				{
					roleToAssign = "Initial User";
				}
			}

			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, roleToAssign);

				return Ok("User registered successfully.");
			}

			return BadRequest(result.Errors);
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			#region ForTokenSignIn
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var token = GenerateJwtToken(user);
				return Ok(new { Token = token });
			}

			return BadRequest("Invalid login attempt.");

			#endregion
		}
		private string GenerateJwtToken(ApplicationUser user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

			var roles = _userManager.GetRolesAsync(user).Result;

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new(ClaimTypes.NameIdentifier, user.Id),
					new(ClaimTypes.Name, user.UserName),
					new(ClaimTypes.Email, user.Email),
					new(ClaimTypes.Role, string.Join(",", roles)),
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"]
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await _signInManager.SignOutAsync();
				return Ok("Logout successful.");
			}
			catch (Exception ex)
			{
				return BadRequest("Invalid logout attempt.");
			}
		}

		[Authorize]
		[HttpGet("profile")]
		public async Task<IActionResult> GetUserProfile()
		{
			// Get the current user's information
			var userIdClaim = User.FindFirst("sub");
			var userId = userIdClaim?.Value;
			//var user = await _userManager.FindByIdAsync(userId);
			var user = await _userService.GetUserProfile(userId);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			return Ok(user);
		}

		[Authorize]
		[HttpPut("profile")]
		public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto model)
		{
			//// Get the current user's information
			//var userIdClaim = User.FindFirst("sub");
			//var userId = userIdClaim?.Value;
			//var user = await _userManager.FindByIdAsync(userId);

			//if (user == null)
			//{
			//	return NotFound("User not found.");
			//}

			//// Update user profile details
			//user.Email = model.Email;
			//// Update other profile information as needed

			//var result = await _userManager.UpdateAsync(user);
			var result = _userService.UpdateUserProfile(model).Result;
			if (result.Succeeded)
			{
				return Ok("User profile updated successfully.");
			}

			return BadRequest(result.Errors);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			// Change password
			var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

			if (result.Succeeded)
			{
				return Ok("Password changed successfully.");
			}

			return BadRequest(result.Errors);
		}
	}
}
