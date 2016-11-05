using Achi.Security;
using Achi.Server.Models;
using Achi.Server.Storage;
using Newtonsoft.Json;
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
	public class AuthenticateController : BaseController
	{
		// Make sure the controller is connected to the data provider
		public AuthenticateController():base()
		{			
		}


		// POST: api/Authenticate/password
		// Authenticate user by user name and password
		[Route("api/Authenticate/password")]
		[HttpPost]
		public async Task<IHttpActionResult> Post(SimpleCredentials cr)
		{
			if (!await InitDb()) return InternalServerError();

			var user = await ApiCallSession.DB.GetDocument("user",cr.login);
			if (user["error"] != null) return this.Unauthorized();

			string passwordHash = user["password"].ToString();			
			if (!AuthSecurity.IsPasswordValid(passwordHash, cr.password)) return Unauthorized();

			var doc = await CreateNewToken(cr.login);

			return Ok(SaveToken(doc));
		}

		//POST: api/Authenticate/token
		//Get user details by token
		[Route("api/Authenticate/UserInfo")]
		[HttpPost]
		public async Task<IHttpActionResult> Validate(TokenCredentials tc)
		{
			if (!await InitDb()) return InternalServerError();
			//if(tc.client_id == )
			var token = await ApiCallSession.DB.GetDocument("token", tc.access_token);

			if (token["error"] != null) return Unauthorized();
			UserTokenDocument doc = token.ToObject<UserTokenDocument>();
			
			if (doc.expires <= DateTime.Now)
			{
				await ApiCallSession.DB.DeleteDocument("token", tc.access_token);
				return Unauthorized();
			}

			var juser = await ApiCallSession.DB.GetDocument("user", doc.user);
			if (juser["error"] != null) return this.NotFound();

			UserInfoDocument user = juser.ToObject<UserInfoDocument>();
			
			return Ok(user);
		}		
	}
}
