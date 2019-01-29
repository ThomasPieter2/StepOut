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
using System.Collections.Generic;

namespace StepOutApi.API_V3
{
    public static class GetFiches
    {
        [FunctionName("GetFiches")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V3/GetFiches")] HttpRequest req, ILogger log)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT * FROM c WHERE c.Gebruik = 'Fiche'";
                var result = client.CreateDocumentQuery<FicheBO>(collectionUrl, query, queryOptions).AsEnumerable();
                List<FicheBO> lstExcersizes = result.ToList<FicheBO>();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }

        }
    }
}
