using System.Linq;
using System;
using System.Threading.Tasks;
using TheCatsDomain.Entities;
using TheCatsDomain.Integration;
using TheCatsDomain.Interfaces;
using TheCatsDomain.Interfaces.Application;
using TheCatsDomain.Interfaces.Repository;
using TheCatsDomain.Models;
using TheCatsRepository.Repositories;
using System.Collections.Generic;

namespace TheCatsApplication
{
    public class CommandCapture : ICommandCapture
    {
        readonly ITheCatWebAPI theCatAPI;
        readonly ICatBreedsRepositories breedsRepository;
        readonly ICategoryRepository categoryRepository;
        readonly IImageUrlRepositories imageUrlRepository;        
        readonly AppSettings appSettings;
        public CommandCapture(IAppConfiguration appConfiguration, ITheCatWebAPI theCatAPI, ICatBreedsRepositories breedsRepository, ICategoryRepository categoryRepository, IImageUrlRepositories imageUrlRepository)
        {
            this.appSettings = appConfiguration.GetAppSettings();
            this.theCatAPI = theCatAPI;
            this.breedsRepository = breedsRepository;
            this.imageUrlRepository = imageUrlRepository;
        }
        public async Task CapureAllBreedsWithImages()
        {
            // Captura uma lista de Breeds a partir da API TheCatAPI
            var breedsList = await theCatAPI.GetBreeds();
            // Para cada Breeds encontrado, armazena na base de dados
            foreach (var breeds in breedsList)
            {
                // Verifica se Breeds já existe na base de dados, se não existir, armazena
                var breedsInDB = await breedsRepository.GetBreeds(breeds.Id);
                if (breedsInDB == null)
                {
                    breedsInDB = new Breeds(breeds.Id, breeds.Name);
                    breedsInDB.SetOrigin(breeds.Origin);
                    breedsInDB.SetTemperament(breeds.Temperament);
                    breedsInDB.SetDescription(breeds.Description);
                    await breedsRepository.AddBreeds(breedsInDB);
                }
                // Encontra as imagens do registro Breeds atual, caso exista, armazena as imagens na base de dados
                var imagesList = await theCatAPI.GetImagesByBreeds(breeds.Id);
                if (imagesList != null && imagesList.Count > 0)
                {
                    foreach (var image in imagesList)
                    {
                        // Verifica se Image existe. Se não, cria o objeto
                        // A variável: imageExists, é para fazer o tratamento se Add ou Update
                        // pois chamando este métodos, a tabela associativa será gravada, caso
                        // a imagem não tenha existido anteriormente.
                        var imageInDB = await imageUrlRepository.GetImageUrl(image.Id);
                        var imageExists = imageInDB != null;
                        if (!imageExists)
                        {
                            imageInDB = new ImageUrl(image.Id, image.Url);
                            imageInDB.SetWidth(image.Width);
                            imageInDB.SetHeight(image.Height);
                        }
                        imageInDB.SetBreeds(breedsInDB);
                        if (!imageExists)
                            await imageUrlRepository.AddImageUrl(imageInDB);
                        else
                            await imageUrlRepository.UpdateImageUrl(imageInDB);
                    }
                }
            }
        }
        public async Task CaptureImagesByCategory()
        {
            // Captura uma lista de Category da API TheCatAPI, em seguida filtra a lista
            // conforme os nomes definidos no AppSettings
            var categoryList = await theCatAPI.GetCategories();
            categoryList = categoryList.Where(x => appSettings.TheCatSettings.ImageCategoryFilter.Contains(x.Name)).ToList();
            // Para cada Category encontrado, armazena na base de dados
            foreach (var category in categoryList)
            {
                // Verifica se Category já existe na base de dados, se não existir, armazena
                var categoryInDB = await categoryRepository.GetCategory(category.Id);
                if (categoryInDB == null)
                {
                    categoryInDB = new Category(category.Id, category.Name);
                    await categoryRepository.AddCategory(categoryInDB);
                }
                // Econtra as imagens do registro Breeds atual, caso exista, armazena as imagens na base de dados
                var imagesList = await theCatAPI.GetImagesByCategory(category.Id, 3);
                if (imagesList != null && imagesList.Count > 0)
                {
                    foreach (var image in imagesList)
                    {
                        // Verifica se Image existe. Se não, cria o objeto
                        // A variável: imageExists, é para fazer o tratamento se Add ou Update
                        // pois chamando este métodos, a tabela associativa será gravada, caso
                        // a imagem não tenha existido anteriormente.
                        var imageInDB = await imageUrlRepository.GetImageUrl(image.Id);
                        var imageExists = imageInDB != null;
                        if (!imageExists)
                        {
                            imageInDB = new ImageUrl(image.Id, image.Url);
                            imageInDB.SetWidth(image.Width);
                            imageInDB.SetHeight(image.Height);
                        }
                        imageInDB.SetCategory(categoryInDB);
                        if (!imageExists)
                            await imageUrlRepository.AddImageUrl(imageInDB);
                        else
                            await imageUrlRepository.UpdateImageUrl(imageInDB);
                    }
                }
            }
        }


    }
}
