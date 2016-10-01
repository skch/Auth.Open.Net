using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achi.Security
{
	public class AuthSecurity
	{
		public static string GetPasswordHash(string password)
		{
			return password;
		}

		public static string CreateNewToken()
		{
			var time = DateTime.UtcNow.Ticks - 1000000000;
			return String.Format("ACT{0}TC", time);
		}
	}
}
