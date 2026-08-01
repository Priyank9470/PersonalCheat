using ServiceManagement.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.Entity
{
	public class Service : BaseModel
	{
		[Key]
		public int ServiceId { get; set; }

		[Required]
		public string ServiceNumber { get; set; }
		public string ServiceName { get; set; }
		public decimal ServicePrice { get; set; }
		public int ServiceDuration { get; set; }
		public bool IsActive { get; set; } = true;
		public ICollection<ServiceBooking> ServiceBookings { get; set; }
	}
}
