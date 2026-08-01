using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.Entity
{
	public class ServiceBooking
	{
		[Key]
		public int BookingID { get; set; }

		public int ServiceID { get; set; }

		[ForeignKey(nameof(ServiceID))]
		public Service Service { get; set; }
	}
}
