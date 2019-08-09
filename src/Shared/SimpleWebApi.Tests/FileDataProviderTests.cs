using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SimpleWebApi.Core.Data;
using SimpleWebApi.Core.Extenstions;
using SimpleWebApi.Tests.TestData;

namespace Core
{
	public class FileDataProviderTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_logger = new Mock<ILogger<FileDataProvider>>();
		}
		
		private Mock<ILogger<FileDataProvider>> _logger;
		private readonly string _dataDir = "TestData";

		[Test]
		public void ReadTestData_WithNoDirectoryProvided_ReturnsEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			
			var result = fileDataProvider.ReadTestData<TestPoco>(null);

			result.Should().BeEmpty();
		}
		[Test]
		public void ReadTestData_WithNoDirectoryFound_ReturnsEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = @"c:\non_existing_directory";

			var result = fileDataProvider.ReadTestData<TestPoco>(directory);

			result.Should().BeEmpty();
		}
		[Test]
		public void ReadTestData_WithNoFilesFound_ReturnsEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), _dataDir);

			var result = fileDataProvider.ReadTestData<NoDataPoco>(directory);

			result.Should().BeEmpty();
		}
		[Test]
		public void ReadTestData_WithEmptyFile_ReturnsEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), _dataDir);

			var result = fileDataProvider.ReadTestData<MyEmptyTest>(directory);

			result.Should().BeEmpty();
		}
		[Test]
		public void ReadTestData_WithExampleDataFile_ReturnsNonEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), _dataDir);

			var result = fileDataProvider.ReadTestData<TestPoco>(directory);

			result.Should().NotBeEmpty();
		}
		[Test]
		public void ReadKeyedTestData_WithExampleDataFile_ReturnsNonEmptyCollection()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), _dataDir);

			var result = fileDataProvider.ReadKeyedTestData<TestKeyedPoco>(directory);

			result.Should().NotBeEmpty();
		}
		[Test]
		public void ReadKeyedTestData_WithExampleDataFileWithDuplicates_DoesNotThrow()
		{
			var fileDataProvider = new FileDataProvider(_logger.Object);
			var directory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectory(), _dataDir);

			Assert.DoesNotThrow(() => fileDataProvider.ReadKeyedTestData<DuplicatePoco>(directory));
		}
		
	}
}