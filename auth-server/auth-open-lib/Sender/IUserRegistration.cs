using System.Threading.Tasks;

namespace Achi.Sender
{
	public interface IUserRegistration
	{
		Task Send(string address, string subject, string body);
	}
}
