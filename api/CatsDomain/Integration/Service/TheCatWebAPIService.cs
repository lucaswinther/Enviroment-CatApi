using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Interfaces;
using TheCatsDomain.Models;

namespace TheCatsDomain.Integration.Service
{
	public class TheCatWebAPIService : ITheCatWebAPI
	{
		readonly AppSettings appSettings;

		public TheCatWebAPIService(IAppConfiguration appConfiguration)
		{
			this.appSettings = appConfiguration.GetAppSettings();
		}

		public async Task<ICollection<BreedsSearchResponse>> GetBreeds()
		{
			var jsonResult = await GetHttpResponse(appSettings.TheCatSettings.BreedsMethod);
			if (string.IsNullOrEmpty(jsonResult))
				return null;
			else
			{
				var result = JsonConvert.DeserializeObject<ICollection<BreedsSearchResponse>>(jsonResult);
				return result;
			}
		}

		public async Task<ICollection<CategorySearchResponse>> GetCategories()
		{
			var jsonResult = await GetHttpResponse(appSettings.TheCatSettings.CategoryMethod);
			if (string.IsNullOrEmpty(jsonResult))
				return null;
			else
			{
				var result = JsonConvert.DeserializeObject<ICollection<CategorySearchResponse>>(jsonResult);
				return result;
			}
		}

		public async Task<ICollection<ImageSearchResponse>> GetImagesByCategory(int categoryId, int limitImages = 10)
		{
			var jsonResult = await GetHttpResponse($"{appSettings.TheCatSettings.ImageMethod}?category_ids={categoryId}&limit={limitImages}&include_categories=false");
			if (string.IsNullOrEmpty(jsonResult))
				return null;
			else
			{
				var result = JsonConvert.DeserializeObject<ICollection<ImageSearchResponse>>(jsonResult);
				return result;
			}
		}

		public async Task<ICollection<ImageSearchResponse>> GetImagesByBreeds(string breedsId, int limitImages = 10)
		{
			var jsonResult = await GetHttpResponse($"{appSettings.TheCatSettings.ImageMethod}?breeds_id={breedsId}&limit={limitImages}&include_categories=false&include_breeds=false");
			if (string.IsNullOrEmpty(jsonResult))
				return null;
			else
			{
				var result = JsonConvert.DeserializeObject<ICollection<ImageSearchResponse>>(jsonResult);
				return result;
			}
		}

		async Task<string> GetHttpResponse(string method)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(appSettings.TheCatSettings.BaseURL);
				var httpResponse = await httpClient.GetAsync(method);
				if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
					return string.Empty;
				else
				{
					var responseAsString = await httpResponse.Content.ReadAsStringAsync();
					return responseAsString;
				}
			}
		}
	}
}
