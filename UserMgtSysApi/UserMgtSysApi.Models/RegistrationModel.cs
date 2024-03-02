﻿namespace UserMgtSysApi.Models
{
	public class RegistrationModel
	{
		public required string FirstName { get; set; }
		public string? LastName { get; set; }
		public required string UserName { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public required string ConfirmPassword { get; set; }
	}
}
