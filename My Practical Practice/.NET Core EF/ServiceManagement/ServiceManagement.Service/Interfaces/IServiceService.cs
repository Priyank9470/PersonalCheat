using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Service.Interfaces
{
	public interface IServiceService
	{
		public Task<(List<ServiceResponse> Items, int TotalRecords)> GetAllServices(string searchText, int pageNumber, int pageSize);
		public Task<int> AddEditservice(AddEditServiceRequest request);
		public Task<ServiceResponse> GetServiceById(int serviceId);
		public Task<bool> DeleteService(int serviceId);
	}
}
