using AutoMapper;
using ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using ServiceManagement.Repository.Interface;
using ServiceManagement.Service.Authentication;
using ServiceManagement.Service.Helpers;
using ServiceManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Service.Classes
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<int> AddEditUser(UserRequest userRequest)
		{
			User user = _mapper.Map<User>(userRequest);
			user.Password = Cryptographer.EncryptPassword(userRequest.Password);
			user.Role = "Admin";
			return await _userRepository.AddEditUser(user);
		}
	}
}
