using System.Threading.Tasks;
using CustomersAPI.Domain.Models;

namespace CustomersAPI.Domain.Infrastructure
{
	public interface ICustomersRepository
	{
		Task<CustomerViewModel> GetCustomerAsync(int customerId);
	}
}
