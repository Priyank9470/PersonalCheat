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

		public async Task<List<ServiceResponse>> GetAllServices(string searchText)
		{
			List<Entity.Service> services = await _serviceRepository.GetAllServices(searchText);
			return _mapper.Map<List<ServiceResponse>>(services);
		}

		public async Task<int> AddEditservice(AddEditServiceRequest request)
		{
			Entity.Service service = _mapper.Map<Entity.Service>(request);
			return await _serviceRepository.AddEditService(service);
		}
	}
}
