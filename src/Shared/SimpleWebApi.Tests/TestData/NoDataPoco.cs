using System;
using System.Collections.Generic;
using System.Text;
using SimpleWebApi.Core.Data;

namespace SimpleWebApi.Tests.TestData
{
	public class NoDataPoco : IKeyedTestData
	{
		public int Id { get; set; }
		public string OtherProperty { get; set; }
	}
}
