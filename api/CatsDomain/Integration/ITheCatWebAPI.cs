using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Models;

namespace TheCatsDomain.Integration
{
	public interface ITheCatWebAPI
	{
		Task<ICollection<BreedsSearchResponse>> GetBreeds();
		Task<ICollection<CategorySearchResponse>> GetCategories();
		Task<ICollection<ImageSearchResponse>> GetImagesByCategory(int categoryId, int limitImages = 4);
		Task<ICollection<ImageSearchResponse>> GetImagesByBreeds(string breedsId, int limitImages = 3);
	}
}
