using System.ComponentModel.DataAnnotations;

namespace TransactionsAPI.Domain.Models
{
	/// <summary>
	/// Transaction for account
	/// </summary>
	public class TransactionViewModel
	{
		public int AccountId { get; set; }
		public double Amount { get; set; }
	}
}
