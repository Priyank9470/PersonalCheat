using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using ServiceManagement.Core.Wrappers;
using ServiceManagement.Service.Classes;
using ServiceManagement.Service.Interfaces;
using System.Net;

namespace ServiceManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly IServiceService _serviceService;
		public ServiceController(IServiceService serviceService)
		{
			_serviceService = serviceService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="searchText"></param>
		/// <returns></returns>
		[HttpGet("GetAllServices")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllServices(string? searchText)
		{
			BaseResponseModel<List<ServiceResponse>> response = new();
			List<ServiceResponse> services = await _serviceService.GetAllServices(searchText);

			if (services != null && services.Count > 0)
			{
				response.StatusCode = HttpStatusCode.OK;
				response.Data = services;
				response.IsSuccess = true;
				response.Message = "Services retrieved successfully.";
				return Ok(response);
			}
			response.StatusCode = HttpStatusCode.NoContent;
			response.Message = "No services found.";
			return Ok(response);
		}

		[HttpPost("AddEditService")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> AddEditservice(AddEditServiceRequest request)
		{
			if (request == null)
			{
				return BadRequest("Invalid Request");
			}

			BaseResponseModel<int> response = new();
			int serviceID = await _serviceService.AddEditservice(request);

			if (serviceID > 0)
			{
				response.StatusCode = request.ServiceID > 0 ? HttpStatusCode.OK : HttpStatusCode.Created;
				response.Data = serviceID;
				response.IsSuccess = true;
				response.Message = $"Service {(request.ServiceID > 0 ? "Updated" : "Added")} successfully.";
				return Ok(response);
			}
			else
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				response.Message = $"Failed to {(request.ServiceID > 0 ? "Update" : "Add")} service.";
				return BadRequest(response);
			}
		}
	}
}
