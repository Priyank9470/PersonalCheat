using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Service.Interfaces
{
	public interface IUserService
	{
		public Task<int> AddEditUser(UserRequest userRequest);
	}
}
