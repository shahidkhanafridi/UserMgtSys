﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgtSysApi.Models
{
	public class ChangePasswordDto
	{
		public string UserId { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
