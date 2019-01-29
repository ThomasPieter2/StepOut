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

namespace StepOutApi
{
    public static class Get_EmailExists
    {
        [FunctionName("GetEmailExistsV2")]
        public static async Task<IActionResult> RunV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "EmailExists/{Email}")] HttpRequest req, ILogger log, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT * FROM c WHERE c.Email ='{Email}' AND c.Gebruik = 'Login'";
                var result = client.CreateDocumentQuery<UserV2>(collectionUrl, query, queryOptions).AsEnumerable();
                List<UserV2> users = result.ToList<UserV2>();
                if (users.Count == 0)
                {
                    return new OkObjectResult(false);
                }
                else
                {
                    return new OkObjectResult(true);
                }

                //IQueryable<FicheBO> result = client.CreateDocumentQuery<FicheBO>(collectionUrl, queryOptions).Where(l => l.Location == location);
                //return new OkObjectResult(result.ToList<FicheBO>());
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex);
                //return new StatusCodeResult(500);
            }
        }
        /*
        [FunctionName("GetEmailExists")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V0/EmailExists/{Email}")] HttpRequest req,ILogger log, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutLogin");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT * FROM c WHERE c.Email ='{Email}'";
                var result = client.CreateDocumentQuery<UserBO>(collectionUrl, query, queryOptions).AsEnumerable();
                List<UserBO> users = result.ToList<UserBO>();
                if (users.Count == 0)
                {
                    return new OkObjectResult(false);
                }
                else
                {
                    return new OkObjectResult(true);
                }

                //IQueryable<FicheBO> result = client.CreateDocumentQuery<FicheBO>(collectionUrl, queryOptions).Where(l => l.Location == location);
                //return new OkObjectResult(result.ToList<FicheBO>());
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex);
                //return new StatusCodeResult(500);
            }
        }
        */
    }
}
