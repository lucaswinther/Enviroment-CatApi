using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Entities;

namespace TheCatsDomain.Interfaces.Repository
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllCategory();
        Task<Category> GetCategory(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
    }
}
