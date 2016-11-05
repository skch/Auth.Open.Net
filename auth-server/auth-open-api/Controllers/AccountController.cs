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
using System.Web.Http;

namespace Achi.Server.Controllers
{
	public class AccountController : BaseController
	{		
		// Make sure the controller is connected to the data provider
		public AccountController() : base()
		{			
		}	
		
		// POST api/Account/Logout
		[Route("Logout")]
		public IHttpActionResult Logout()
		{
			return Ok();
		}

		// GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
		[Route("ManageInfo")]
		public async Task<ServiceResponse> GetManageInfo(string returnUrl, bool generateState = false)
		{
			return null;
		}

		// POST api/Account/ChangePassword
		[Route("ChangePassword")]
		public async Task<IHttpActionResult> ChangePassword(ChangePassordModel model)
		{

			return Ok();
		}

		// POST api/Account/SetPassword
		[Route("SetPassword")]
		public async Task<IHttpActionResult> SetPassword(SetPassordModel model)
		{
			return Ok();
		}

		// POST api/Account/AddExternalLogin
		[Route("AddExternalLogin")]
		public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginModel model)
		{
		
			return Ok();
		}

		// POST api/Account/RemoveLogin
		[Route("RemoveLogin")]
		public async Task<IHttpActionResult> RemoveLogin(RemoveLoginModel model)
		{

			return Ok();
		}

		// GET api/Account/ExternalLogin
		[OverrideAuthentication]
		[AllowAnonymous]
		[Route("ExternalLogin", Name = "ExternalLogin")]
		public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
		{
			return Ok();
		}

		// GET api/Account/Confirmation?token=%2F
		[AllowAnonymous]
		[Route("api/Account/Confirmation")]
		public async Task<IHttpActionResult> Confirmation(string token)
		{
			if (!await InitDb()) return InternalServerError();

			var doc = await ApiCallSession.DB.GetDocument("token", token);			
			if (doc["error"] != null) return InternalServerError();

			UserTokenDocument userToken = doc.ToObject<UserTokenDocument>();

			if(userToken.expires <= DateTime.Now)
			{
				await ApiCallSession.DB.DeleteDocument("token", token);
				//token expired -> return error
				return InternalServerError();
			}
			//delete temporary token
			await ApiCallSession.DB.DeleteDocument("token", token);
						
			//generate new token
			var newToken = await CreateNewToken(userToken.user);
			var ju = await ApiCallSession.DB.GetDocument("user", newToken.user);
			var u = ju.ToObject<User>();
			u.inactive = false;
			//activate user
			await ApiCallSession.DB.SaveDocument("user", u.email, JObject.FromObject(u));
			//send email
			await ApiCallSession.Sender.Send(u.email, "Registration finished", "You're registred successfully!");					
			//save new token in db
			var user = await SaveToken(newToken);

			return Ok(user);
		}

		// POST api/Account/Register
		[AllowAnonymous]
		[Route("api/Account/Register")]
		public async Task<IHttpActionResult> Register(RegisterModel model)
		{
			if (!await InitDb()) return InternalServerError();

			var user = await ApiCallSession.DB.GetDocument("user", model.email);
			//User exist - return correct error
			if (user["error"] == null) return InternalServerError();

			string receivedPasswordHash = AuthSecurity.GetPasswordHash(model.password);

			User newUser = new Models.User() {
				email = model.email,
				inactive = true,
				password = receivedPasswordHash
				};
			var juser = JObject.FromObject(newUser);
			juser.Merge(JObject.FromObject(model.user_info));		

			await ApiCallSession.DB.SaveDocument("user", model.email, juser);

			var doc = new UserTokenDocument()
			{
				token = AuthSecurity.CreateNewToken(),
				type = "validation",
				user = model.email,
				expires = DateTime.Now.AddMinutes(5)
			};

			var res = SaveToken(doc);			 
			//send email
			string url = Url.Link("DefaultApi", new { controller = "Account/Confirmation", token = doc.token });			
			await ApiCallSession.Sender.Send(model.email, "Confirm registration", url);
			return Ok(res.Result);
		}

		// POST api/Account/RegisterExternal
		[OverrideAuthentication]
		[Route("RegisterExternal")]
		public async Task<IHttpActionResult> RegisterExternal(RegisterModel model)
		{
			return Ok();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

	}
}
