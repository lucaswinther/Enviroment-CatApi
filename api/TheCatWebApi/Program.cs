using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheCatsApplication.Log;
using TheCatsDomain.Models;

namespace TheCatWebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			var appConfiguration = new AppConfiguration();
			return Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			}).ConfigureLogging((hostingContext, logging) =>
			{
				logging.AddElasticProvider(new ElasticIntegrationService(appConfiguration));
			});
		}
	}
}
