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
        // Comandos base para serem concatenados
        const string queryBase =
            @"SELECT img.ImageUrlId, img.Url, img.Width, img.Height
              FROM imageurl img";

        readonly TheCatDBContext theCatContext;

        /// <summary>
        /// Construtor da classe: Espera um DBContext responsável por acessar a base e que implementa os
        /// comandos de banco de dados
        /// </summary>
        /// <param name="theCatContext"></param>
        public ImageUrlRepository(TheCatDBContext theCatContext)
        {
            this.theCatContext = theCatContext;
        }

        /// <summary>
        /// Método traz todas as as informações da tabela ImageUrl
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<ImageUrl>> GetAllImageUrl()
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<ImageUrl>(queryBase);
                return result.ToList();
            }
        }

        /// <summary>
        /// Método traz a informação da tabela ImageUrl conforme o Id informado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ImageUrl> GetImageUrl(string id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<ImageUrl>($"{queryBase} WHERE img.ImageUrlId = '{id}'");
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Método traz a informação da tabela ImageUrl através de CateogryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICollection<ImageUrl>> GetImageUrlByCategory(int id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var qryJoin = string.Concat(queryBase, " JOIN ImageUrlCategory imc on img.ImageUrlId = imc.ImageUrlId");
                var result = await conn.QueryAsync<ImageUrl>($"{qryJoin} WHERE imc.CategoryId = {id}");
                return result.ToList();
            }
        }

        /// <summary>
        /// Método traz a informação da tabela ImageUrl através de BreedsId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICollection<ImageUrl>> GetImageUrlByBreeds(string id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var qryJoin = string.Concat(queryBase, " JOIN ImageUrlBreeds imb on img.ImageUrlId = imb.ImageUrlId");
                var result = await conn.QueryAsync<ImageUrl>($"{qryJoin} WHERE imb.BreedsId = '{id}'");
                return result.ToList();
            }
        }

        /// <summary>
        /// Método adiciona um registro na tabela ImageUrl, caso o objeto imageUrl passado seja válido
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método atualiza um registro na tabela ImageUrl, caso o objeto imageUrl passado seja válido
        /// Também verifica se os objetos Breeds e Category estão relacionados e ignora o status de atualização deles
        /// para que o ORM não tente inserir novamente registros já existentes
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Verifica se a ImageUrl está associada com Breeds e/ou Category e
        /// caso sim, associa nas suas respectivas tabelas
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        async Task SaveImageUrlWithAssociation(ImageUrl imageUrl)
        {
            if (imageUrl.Breeds != null)
                await AssociateImageUrlToBreeds(imageUrl);
            if (imageUrl.Category != null)
                await AssociateImageUrlToCategory(imageUrl);
        }

        /// <summary>
        /// Associa ImageUrl com Breeds. Antes verifica se já existe associação, caso não,
        /// insere na tabela de associação
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Associa ImageUrl com Category. Antes verifica se já existe associação, caso não,
        /// insere na tabela de associação
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
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
