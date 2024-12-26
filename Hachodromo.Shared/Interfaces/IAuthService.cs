using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hachodromo.Shared.Interfaces
{
	public interface IAuthService
	{
		Task<string?> Authenticate(string email, string password);
	}
}
