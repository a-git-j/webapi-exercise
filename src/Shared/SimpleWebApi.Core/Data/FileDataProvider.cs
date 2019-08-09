using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleWebApi.Core.Extenstions;

namespace SimpleWebApi.Core.Data
{
	public class FileDataProvider : ITestDataProvider
	{
		public FileDataProvider(ILogger<FileDataProvider> logger)
		{
			_logger = logger;
		}
		private readonly ILogger<FileDataProvider> _logger;

		#region ITestDataProvider implementation
		public ConcurrentBag<T> ReadTestData<T>() where T : class, new()
		{
			var startInDirectory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), "TestData");
			return ReadTestData<T>(startInDirectory);
		}
		public ConcurrentDictionary<int, T> ReadKeyedTestData<T>() where T : class, IKeyedTestData, new()
		{
			var startInDirectory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), "TestData");
			return ReadKeyedTestData<T>(startInDirectory);
		}
		public bool SaveTestData<T>(IEnumerable<T> valuesToSave) where T : class, new()
		{
			var saveAs = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), "TestData", string.Format("{0}.json", typeof(T).Name));
			return SaveJsonFile<T>(valuesToSave, saveAs);
		}
		#endregion

		#region Helper testable methods
		internal ConcurrentBag<T> ReadTestData<T>(string path) where T : class, new()
		{
			return ReadTestData<T, ConcurrentBag<T>>(path, (items, collection) => {
				items.ForEach(x =>
				{
					collection.Add(x);
				});
			});
		}
		internal ConcurrentDictionary<int, T> ReadKeyedTestData<T>(string path) where T : class, IKeyedTestData, new()
		{
			return ReadTestData<T, ConcurrentDictionary<int, T>>(path, (items, collection) => {
				items.ForEach(x =>
				{
					collection.AddOrUpdate(x.Id, (key) => x, (key, value) => x);
				});
			});			
		}
		#endregion

		#region Helper member methods
		private Y ReadTestData<T, Y>(string path, Action<List<T>, Y> addToCollection) 
			where T : class, new()
			where Y : ICollection
				
		{
			var collection = Activator.CreateInstance<Y>();
			if (string.IsNullOrWhiteSpace(path))
			{
				return collection;
			}
			try
			{
				var directoryInfo = new DirectoryInfo(path);
				if (directoryInfo.Exists)
				{
					var filter = string.Format("{0}*.json", typeof(T).Name);
					foreach (var file in directoryInfo.GetFiles(filter, SearchOption.AllDirectories))
					{
						if (string.IsNullOrWhiteSpace(file?.FullName) || !File.Exists(file.FullName))
							continue;

						var items = ReadJsonFile<T>(file.FullName);
						addToCollection(items, collection);
					}
				}

			}
			catch (Exception e)
			{
				_logger.LogError(e, "Can't read test data for <{0}> from directory: '{1}'", typeof(T).Name, path);
			}
			return collection;
		}

		private List<T> ReadJsonFile<T>(string filePath) where T: class, new()
		{
			List<T> items = new List<T>();
			var json = "";
			try
			{
				//normally should be a bit more sophisticated - here I just want to quickly read the file
				using (var reader = new StreamReader(filePath))
				{
					json = reader.ReadToEnd();
				}
				items = JsonConvert.DeserializeObject<List<T>>(json);
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Can't parse <{0}> from json file: '{1}'", typeof(T).Name, filePath);
			}
			return items;
		}
		private bool SaveJsonFile<T>(IEnumerable<T> items, string filePath) where T : class, new()
		{
			try
			{
				var fileContent = JsonConvert.SerializeObject(items);
				//normally should be a bit more sophisticated - here I just want to quickly dump to the file
				using (var writer = new StreamWriter(filePath))
				{
					writer.Write(fileContent);
				}
				return true;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Can't save <{0}> collection to json file: '{1}'", typeof(T).Name, filePath);
				return false;
			}
		}
		#endregion
	}
}
