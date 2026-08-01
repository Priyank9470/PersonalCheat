using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Service.Interfaces;

namespace ServiceManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IValidator<UserRequest> _validator;

		public UserController(IUserService userService, IValidator<UserRequest> validator)
		{
			_userService = userService;
			_validator = validator;
		}

		[HttpPost("AddEditUser")]
		public async Task<IActionResult> AddEditUser(UserRequest userRequest)
		{
			if (userRequest == null)
			{
				return BadRequest("Invalid Request");
			}

			var validationResult = await _validator.ValidateAsync(userRequest);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.ToDictionary());
			}

			int userID = await _userService.AddEditUser(userRequest);
			if (userID > 0)
			{
				return Ok(new { IsSuccess = true, Message = "User added/edited successfully.", UserID = userID });
			}
			else
			{
				return BadRequest(new { IsSuccess = false, Message = "Failed to add/edit user." });
			}
		}
	}
}
