using Microsoft.EntityFrameworkCore;
using ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Repository.Classes
{
	public class UserRepository : IUserRepository
	{
		private readonly ServiceManagementDBContext _context;
		public UserRepository(ServiceManagementDBContext context)
		{
			_context = context;
		}
		public async Task<User> AuthenticateUser(LoginRequest loginRequest)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => (u.Email == loginRequest.UserName || u.UserName == loginRequest.UserName) && u.Password == loginRequest.Password);
		}

		public async Task<int> AddEditUser(User user)
		{
			if (user.UserId == 0)
			{
				await _context.AddAsync(user);
			}
			else
			{
				var existingUser = await _context.Users.FindAsync(user.UserId);
				if (existingUser != null)
				{
					existingUser.UserName = user.UserName;
					existingUser.Email = user.Email;
					existingUser.Password = user.Password;
					await _context.UpdateAsync(existingUser);
				}
			}
			return user.UserId;
		}
	}
}
