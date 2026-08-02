using ServiceManagement.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Repository.Interface
{
	public interface IServiceRepository
	{
		public Task<(List<Service> Items, int TotalRecords)> GetAllServices(string searchText, int pageNumber, int pageSize);
		public Task<int> AddEditService(Service service);
		public Task<Service> GetServiceById(int serviceId);
		public Task<bool> DeleteService(int serviceId);
	}
}
