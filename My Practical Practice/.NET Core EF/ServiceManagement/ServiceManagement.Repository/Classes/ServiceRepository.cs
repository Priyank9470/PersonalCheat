using Microsoft.EntityFrameworkCore;
using ServiceManagement.Core.Entity;
using ServiceManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Repository.Classes
{
	public class ServiceRepository : IServiceRepository
	{
		ServiceManagementDBContext _dbContext;
		public ServiceRepository(ServiceManagementDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Service>> GetAllServices(string searchText)
		{
			return await _dbContext.Services
						.Where(s => s.IsActive &&
							(string.IsNullOrEmpty(searchText) ||
								 s.ServiceName.Contains(searchText) ||
								 s.ServiceNumber.Contains(searchText)))
						.ToListAsync();
		}

		public async Task<int> AddEditService(Service service)
		{
			if (service.ServiceId > 0)
			{
				Service oService = await _dbContext.Services.FindAsync(service.ServiceId);
				if(oService != null)
				{
					oService.ServicePrice = service.ServicePrice;
					oService.ServiceName = service.ServiceName;
					oService.ServiceName = service.ServiceName;
				}
				await _dbContext.UpdateAsync(oService);
			}
			else
			{
				service.ServiceNumber = "S_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				await _dbContext.AddAsync(service);
			}
			return service.ServiceId;
		}
	}
}
