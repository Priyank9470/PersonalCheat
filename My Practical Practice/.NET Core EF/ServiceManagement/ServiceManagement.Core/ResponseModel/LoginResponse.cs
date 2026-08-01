using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.ResponseModel
{
	public class LoginResponse
	{
		public int UserId { get; set; }
		public string AuthToken { get; set; }
	}
}
