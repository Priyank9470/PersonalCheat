using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.ResponseModel
{
	public class UserResponse
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
	}
}
