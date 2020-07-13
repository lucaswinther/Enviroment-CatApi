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
        // Comandos base para serem concatenados
        const string queryBase =
            @"SELECT BreedsId, Name, Origin, Temperament, Description
              FROM breeds";

        readonly TheCatDBContext theCatContext;
        readonly IImageUrlRepositories imageUrlRepository;

        /// <summary>
        /// Construtor da classe: Espera um DBContext responsável por acessar a base e que implementa os
        /// comandos de banco de dados
        /// </summary>
        /// <param name="theCatContext"></param>
        public BreedsRepository(TheCatDBContext theCatContext, IImageUrlRepositories imageUrlRepository)
        {
            this.theCatContext = theCatContext;
            this.imageUrlRepository = imageUrlRepository;
        }

        /// <summary>
        /// Método traz todas as as informações da tabela Breeds
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método traz a informação da tabela Breeds conforme o Id informado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método traz todas as informações da tabela Breed que contenham o Temperamento passado
        /// </summary>
        /// <param name="temperament"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método traz todas as informações da tabela Breed que contenham a Origem passada
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método adiciona um registro na tabela Breeds, caso o objeto breeds passado seja válido
        /// </summary>
        /// <param name="breeds"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método atualiza um registro na tabela Breeds, caso o objeto breeds passado seja válido
        /// </summary>
        /// <param name="breeds"></param>
        /// <returns></returns>
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
