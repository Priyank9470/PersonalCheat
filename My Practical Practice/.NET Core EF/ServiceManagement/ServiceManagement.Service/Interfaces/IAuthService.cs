using ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Service.Interfaces
{
	public interface IAuthService
	{
		public Task<LoginResponse> AuthenticateUser(LoginRequest loginRequest);
	}
}
