using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achi.Server.Models
{
	public class AuthenticateStatus
	{
		private bool hasErrors = false;
		private string lastError;

		public UserInfo UserInfo { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string ClientId { get; set; }
		public bool Authenticated { get; set; }
		public string Token { get; set; }
		public bool HasErrors
		{
			get
			{
				return hasErrors;
			}
		}

		public AuthenticateStatus SetError(string task, string msg)
		{
			hasErrors = true;
			lastError = msg;
			return this;
		}

		public override string ToString()
		{
			if (hasErrors)
			{
				return JsonConvert.SerializeObject(new ServiceFailed());
			}

			return JsonConvert.SerializeObject(new ServiceOK());
		}
	}
}
