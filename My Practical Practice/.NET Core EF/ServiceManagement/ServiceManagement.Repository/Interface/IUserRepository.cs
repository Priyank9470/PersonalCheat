using ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Repository.Interface
{
	public interface IUserRepository
	{
		public Task<int> AddEditUser(User user);
		public Task<User> AuthenticateUser(LoginRequest loginRequest);
	}
}
