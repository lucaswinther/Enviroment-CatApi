using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCatsDomain.Entities;
using TheCatsDomain.Interfaces.Repository;
using TheCatsRepository.Context;

namespace TheCatsRepository.Repositories
{
    public class BreedsRepository : ICatBreedsRepositories
    {
        const string queryBase =
            @"SELECT BreedsId, Name, Origin, Temperament, Description
              FROM breeds";

        readonly TheCatDBContext theCatContext;
        readonly IImageUrlRepositories imageUrlRepository;

        public BreedsRepository(TheCatDBContext theCatContext, IImageUrlRepositories imageUrlRepository)
        {
            this.theCatContext = theCatContext;
            this.imageUrlRepository = imageUrlRepository;
        }

        public async Task<ICollection<Breeds>> GetAllBreeds(bool includeImages = false)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Breeds>(queryBase);
                if (includeImages)
                    await FillImageInBreedsObject(result.ToList());
                return result.ToList();
            }
        }
        public async Task<Breeds> GetBreeds(string idOrName, bool includeImages = false)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Breeds>($"{queryBase} WHERE BreedsId = '{idOrName}' OR Name like '%{idOrName}%'");
                if (includeImages)
                    await FillImageInBreedsObject(result.FirstOrDefault());
                return result.FirstOrDefault();
            }
        }
        public async Task<ICollection<Breeds>> GetBreedsByTemperament(string temperament, bool includeImages = false)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Breeds>($"{queryBase} WHERE Temperament like '%{temperament}%'");
                if (includeImages)
                    await FillImageInBreedsObject(result.ToList());
                return result.ToList();
            }
        }
        public async Task<ICollection<Breeds>> GetBreedsByOrigin(string origin, bool includeImages = false)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Breeds>($"{queryBase} WHERE Origin like '%{origin}%'");
                if (includeImages)
                    await FillImageInBreedsObject(result.ToList());
                return result.ToList();
            }
        }
        public async Task AddBreeds(Breeds breeds)
        {
            if (!breeds.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"INSERT INTO breeds 
                        (BreedsId, Name, Origin, Temperament, Description) 
                      VALUES
                        (@BreedsId, @Name, @Origin, @Temperament, @Description)";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, breeds);
                }
            }
        }
        public async Task UpdateBreeds(Breeds breeds)
        {
            if (!breeds.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"UPDATE breeds SET 
                        Name = @Name
                        , Origin = @Origin
                        , Temperament = @Temperament
                        , Description = @Description
                    WHERE BreedsId = @BreedsId";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, breeds);
                }
            }
        }
        async Task FillImageInBreedsObject(ICollection<Breeds> listBreeds)
        {
            foreach (var breeds in listBreeds)
                await FillImageInBreedsObject(breeds);
        }
        async Task FillImageInBreedsObject(Breeds breeds)
        {
            var result = await imageUrlRepository.GetImageUrlByBreeds(breeds.BreedsId);
            breeds.Images = result.Take(3).ToList();
        }
    }
}
