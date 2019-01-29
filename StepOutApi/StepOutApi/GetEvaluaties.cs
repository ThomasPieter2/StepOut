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
using System.Linq;
using StepOutApi.Model;

namespace StepOutApi.API_V3
{
    public static class GetEvaluaties
    {
        [FunctionName("GetEvaluaties")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V3/GetEvaluaties/{Email}")] HttpRequest req, ILogger log,string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT * FROM c WHERE c.Gebruik = 'Evaluatie'AND c.Email = '{Email}'";
                var result = client.CreateDocumentQuery<EvaluatieBO>(collectionUrl, query, queryOptions).AsEnumerable();

                //IQueryable<FicheBO> result = client.CreateDocumentQuery<FicheBO>(collectionUrl, queryOptions).Where(l => l.Location == location);
                //return new OkObjectResult(result.ToList<FicheBO>());
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
