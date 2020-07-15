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
		

		public CatsBreedsController(ICatBreedsRepositories breedsRepository, IImageUrlRepositories imageUrlRepositories, ICommandCapture commandCapture, ILogger<CatsBreedsController> logger)
		{
			stopWatch = new Stopwatch();
			this.breedsRepository = breedsRepository;
            this.imageUrlRepositories = imageUrlRepositories;
            this.commandCapture = commandCapture;
			this.logger = logger;			
		}

        /// <summary>
        /// Endpoint to load cats database
        /// </summary>
        /// <returns></returns>
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
                logger.LogInformation((int)LogLevel.Information, $"Finish Load Database ;{stopWatch.ElapsedMilliseconds} ms");
                
                logger.LogInformation((int)LogLevel.Information, $"Starting Load Images By Category ;{stopWatch.ElapsedMilliseconds} ms");
                await commandCapture.CaptureImagesByCategory();                
                logger.LogInformation((int)LogLevel.Information, $"Finish Load Images By Category ;{stopWatch.ElapsedMilliseconds} ms");
                stopWatch.Stop();


                return Ok($"Finish load data in {stopWatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Fail to Load Data: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Endpoint to get all breeads in database
        /// </summary>
        /// <returns></returns>
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
				logger.LogInformation((int)LogLevel.Information, $"Find a total of {result.Count} breeds in {stopWatch.ElapsedMilliseconds} ms");
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogError((int)LogLevel.Error, $"Fail to get all breeds: {ex.Message}");
				return BadRequest();
			}
		}

        /// <summary>
        /// Endpoint to get a specific Breed using Id or name
        /// </summary>
        /// <param name="IdOrName"></param>
        /// <returns></returns>
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
                var msg = result != null ? $"Breed {result?.Name} found" : $"Breed search for: {IdOrName} not found";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Fail to get breed {IdOrName}: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Endpoint to get a specific Breed using temperament
        /// </summary>
        /// <param name="temperament"></param>
        /// <returns></returns>
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
                var msg = result != null ? $"Find a total of {result.Count} breeds for temperament {temperament}" : $"Breeds searched for temperament: {temperament} not found";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Error to get breeads using temperament {temperament}: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Endpoint to get a specific image using a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
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
                var msg = result != null ? $"Finds {result.Count} breeds for {category}" : $"Breeds for category: {category} not found";
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Error to get Images using category {category}: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Endpoint to get a specific image using a origin
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
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
                var msg = result != null ? $"Find a total of {result.Count} breeds for origin {origin}" : $"Breeds searched for origin: {origin} not found"; 
                logger.LogInformation((int)LogLevel.Information, $"{msg};{stopWatch.ElapsedMilliseconds}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogLevel.Error, $"Error to get breeads using origin {origin}: {ex.Message}");
                return BadRequest();
            }
        }

    }
}
