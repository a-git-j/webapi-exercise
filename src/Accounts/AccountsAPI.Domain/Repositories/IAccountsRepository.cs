using System.Collections.Generic;
using System.Threading.Tasks;
using AccountsAPI.Domain.Models;

namespace AccountsAPI.Domain.Infrastructure
{
	public interface IAccountsRepository
	{
		Task<AccountViewModel> CreateAccountAsync(int customerId);
		Task<bool> RemoveAccountAsync(int accountId);
		Task<IEnumerable<AccountViewModel>> GetCustomerAccountsAsync(int customerId);		
	}
}
