using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Entities;
using TheCatsDomain.Interfaces.Repository;
using TheCatsRepository.Context;

namespace TheCatsRepository.Repositories
{
    public class ImageUrlRepository : IImageUrlRepositories
    {
        const string queryBase =
            @"SELECT img.ImageUrlId, img.Url, img.Width, img.Height
              FROM imageurl img";

        readonly TheCatDBContext theCatContext;

        public ImageUrlRepository(TheCatDBContext theCatContext)
        {
            this.theCatContext = theCatContext;
        }
        public async Task<ICollection<ImageUrl>> GetAllImageUrl()
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<ImageUrl>(queryBase);
                return result.ToList();
            }
        }
        public async Task<ImageUrl> GetImageUrl(string id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<ImageUrl>($"{queryBase} WHERE img.ImageUrlId = '{id}'");
                return result.FirstOrDefault();
            }
        }
        public async Task<ICollection<ImageUrl>> GetImageUrlByCategory(int id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var qryJoin = string.Concat(queryBase, " JOIN ImageUrlCategory imc on img.ImageUrlId = imc.ImageUrlId");
                var result = await conn.QueryAsync<ImageUrl>($"{qryJoin} WHERE imc.CategoryId = {id}");
                return result.ToList();
            }
        }
        public async Task<ICollection<ImageUrl>> GetImageUrlByBreeds(string id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var qryJoin = string.Concat(queryBase, " JOIN ImageUrlBreeds imb on img.ImageUrlId = imb.ImageUrlId");
                var result = await conn.QueryAsync<ImageUrl>($"{qryJoin} WHERE imb.BreedsId = '{id}'");
                return result.ToList();
            }
        }
        public async Task AddImageUrl(ImageUrl imageUrl)
        {
            if (!imageUrl.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"INSERT INTO imageurl 
                        (ImageUrlId, Url, Width, Height) 
                      VALUES
                        (@ImageUrlId, @Url, @Width, @Height)";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, imageUrl);
                }
                await SaveImageUrlWithAssociation(imageUrl);
            }
        }
        public async Task UpdateImageUrl(ImageUrl imageUrl)
        {
            if (!imageUrl.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"UPDATE imageurl SET 
                        Url = @Url
                        , Width = @Width
                        , Height = @Height
                    WHERE ImageUrlId = @ImageUrlId";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, imageUrl);
                }
                await SaveImageUrlWithAssociation(imageUrl);
            }
        }
        async Task SaveImageUrlWithAssociation(ImageUrl imageUrl)
        {
            if (imageUrl.Breeds != null)
                await AssociateImageUrlToBreeds(imageUrl);
            if (imageUrl.Category != null)
                await AssociateImageUrlToCategory(imageUrl);
        }
        async Task AssociateImageUrlToBreeds(ImageUrl imageUrl)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryFirstAsync<int>(
                    @$"SELECT count(*) as nrec
                       FROM ImageUrlBreeds
                       WHERE ImageUrlId = '{imageUrl.ImageUrlId}'
                         AND BreedsId = '{imageUrl.Breeds.BreedsId}'"
                );
                if (result == 0)
                {
                    await conn.ExecuteAsync(
                        $@"INSERT INTO ImageUrlBreeds (ImageUrlId, BreedsId)
                           VALUES ('{imageUrl.ImageUrlId}', '{imageUrl.Breeds.BreedsId}')"
                    );
                }
            }
        }
        async Task AssociateImageUrlToCategory(ImageUrl imageUrl)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryFirstAsync<int>(
                    @$"SELECT count(*) as nrec
                       FROM ImageUrlCategory
                       WHERE ImageUrlId = '{imageUrl.ImageUrlId}'
                         AND CategoryId = {imageUrl.Category.CategoryId}"
                );
                if (result == 0)
                {
                    await conn.ExecuteAsync(
                        $@"INSERT INTO ImageUrlCategory (ImageUrlId, CategoryId)
                           VALUES ('{imageUrl.ImageUrlId}', {imageUrl.Category.CategoryId})"
                    );
                }
            }
        }
    }
}
