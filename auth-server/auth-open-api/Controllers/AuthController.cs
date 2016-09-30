using Achi.Server.Generator;
using Achi.Server.Models;
using Achi.Storage;
using Achi.Storage.Mock;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Achi.Server.Controllers
{
	public class AuthController : ApiController
	{
		private IUserDataStorage storage;

		private Token token;
		public AuthController()
		{
			storage = new MockDataStorage();		
		}

		//POST
		[Route("Authenticate")]
		public HttpResponseMessage Authenticate(string post)// create class with login password
		{
			AuthenticateStatus status = new AuthenticateStatus();
			status = ValidateData(status, post);
			status = Authenticating(status);
			status = GenerateToken(status);
			status = BindToken(status);

			return this.Request.CreateResponse<string>(HttpStatusCode.OK, status.ToString());
		}

		private AuthenticateStatus ValidateData(AuthenticateStatus status, string data)
		{
			if (status.HasErrors) return status;
			try
			{
				JObject j = JObject.Parse(data);
				status.Login = j["login"].ToString();
				status.Password = j["password"].ToString();
				status.ClientId = j["clientId"].ToString();

				if (string.IsNullOrEmpty(status.Login) || string.IsNullOrEmpty(status.Password))
				{
					status.SetError("validation", "login, password or clientId is empty");
				}

			}
			catch (Exception ex)
			{
				status.SetError("validation", ex.Message);
			}
			return status;
		}

		private AuthenticateStatus Authenticating(AuthenticateStatus status)
		{
			if (status.HasErrors) return status;
			try
			{
				status.Authenticated = storage.Authenticating(status.Login, status.Password, status.ClientId);
				if (!status.Authenticated)
				{
					status.SetError("authenticating", "login or password is wrong");
				}
			}
			catch (Exception ex)
			{
				status.SetError("authenticating", ex.Message);
			}
			return status;
		}

		private AuthenticateStatus GenerateToken(AuthenticateStatus status)
		{
			if (status.HasErrors) return status;
			try
			{
				token = new Token();
				status.Token = token.Generate();
			}
			catch (Exception ex)
			{
				status.SetError("generateToken", ex.Message);
			}
			return status;
		}

		private AuthenticateStatus BindToken(AuthenticateStatus status)
		{
			if (status.HasErrors) return status;
			try
			{
				storage.BindTokenWithUser(status.Login, status.Token);
			}
			catch (Exception ex)
			{
				status.SetError("bindToken", ex.Message);
			}
			return status;
		}
	}
}
