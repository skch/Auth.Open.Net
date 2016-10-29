using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Achi.Server
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			// Using unmanaged DLLs like libsodium. Need to specify path
			string path = Environment.GetEnvironmentVariable("PATH");
			string binDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
			Environment.SetEnvironmentVariable("PATH", path + ";" + binDir);
		}
	}
}
