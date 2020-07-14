using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCatsDomain.Entities;

namespace TheCatsDomain.Interfaces.Repository
{
    public interface IElasticIntegration
    {
        Task CreateIndex();
        Task AddDoc(LogEvent logEvent);
    }
}
