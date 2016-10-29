using Sodium;
using System;

namespace Achi.Security
{
	public class AuthSecurity
	{
		public static string GetPasswordHash(string password)
		{
			//this will produce a 32 byte hash and salt, to verify
			try
			{
				var hash = PasswordHash.ScryptHashString(password, PasswordHash.Strength.Medium);
				return hash;
			} catch(Exception ex)
			{

			}
			return password;
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
