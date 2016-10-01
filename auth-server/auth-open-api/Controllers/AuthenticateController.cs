using Achi.Security;
using Achi.Server.Models;
using Achi.Server.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Achi.Server.Controllers
{
	public class AuthenticateController : ApiController
	{
		public Task Initilization { get; private set; }

		// Make sure the controller is connected to the data provider
		public AuthenticateController()
		{
			Initilization = ApiCallSession.InitializeAsync();
		}


		// POST: api/Authenticate/password
		// Authenticate user by user name and password
		[Route("api/Authenticate/password")]
		[HttpPost]
		public async Task<IHttpActionResult> Post(SimpleCredentials cr)
		{
			await Initilization;
			if (!ApiCallSession.DB.IsOpen()) return InternalServerError();
			var user = await ApiCallSession.DB.GetDocument("user",cr.login);
			if (user["error"] != null) return this.Unauthorized();

			string passwordHash = user["password"].ToString();
			string receivedPasswordHash = AuthSecurity.GetPasswordHash(cr.password);

			if (passwordHash != receivedPasswordHash) return Unauthorized();

			var doc = new UserTokenDocument() {
				token = AuthSecurity.CreateNewToken(),
				type = "temp",
				user = cr.login,
				expires = DateTime.Now.AddHours(12)
			};
			await ApiCallSession.DB.SaveDocument("token", doc.token, JObject.FromObject(doc));

			var res = new UserTokenInfo(doc);
			return Ok(res);
		}


	}
}
