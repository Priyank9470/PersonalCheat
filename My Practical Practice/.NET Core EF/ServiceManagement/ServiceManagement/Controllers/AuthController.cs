using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using ServiceManagement.Core.Wrappers;
using ServiceManagement.Service.Interfaces;
using System.Net;

namespace ServiceManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			BaseResponseModel<LoginResponse> response = new();

			LoginResponse loginresponse = await _authService.AuthenticateUser(request);

			if (loginresponse == null)
			{
				response.IsSuccess = false;
				response.StatusCode = HttpStatusCode.Unauthorized;
				response.Message = "Invalid username or password.";
				return Unauthorized(response);
			}

			response.IsSuccess = true;
			response.StatusCode = HttpStatusCode.OK;
			response.Message = "Login successful.";
			response.Data = loginresponse;
			return Ok(response);
		}
	}
}
