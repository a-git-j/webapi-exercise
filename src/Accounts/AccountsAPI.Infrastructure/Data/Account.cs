using System;
using System.Collections.Generic;
using System.Text;
using SimpleWebApi.Core.Data;

namespace AccountsAPI.Infrastructure.Models
{
	public class Account : IKeyedTestData
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
	}
}

