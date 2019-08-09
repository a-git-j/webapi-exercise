namespace AccountsAPI.Domain.Models
{
	/// <summary>
	/// Data for creation of new account
	/// </summary>
	public class NewAccount
	{
		public int CustomerId { get; set; }
		public double? InitialCredit { get; set; }		
	}
}
