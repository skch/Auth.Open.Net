using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achi.Storage
{
	public interface IUserDataStorage
	{

		bool IsOpen();
		Task InitAsync();

		Task SaveDocument(string type, string id, JToken data);
		Task<JObject> GetDocument(string type, string id);
		Task<bool> DocumentExists(string type, string id);
		Task DeleteDocument(string type, string id);


	}
}
