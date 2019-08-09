using System.Collections.Generic;

namespace AccountsAPI.Domain.Models
{
	/// <summary>
	/// Account detailed information
	/// </summary>
	public class AccountViewModel
	{
		public int AccountId { get; set; }
		public double? CurrentBalance { get; set; }

		public IEnumerable<double> Transactions { get; set; }
	}
}
