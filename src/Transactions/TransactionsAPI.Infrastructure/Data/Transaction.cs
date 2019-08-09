using SimpleWebApi.Core.Data;

namespace TransactionsAPI.Infrastructure.Models
{
	public class Transaction 
	{
		//since it's not relational db, don't bother with keeping transaction's Id

		public int AccountId { get; set; }
		public double Amount { get; set; }
	}
}
