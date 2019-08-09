using System;
using SimpleWebApi.Core.Data;

namespace CustomersAPI.Infrastructure.Models
{
	public class Customer : IKeyedTestData
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }

	}
}
