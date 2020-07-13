using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatWebApi.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatsBreedsController : ControllerBase
	{

		readonly Stopwatch stopWatch;
		readonly ICatBreedsRepositories breedsRepository;
		readonly ILogger<CatsBreedsController> logger;
		readonly IWebHostEnvironment env;

		public CatsBreedsController(ICatBreedsRepositories breedsRepository, ILogger<CatsBreedsController> logger, IWebHostEnvironment env)
		{
			stopWatch = new Stopwatch();
			this.breedsRepository = breedsRepository;
			this.logger = logger;
			this.env = env;
		}

		[HttpGet]
		[Route("getallbreeds")]
		public async Task<IActionResult> GetAllBreeds()
		{
			try
			{
				stopWatch.Restart();
				stopWatch.Start();
				var result = await breedsRepository.GetAllBreeds(true);
				stopWatch.Stop();
				logger.LogInformation((int)LogLevel.Information, $"Encontrados {result.Count} raças;{stopWatch.ElapsedMilliseconds}");
				if (env.IsDevelopment())
					logger.LogDebug((int)LogLevel.Debug, $"Encontrados {result.Count} raças;{stopWatch.ElapsedMilliseconds}");
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogError((int)LogLevel.Error, $"Erro ao buscar raças: {ex.Message}");
				return BadRequest();
			}
		}

	}
}
