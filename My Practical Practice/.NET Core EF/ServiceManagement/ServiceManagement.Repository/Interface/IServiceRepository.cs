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
		public Task<List<Service>> GetAllServices(string searchText);
		public Task<int> AddEditService(Service service);
	}
}
