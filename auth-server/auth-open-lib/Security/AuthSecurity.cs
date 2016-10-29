using Sodium;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Achi.Security
{
	public class AuthSecurity
	{

		private static byte[] getHash(string inputString)
		{
			HashAlgorithm algorithm = SHA1.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}

		private static string getHashString(string inputString)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in getHash(inputString))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}


		public static string GetPasswordHash(string password)
		{
			//this will produce a 32 byte hash and salt, to verify
			try
			{
				var hash = PasswordHash.ScryptHashString(password, PasswordHash.Strength.Medium);
				return hash;
			} catch(Exception ex)
			{
				// need to avoid this
				return getHashString(password);
			}
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
