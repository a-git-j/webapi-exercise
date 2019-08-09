using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomersAPI.Domain.Models
{
	/// <summary>
	/// Customer basic information
	/// </summary>
	public class CustomerViewModel
	{
		public string Name { get; set; }
		public string Surname { get; set; }
	}
}
