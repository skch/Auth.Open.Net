using Achi.Server.Models;
using Achi.Server.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Achi.Server.Controllers
{
    public class BaseController : ApiController
    {

		protected Task Initilization { get; private set; }

		// Make sure the controller is connected to the data provider
		public BaseController()
		{
			Initilization = ApiCallSession.InitializeAsync();
		}

		protected async Task<UserTokenInfo> SaveToken(UserTokenDocument doc)
		{
			await ApiCallSession.DB.SaveDocument("token", doc.token, JObject.FromObject(doc));
			return new UserTokenInfo(doc);
		}

		protected async Task<bool> InitDb()
		{
			await Initilization;
			return ApiCallSession.DB.IsOpen();
		}
	}
}
