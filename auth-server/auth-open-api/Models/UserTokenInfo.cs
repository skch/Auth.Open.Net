using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achi.Server.Models
{
	public class UserTokenInfo
	{
		public UserTokenInfo() { }

		public UserTokenInfo(UserTokenDocument doc) {
			access_token = doc.token;
			token_type = doc.type;
			var ex = doc.expires - DateTime.Now;
			expires_in = Convert.ToInt32(Math.Round(ex.TotalSeconds, 0));
		}

		
		public string access_token;
		public string token_type;
		public int expires_in;
	}
}