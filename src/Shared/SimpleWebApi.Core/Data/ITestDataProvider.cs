using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebApi.Core.Data
{
	public interface ITestDataProvider
	{
		ConcurrentBag<T> ReadTestData<T>() where T : class, new();
		ConcurrentDictionary<int, T> ReadKeyedTestData<T>() where T : class, IKeyedTestData, new();
		bool SaveTestData<T>(IEnumerable<T> valuesToSave) where T : class, new();
	}
}
