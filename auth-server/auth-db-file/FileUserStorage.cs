using Achi.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Achi
{
	public class FileUserStorage : IUserDataStorage
	{

		private bool isOpen = false;

		private string _dbPath = null;
		protected string dbPath
		{
			get
			{
				if (_dbPath == null)
				{
					string dpath = ConfigurationManager.AppSettings["db-path"];
					_dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dpath);
				}
				return _dbPath;
			}
		}

		#region Interface methods

		public bool IsOpen()
		{
			return isOpen;
		}

		public async Task InitAsync()
		{
			isOpen = await DocumentExists("config", "storage");
		}

		public async Task SaveDocument(string type, string id, JToken data) {
			string fname = Path.Combine(dbPath, type+"-" + id + ".json");
			await writeAllText(fname, data.ToString());
		}


		public async Task<JObject> GetDocument(string type, string id) {
			try
			{
				string fname = Path.Combine(dbPath, type+"-" + id + ".json");
				if (!File.Exists(fname)) return createError("Document not found");
				string jtx = await readAllText(fname);
				return JObject.Parse(jtx);
			}
			catch (Exception ex)
			{
				return createFatalError(ex);
			}
		}


		public async Task<bool> DocumentExists(string type, string id) {
			string fname = Path.Combine(dbPath, type + "-" + id + ".json");
			await Task.Yield();
			return File.Exists(fname);
		}

		public async Task DeleteDocument(string type, string id) {
			string fname = Path.Combine(dbPath, type + "-" + id + ".json");
			if (!File.Exists(fname)) return;
			FileInfo fi = new FileInfo(fname);
			await FileDeleteAsync(fi);
		}

		#endregion


		public static Task FileDeleteAsync(FileInfo fi)
		{
			return Task.Factory.StartNew(() => fi.Delete());
		}






		#region Private methods
		private JObject createError(string msg)
		{
			var res = new JObject();
			res.Add(new JProperty("error", msg));
			return res;
		}

		private JObject createFatalError(Exception ex)
		{
			var res = new JObject();
			res.Add(new JProperty("error", "Exception: "+ex.Message));
			return res;
		}

		// Read file contents to a string
		private async Task<string> readAllText(string fname)
		{
			string res = "";
			using (var reader = File.OpenText(fname))
			{
				res = await reader.ReadToEndAsync();
			}
			return res;
		}

		private async Task writeAllText(string fname, string data)
		{
			using (StreamWriter writer = File.CreateText(fname))
			{
				await writer.WriteAsync(data);
				writer.Flush();
			}

		}

		#endregion

	}
}
