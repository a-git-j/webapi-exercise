using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SimpleWebApi.Core.Web.Logging;

namespace TransactionsAPI
{
	public class Program
	{
		private static IConfiguration _configuration;

		public static void Main(string[] args)
		{
			//initilize settings to read configuration NLog setup
			_configuration = LoggersConfiguration.InitializeConfigurationFromSettingsFile();
			LoggersConfiguration.InitializeNLogConfigurationFromSettingsFile(_configuration);

			//initialize logger first - so that app start can be logged, including all config erroros during building the WebHost
			var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
			try
			{
				logger.Debug("------------------------");
				logger.Info("--Starting application--");
				CreateWebHostBuilder(args).Build().Run();
			}
			catch (Exception e)
			{
				logger.Error(e, "--Stopped application due to unhandled exception occurred!--");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit
				NLog.LogManager.Shutdown();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
				})
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseKestrel(x => x.AddServerHeader = false)
				//.UseIISIntegration()
				.UseStartup<Startup>()
				.UseNLog(); //NLog's DI
	}
}
