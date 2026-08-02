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
	[Authorize(Roles = "Admin")]
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
		public async Task<IActionResult> GetAllServices(string? searchText, int pageNumber, int pageSize)
		{
			BaseResponseModel<List<ServiceResponse>> response = new();
			(List<ServiceResponse> Items, int TotalRecords) = await _serviceService.GetAllServices(searchText, pageNumber, pageSize);

			if (Items != null && Items.Count > 0)
			{
				response.StatusCode = HttpStatusCode.OK;
				response.Data = Items;
				response.TotalRecords = TotalRecords;
				response.IsSuccess = true;
				response.Message = "Services retrieved successfully.";
				return Ok(response);
			}
			response.StatusCode = HttpStatusCode.NoContent;
			response.Message = "No services found.";
			return Ok(response);
		}

		[HttpPost("AddEditService")]
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

		[HttpGet("GetServiceById")]
		public async Task<IActionResult> GetServiceById(int id)
		{
			BaseResponseModel<ServiceResponse> response = new();
			ServiceResponse service = await _serviceService.GetServiceById(id);

			if (service != null)
			{
				response.StatusCode = HttpStatusCode.OK;
				response.Data = service;
				response.IsSuccess = true;
				response.Message = "Service retrieved successfully.";
				return Ok(response);
			}
			response.StatusCode = HttpStatusCode.NotFound;
			response.IsSuccess = true;
			response.Message = "Service not found.";
			return NotFound(response);
		}

		[HttpDelete("DeleteService")]
		public async Task<IActionResult> DeleteService(int id)
		{
			BaseResponseModel<bool> response = new();
			bool isDeleted = await _serviceService.DeleteService(id);
			if (isDeleted)
			{
				response.StatusCode = HttpStatusCode.OK;
				response.Data = true;
				response.IsSuccess = true;
				response.Message = "Service deleted successfully.";
				return Ok(response);
			}
			else
			{
				response.StatusCode = HttpStatusCode.NotFound;
				response.Data = false;
				response.IsSuccess = false;
				response.Message = "Service not found or could not be deleted.";
				return NotFound(response);
			}
		}
	}
}
