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

        /// <summary>
        /// Construtor recebe o objeto AppSettings para que possa ter as informações
        /// da URL e métodos. Estas informações veem do arquivo AppSettings.json
        /// </summary>
        /// <param name="appSettings"></param>
        public TheCatWebAPIService(IAppConfiguration appConfiguration)
        {
            this.appSettings = appConfiguration.GetAppSettings();
        }

        /// <summary>
        /// Consulta todas as informações de Breeds na API
        /// Se o retorno da API não for vazio, serializa uma lista de objetos BreedsSearchResponse
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Consulta todas as informações de Category na API
        /// Se o retorno da API não for vazio, serializa uma lista de objetos CategorySearchResponse
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Consulta todas as informações de Images da API pelo Id Category passado
        /// O segundo parâmetro limita o número de registros ue serão retornados
        /// Se o retorno da API não for vazio, serializa uma lista de objetos ImageSearchResponse
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="limitImages"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Consulta todas as informações de Images da API pelo Id Breeds passado
        /// O segundo parâmetro limita o número de registros ue serão retornados
        /// Se o retorno da API não for vazio, serializa uma lista de objetos ImageSearchResponse
        /// </summary>
        /// <param name="breedsId"></param>
        /// <param name="limitImages"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método auxiliar, que realiza a chamada http para a API e retorna o conteúdo no formato string
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
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
