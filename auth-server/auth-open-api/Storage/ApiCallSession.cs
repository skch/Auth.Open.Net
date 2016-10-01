using Achi.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Achi.Server.Storage
{
	public class ApiCallSession
	{

		public static IUserDataStorage DB;

		public static async Task InitializeAsync(string appDataPath)
		{

			// Connect to the data provider here
			DB = new FileUserStorage() { dbPath = appDataPath };
			await DB.InitAsync();
		}


	}
}