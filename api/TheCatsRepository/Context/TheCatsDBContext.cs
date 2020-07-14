using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using TheCatsDomain.Interfaces;
using TheCatsDomain.Models;

namespace TheCatsRepository.Context
{
    public class TheCatDBContext
    {
        readonly AppSettings appSettings;

        public TheCatDBContext(IAppConfiguration appConfiguration)
        {
            this.appSettings = appConfiguration.GetAppSettings();
        }
        public DbConnection GetConnection => new SqlConnection(appSettings.ConnectionString);
    }
}
