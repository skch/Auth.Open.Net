
namespace Achi.Storage
{
	public interface IUserDataStorage
	{
		bool Authenticating(string login, string password, string clientId);
		void BindTokenWithUser(string login, string token);
	}
}
