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
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly JwtTokenGeneration _jwtTokenGeneration;

		public AuthService(IUserRepository userRepository, JwtTokenGeneration jwtTokenGeneration)
		{
			_userRepository = userRepository;
			_jwtTokenGeneration = jwtTokenGeneration;
		}

		public async Task<LoginResponse> AuthenticateUser(LoginRequest loginRequest)
		{
			loginRequest.Password = Cryptographer.EncryptPassword(loginRequest.Password); //set the encrypted password here.
			User user = await _userRepository.AuthenticateUser(loginRequest);
			if (user == null) return null;

			return new LoginResponse
			{
				UserId = user.UserId,
				AuthToken = _jwtTokenGeneration.GenerateToken(user)
			};
		}
	}
}
