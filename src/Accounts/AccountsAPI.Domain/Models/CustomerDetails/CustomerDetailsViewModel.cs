using System.Collections.Generic;

namespace AccountsAPI.Domain.Models
{
	/// <summary>
	/// Customer and owned accounts
	/// Customer and owned accounts
	/// </summary>
	public class CustomerDetailsViewModel
	{
		public string Name { get; set; }
		public string Surname { get; set; }

		public IEnumerable<AccountViewModel> Accounts { get; set; }
	}
}
