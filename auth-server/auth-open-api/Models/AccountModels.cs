using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achi.Server.Models
{
	public class RegisterModel
	{
		public string client_id;
		public string client_secret;
		public string email;
		public string login;
		public string password;
		public object user_info;
	}

	public class RemoveLoginModel
	{
	}

	public class AddExternalLoginModel
	{
	}

	public class SetPassordModel
	{
	}

	public class ChangePassordModel
	{
	}
}