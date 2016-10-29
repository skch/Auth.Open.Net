using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achi.Server.Models
{
	public class UserTokenDocument
	{
		public string token;
		public string type;
		public string user;
		public DateTime expires;
		public bool inactive = false;		
	}
}