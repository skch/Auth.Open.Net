using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achi.Server.Models
{
	public class ServiceResponse
	{
		public string status = "";

		public override string ToString()
		{
			return status;
		}
	}
}