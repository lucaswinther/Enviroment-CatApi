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

        /// <summary>
        /// Construtor: Recebe como parâmetro AppSettings que irá conter a string de conexão
        /// Caso queria mudar o banco de dados, basta vir nesta classe e ajustar para o novo
        /// </summary>
        /// <param name="options"></param>
        public TheCatDBContext(IAppConfiguration appConfiguration)
        {
            this.appSettings = appConfiguration.GetAppSettings();
        }

        /// <summary>
        /// Cria uma conexão com a base de dados
        /// </summary>
        public DbConnection GetConnection => new SqlConnection(appSettings.ConnectionString);
    }
}
