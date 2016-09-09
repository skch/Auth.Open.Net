using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achi.Server.Models
{
	public class ServiceOK: ServiceResponse
	{

		public ServiceOK()
		{
			status = "success";
		}
	}
}