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
    public class CategoryRepository : ICategoryRepository
    {
        // Comandos base para serem concatenados
        const string queryBase =
            @"SELECT CategoryId, Name
              FROM category";

        readonly TheCatDBContext theCatContext;

        /// <summary>
        /// Construtor da classe: Espera um DBContext responsável por acessar a base e que implementa os
        /// comandos de banco de dados
        /// </summary>
        /// <param name="theCatContext"></param>
        public CategoryRepository(TheCatDBContext theCatContext)
        {
            this.theCatContext = theCatContext;
        }

        /// <summary>
        /// Método traz todas as as informações da tabela Category
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Category>> GetAllCategory()
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Category>(queryBase);
                return result.ToList();
            }
        }

        /// <summary>
        /// Método traz a informação da tabela Category conforme o Id informado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetCategory(int id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Category>($"{queryBase} WHERE CategoryId = {id}");
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Método adiciona um registro na tabela Category, caso o objeto category passado seja válido
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task AddCategory(Category category)
        {
            if (!category.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"INSERT INTO category 
                        (CategoryId, Name) 
                      VALUES
                        (@CategoryId, @Name)";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, category);
                }
            }
        }

        /// <summary>
        /// Método atualiza um registro na tabela Category, caso o objeto category passado seja válido
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task UpdateCategory(Category category)
        {
            if (!category.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"UPDATE category SET 
                        Name = @Name
                    WHERE CategoryId = @CategoryId";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, category);
                }
            }
        }
    }
}
