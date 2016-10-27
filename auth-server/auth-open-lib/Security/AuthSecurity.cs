using Sodium;
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
			//this will produce a 32 byte hash and salt, to verify
			var hash = PasswordHash.ScryptHashString(password, PasswordHash.Strength.Medium);
			return hash;
		}

		public static bool IsPasswordValid(string hash, string password)
		{
			return PasswordHash.ScryptHashStringVerify(hash, password);
		}

		public static string CreateNewToken()
		{
			var time = DateTime.UtcNow.Ticks - 1000000000;
			return String.Format("ACT{0}TC", time);
		}
	}
}
