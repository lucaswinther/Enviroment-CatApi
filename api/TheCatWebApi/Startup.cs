using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheCatsApplication;
using TheCatsDomain.Integration;
using TheCatsDomain.Integration.Service;
using TheCatsDomain.Interfaces;
using TheCatsDomain.Interfaces.Application;
using TheCatsDomain.Interfaces.Repository;
using TheCatsDomain.Models;
using TheCatsRepository.Context;
using TheCatsRepository.Repositories;

namespace TheCatWebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			// Injeção de Dependência
			services.AddScoped<IAppConfiguration, AppConfiguration>();
			services.AddScoped<TheCatDBContext>();
			services.AddScoped<ICatBreedsRepositories, BreedsRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IImageUrlRepositories, ImageUrlRepository>();
			services.AddScoped<ILogEventRepository, LogEventRepository>();
			services.AddScoped<ITheCatWebAPI, TheCatWebAPIService>();
			services.AddScoped<ICommandCapture, CommandCapture>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
