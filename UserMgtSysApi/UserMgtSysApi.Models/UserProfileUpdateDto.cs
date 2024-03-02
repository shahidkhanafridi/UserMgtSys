using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgtSysApi.Models
{
	public class UserProfileUpdateDto
	{
		public string Email { get; set; }
		public string UserId { get; set; }
		// Add other fields for updating profile information
	}
}
