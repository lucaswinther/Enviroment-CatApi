using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Entities;
using TheCatsDomain.Interfaces;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatsApplication.Log
{
	
public class ElasticIntegrationService : IElasticIntegration
    {
        readonly string baseURL;
        readonly string indexName;
        readonly string docMethod;

        public ElasticIntegrationService(IAppConfiguration appConfiguration)
        {
            var appSettings = appConfiguration.GetAppSettings();
            baseURL = appSettings.ElasticSettings.BaseURL;
            indexName = appSettings.ElasticSettings.IndexName;
            docMethod = appSettings.ElasticSettings.DocMethod;
        }
        public async Task CreateIndex()
        {
            var indexExist = await IndexExist();
            if (indexExist)
                return;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri($"{baseURL}/{indexName}");
                    var contentJson = new StringBuilder();
                    contentJson.Append("{");
                    contentJson.Append("    \"settings\" : {");
                    contentJson.Append("        \"number_of_shards\" : 1");
                    contentJson.Append("    },");
                    contentJson.Append("    \"mappings\" : {");
                    contentJson.Append("        \"properties\" : {");
                    contentJson.Append("            \"LogEventId\" : { \"type\" : \"integer\" },");
                    contentJson.Append("            \"EventDate\" : { \"type\" : \"date\" },");
                    contentJson.Append("            \"EventTypeId\" : { \"type\" : \"integer\" },");
                    contentJson.Append("            \"EventType\" : { \"type\" : \"text\" },");
                    contentJson.Append("            \"MethodName\" : { \"type\" : \"text\" },");
                    contentJson.Append("            \"ExecutionTime\" : { \"type\" : \"long\" },");
                    contentJson.Append("            \"Description\" : { \"type\" : \"text\" }");
                    contentJson.Append("        }");
                    contentJson.Append("    }");
                    contentJson.Append("}");
                    HttpContent content = new StringContent(
                        contentJson.ToString(),
                        Encoding.UTF8,
                        "application/json"
                    );
                    var httpResponse = await httpClient.PutAsync(uri, content);
                }
            }
            catch
            {
            }
        }
        public async Task AddDoc(LogEvent logEvent)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri($"{baseURL}/{indexName}/{docMethod}");
                    var contentJson = new StringBuilder();
                    contentJson.Append("{");
                    contentJson.AppendFormat("  \"LogEventId\": {0},", logEvent.LogEventId);
                    contentJson.AppendFormat("  \"EventDate\": \"{0}\",", logEvent.EventDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    contentJson.AppendFormat("  \"EventTypeId\": {0},", (int)logEvent.EventTypeId);
                    contentJson.AppendFormat("  \"EventType\": \"{0}\",", logEvent.EventType);
                    contentJson.AppendFormat("  \"MethodName\": \"{0}\",", logEvent.MethodName);
                    contentJson.AppendFormat("  \"ExecutionTime\": {0},", logEvent.ExecutionTime);
                    contentJson.AppendFormat("  \"Description\": \"{0}\"", logEvent.Description);
                    contentJson.Append("}");
                    HttpContent content = new StringContent(
                        contentJson.ToString(),
                        Encoding.UTF8,
                        "application/json"
                    );
                    var httpResponse = await httpClient.PostAsync(uri, content);
                }
            }
            catch
            {
            }
        }
        async Task<bool> IndexExist()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uri = new Uri($"{baseURL}/{indexName}");
                    var httpResponse = await httpClient.GetAsync(uri);
                    return httpResponse.StatusCode == System.Net.HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
