using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using StepOutApi.Model;
using Microsoft.Azure.Documents.Client;

namespace StepOutApi
{
    public static class Post_FicheData
    {
        [FunctionName("FicheData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostFicheData")] HttpRequest req,ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                FicheV2 data = JsonConvert.DeserializeObject<FicheV2>(requestBody);
                data.FicheId = Guid.NewGuid();
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");

                await client.CreateDocumentAsync(collectionUrl, data);

                return new StatusCodeResult(200);
            }
            catch (Exception ex) 
            {

                //om makklijk fouten te kunnen opsporen.
                //return new OkObjectResult(ex.Message);
                return new StatusCodeResult(500);
            }

        }
    }
}
