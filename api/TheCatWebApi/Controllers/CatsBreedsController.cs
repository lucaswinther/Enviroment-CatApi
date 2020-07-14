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
using TheCatsDomain.Interfaces.Application;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatWebApi.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatsBreedsController : ControllerBase
	{

		readonly Stopwatch stopWatch;
		readonly ICatBreedsRepositories breedsRepository;
        readonly IImageUrlRepositories imageUrlRepositories;
        readonly ICommandCapture commandCapture;
        readonly ILogger<CatsBreedsController> logger;
		readonly IWebHostEnvironment env;

		public CatsBreedsController(ICatBreedsRepositories breedsRepository, IImageUrlRepositories imageUrlRepositories, ICommandCapture commandCapture, ILogger<CatsBreedsController> logger, IWebHostEnvironment env)
		{
			stopWatch = new Stopwatch();
			this.breedsRepository = breedsRepository;
            this.imageUrlRepositories = imageUrlRepositories;
            this.commandCapture = commandCapture;
			this.logger = logger;
			this.env = env;
		}

        [HttpGet]
        [Route("loadcats")]
        public async Task<IActionResult> LoadCats()
        {
            try
            {
                stopWatch.Restart();
                stopWatch.Start();
                logger.LogInformation((int)LogLevel.Information, $"Starting Load Database ;{stopWatch.ElapsedMilliseconds} ms");
                await commandCapture.CapureAllBreedsWithImages();
                stopWatch.Stop();
                logger.LogInformation((int)LogLevel.Information, $"Finish Load Database ;{stopWatch.ElapsedMilliseconds} ms");

                stopWatch.Start();
                logger.LogInformation((int)LogLevel.Information, $"Starting Load Images By Category ;{stopWatch.ElapsedMilliseconds} ms");
                await commandCapture.CaptureImagesByCategory();
                stopWatch.Stop();
                logger.LogInformation((int)LogLevel.Information, $"Finish Load Images By Category ;{stopWatch.ElapsedMilliseconds} ms");
                

                return Ok($"Finish load data");
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Fail to Load Data: {ex.Message}");
                return BadRequest();
            }
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

        [HttpGet]
        [Route("getbreeds")]
        public async Task<IActionResult> GetBreeds(string IdOrName)
        {
            try
            {
                stopWatch.Restart();
                stopWatch.Start();
                var result = await breedsRepository.GetBreeds(IdOrName, true);
                stopWatch.Stop();
                var msg = result != null ? $"Raça {result?.Name} encontrada" : $"Raça pesquisada por: {IdOrName} não encontrada";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                if (env.IsDevelopment())
                    logger.LogDebug((int)LogLevel.Debug, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Erro ao buscar raça por {IdOrName}: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getbreedsbytemperament")]
        public async Task<IActionResult> GetBreedsByTemperament(string temperament)
        {
            try
            {
                stopWatch.Restart();
                stopWatch.Start();
                var result = await breedsRepository.GetBreedsByTemperament(temperament, true);
                stopWatch.Stop();
                var msg = result != null ? $"Encontrados {result.Count} raça(s) para temperamento {temperament}" : $"Raças pesquisadas por temperamento: {temperament} não encontradas";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                if (env.IsDevelopment())
                    logger.LogDebug((int)LogLevel.Debug, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Erro ao buscar raça por temperamento {temperament}: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getimageurlbycategory")]
        public async Task<IActionResult> getimageurlbycategory(int category)
        {
            try
            {
                stopWatch.Restart();
                stopWatch.Start();
                var result = await imageUrlRepositories.GetImageUrlByCategory(category);
                stopWatch.Stop();
                var msg = result != null ? $"Encontrados {result.Count} raça(s) para temperamento {category}" : $"Raças pesquisadas por temperamento: {category} não encontradas";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                if (env.IsDevelopment())
                    logger.LogDebug((int)LogLevel.Debug, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Erro ao buscar raça por temperamento {category}: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getbreedsbyorigin")]
        public async Task<IActionResult> GetBreedsByOrigin(string origin)
        {
            try
            {
                stopWatch.Restart();
                stopWatch.Start();
                var result = await breedsRepository.GetBreedsByOrigin(origin, true);
                stopWatch.Stop();
                var msg = result != null ? $"Encontrados {result.Count} raça(s) para origem {origin}" : $"Raças pesquisadas por origem: {origin} não encontradas";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                if (env.IsDevelopment())
                    logger.LogDebug((int)LogLevel.Debug, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Erro ao buscar raça por origem {origin}: {ex.Message}");
                return BadRequest();
            }
        }

    }
}
