using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.RequestModel
{
	public class AddEditServiceRequest
	{
		public int ServiceID { get; set; }
		public string ServiceName { get; set; }
		public decimal ServicePrice { get; set; }
		public int ServiceDuration { get; set; }
	}
}
