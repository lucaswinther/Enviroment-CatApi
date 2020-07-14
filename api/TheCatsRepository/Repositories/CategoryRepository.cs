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
        const string queryBase =
            @"SELECT CategoryId, Name
              FROM category";

        readonly TheCatDBContext theCatContext;

        public CategoryRepository(TheCatDBContext theCatContext)
        {
            this.theCatContext = theCatContext;
        }
        public async Task<ICollection<Category>> GetAllCategory()
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Category>(queryBase);
                return result.ToList();
            }
        }
        public async Task<Category> GetCategory(int id)
        {
            using (var conn = theCatContext.GetConnection)
            {
                var result = await conn.QueryAsync<Category>($"{queryBase} WHERE CategoryId = {id}");
                return result.FirstOrDefault();
            }
        }
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
