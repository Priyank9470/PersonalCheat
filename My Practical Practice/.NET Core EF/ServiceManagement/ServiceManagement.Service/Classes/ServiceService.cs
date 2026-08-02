using AutoMapper;
using ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using ServiceManagement.Repository.Classes;
using ServiceManagement.Repository.Interface;
using ServiceManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = ServiceManagement.Core.Entity;

namespace ServiceManagement.Service.Classes
{
	public class ServiceService : IServiceService
	{
		private readonly IServiceRepository _serviceRepository;
		private readonly IMapper _mapper;
		public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
		{
			_serviceRepository = serviceRepository;
			_mapper = mapper;
		}

		public async Task<(List<ServiceResponse> Items, int TotalRecords)> GetAllServices(string searchText, int pageNumber, int pageSize)
		{
			(List<Entity.Service> services, int TotalRecords) = await _serviceRepository.GetAllServices(searchText, pageNumber, pageSize);
			return (_mapper.Map<List<ServiceResponse>>(services), TotalRecords);
		}

		public async Task<int> AddEditservice(AddEditServiceRequest request)
		{
			Entity.Service service = _mapper.Map<Entity.Service>(request);
			return await _serviceRepository.AddEditService(service);
		}

		public async Task<ServiceResponse> GetServiceById(int serviceId)
		{
			Entity.Service service = await _serviceRepository.GetServiceById(serviceId);
			return _mapper.Map<ServiceResponse>(service);
		}

		public async Task<bool> DeleteService(int serviceId)
		{
			return await _serviceRepository.DeleteService(serviceId);
		}
	}
}
