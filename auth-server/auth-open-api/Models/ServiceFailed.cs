using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achi.Server.Models
{
	public class ServiceFailed:ServiceResponse
	{
		public ServiceFailed()
		{
			status = "failed";
		}
	}
}
