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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;

namespace StepOutApi
{
    public static class UpdateLogs
    {
        [FunctionName("UpdateLogs")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                List<Log> newLog = JsonConvert.DeserializeObject<List<Log>>(requestBody);
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { EnableCrossPartitionQuery = true };

                
                string query = $"SELECT * FROM c WHERE c.Gebruik = 'Log'";
                ReplaceLog PrevLog = client.CreateDocumentQuery<ReplaceLog>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();

                PrevLog.Logs = PrevLog.Logs.Concat(newLog).ToList();
                //foreach (Log item in PrevLog.Logs)
                //{
                //    newLog.Append(item);
                //}
                //Fetch the Document to be updated
                Document doc = client.CreateDocumentQuery<Document>(collectionUrl, queryOptions)
                                            .Where(r => r.Id == PrevLog.id.ToString())
                                            .AsEnumerable()
                                            .SingleOrDefault();

                //Update some properties on the found resource
                doc.SetPropertyValue("Logs", PrevLog.Logs);

                //Now persist these changes to the database by replacing the original resource
                Document updated = await client.ReplaceDocumentAsync(doc);
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }
        }
    }
}
