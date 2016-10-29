using Achi.Sender;
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
		public static IUserRegistration Sender;

		public static async Task InitializeAsync()
		{

			// Connect to the data provider here
			DB = new FileUserStorage();
			Sender = new FileUserStorage();
			await DB.InitAsync();			
		}


	}
}