using System;

namespace Achi.Storage.Mock
{
	public class MockDataStorage : IUserDataStorage
	{
		public bool Authenticating(string login, string password, string clientId)
		{
			return true;
		}

		public void BindTokenWithUser(string login, string token)
		{
			throw new NotImplementedException();
		}
	}
}
