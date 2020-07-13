using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCatsDomain.Entities;

namespace TheCatsDomain.Interfaces.Repository
{
    public interface ICatBreedsRepositories
    {
        Task<ICollection<Breeds>> GetAllBreeds(bool includeImages = false);
        Task<Breeds> GetBreeds(string idOrName, bool includeImages = false);
        Task<ICollection<Breeds>> GetBreedsByTemperament(string temperament, bool includeImages = false);
        Task<ICollection<Breeds>> GetBreedsByOrigin(string origin, bool includeImages = false);
        Task AddBreeds(Breeds breeds);
        Task UpdateBreeds(Breeds breeds);
    }
}
