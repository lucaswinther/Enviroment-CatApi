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
            var breedsList = await theCatAPI.GetBreeds();
            foreach (var breeds in breedsList)
            {
                var breedsInDB = await breedsRepository.GetBreeds(breeds.Id);
                if (breedsInDB == null)
                {
                    breedsInDB = new Breeds(breeds.Id, breeds.Name);
                    breedsInDB.SetOrigin(breeds.Origin);
                    breedsInDB.SetTemperament(breeds.Temperament);
                    breedsInDB.SetDescription(breeds.Description);
                    await breedsRepository.AddBreeds(breedsInDB);
                }
                var imagesList = await theCatAPI.GetImagesByBreeds(breeds.Id);
                if (imagesList != null && imagesList.Count > 0)
                {
                    foreach (var image in imagesList)
                    {
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
            var categoryList = await theCatAPI.GetCategories();
            categoryList = categoryList.Where(x => appSettings.TheCatSettings.ImageCategoryFilter.Contains(x.Name)).ToList();
            foreach (var category in categoryList)
            {
                var categoryInDB = await categoryRepository.GetCategory(category.Id);
                if (categoryInDB == null)
                {
                    categoryInDB = new Category(category.Id, category.Name);
                    await categoryRepository.AddCategory(categoryInDB);
                }
                var imagesList = await theCatAPI.GetImagesByCategory(category.Id, 3);
                if (imagesList != null && imagesList.Count > 0)
                {
                    foreach (var image in imagesList)
                    {
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
