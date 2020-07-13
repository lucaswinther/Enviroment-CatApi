using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Entities;

namespace TheCatsDomain.Interfaces.Repository
{
    public interface IImageUrlRepositories
    {
        Task<ICollection<ImageUrl>> GetAllImageUrl();
        Task<ImageUrl> GetImageUrl(string id);
        Task<ICollection<ImageUrl>> GetImageUrlByCategory(int id);
        Task<ICollection<ImageUrl>> GetImageUrlByBreeds(string id);
        Task AddImageUrl(ImageUrl imageUrl);
        Task UpdateImageUrl(ImageUrl imageUrl);
    }
}
