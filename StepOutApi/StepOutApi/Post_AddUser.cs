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

namespace StepOutApi
{
    public static class Post_AddUser
    {
        [FunctionName("PostAddUser")]
        public static async Task<IActionResult> RunV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserV2 data = JsonConvert.DeserializeObject<UserV2>(requestBody);
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");

                await client.CreateDocumentAsync(collectionUrl, data);
            }
            catch (Exception ex)
            {

                //om makklijk fouten te kunnen opsporen.
                return new OkObjectResult(ex.Message);
                //return new StatusCodeResult(500);
            }

            return new StatusCodeResult(200);
        }

        [FunctionName("PostAddUserV0")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "V0")] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserBO data = JsonConvert.DeserializeObject<UserBO>(requestBody);
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutLogin");

                await client.CreateDocumentAsync(collectionUrl, data);
            }
            catch (Exception ex)
            {

                //om makklijk fouten te kunnen opsporen.
                return new OkObjectResult(ex.Message);
                //return new StatusCodeResult(500);
            }

            return new StatusCodeResult(200);
        }
    }
}

