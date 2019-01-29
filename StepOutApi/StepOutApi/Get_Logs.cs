using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using StepOutApi.Model;
using System.Linq;

namespace StepOutApi
{
    public static class Get_Logs
    {
        [FunctionName("Get_Logs")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetLogs")] HttpRequest req, ILogger log)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT * FROM c WHERE c.Gebruik = 'Log'";
                logging result = client.CreateDocumentQuery<logging>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();

                return new OkObjectResult(result.Logs);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
