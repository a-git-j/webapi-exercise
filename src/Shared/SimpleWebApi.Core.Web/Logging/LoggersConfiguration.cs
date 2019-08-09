using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using NLog;

namespace SimpleWebApi.Core.Web.Logging
{
	public static class LoggersConfiguration
	{
		public static IConfiguration InitializeConfigurationFromSettingsFile()
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configFile = "appsettings.json";
			if (!string.IsNullOrWhiteSpace(env)) configFile = $"appsettings.{env}.json";
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(configFile, optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();		
		}
	
		public static void InitializeNLogConfigurationFromSettingsFile(IConfiguration configuration)
		{
			LogManager.Configuration = new NLog.Extensions.Logging.NLogLoggingConfiguration(configuration.GetSection("NLog"));
		}
	}
}
