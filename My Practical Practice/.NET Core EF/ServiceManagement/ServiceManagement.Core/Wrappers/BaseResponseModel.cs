using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Core.Wrappers
{
	public class BaseResponseModel<T>
	{
		public bool IsSuccess { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
	}
}
