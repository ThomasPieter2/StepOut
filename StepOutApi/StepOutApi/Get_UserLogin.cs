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
using System.Reflection.Metadata;

namespace StepOutApi
{
    public static class Get_UserLogin
    {
        [FunctionName("GetUserLoginV2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "UserLogin/{Email}")] HttpRequest req, ILogger log, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT c.Wachtwoord FROM c WHERE c.Email ='{Email}' AND c.Gebruik = 'Login'";
                UserBO result = client.CreateDocumentQuery<UserBO>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();


                //IQueryable<FicheBO> result = client.CreateDocumentQuery<FicheBO>(collectionUrl, queryOptions).Where(l => l.Location == location);
                //return new OkObjectResult(result.ToList<FicheBO>());
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex);
                //return new StatusCodeResult(500);
            }
        }

        /*
        [FunctionName("GetUserLogin")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V0/UserLogin/{Email}")] HttpRequest req, ILogger log, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutLogin");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

                string query = $"SELECT c.Wachtwoord FROM c WHERE c.Email ='{Email}'";
                UserBO result = client.CreateDocumentQuery<UserBO>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();


                //IQueryable<FicheBO> result = client.CreateDocumentQuery<FicheBO>(collectionUrl, queryOptions).Where(l => l.Location == location);
                //return new OkObjectResult(result.ToList<FicheBO>());
                return new OkObjectResult(result);
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
