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
using Microsoft.Azure.Documents;
using System.Linq;

namespace StepOutApi
{
    public static class Get_UpdatePassword
    {
        [FunctionName("GetUpdatePasswordV2")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "UpdatePasswoord/{Email}/{Wachtwoord}")] HttpRequest req, ILogger log, string Wachtwoord, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutData");
                FeedOptions queryOptions = new FeedOptions { EnableCrossPartitionQuery = true };


                string query = $"SELECT c.id FROM c WHERE c.Email ='{Email}' AND c.Gebruik = 'Login'";
                ReplaceUser DocId = client.CreateDocumentQuery<ReplaceUser>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();

                //Fetch the Document to be updated
                Document doc = client.CreateDocumentQuery<Document>(collectionUrl, queryOptions)
                                            .Where(r => r.Id == DocId.id.ToString())
                                            .AsEnumerable()
                                            .SingleOrDefault();

                //Update some properties on the found resource

                doc.SetPropertyValue("Wachtwoord", Wachtwoord);

                //Now persist these changes to the database by replacing the original resource
                Document updated = await client.ReplaceDocumentAsync(doc);
                return new StatusCodeResult(200);


            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        /*
        [FunctionName("GetUpdatePasswordV2")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V0/UpdatePasswoord/{Email}/{Wachtwoord}")] HttpRequest req, ILogger log, string Wachtwoord, string Email)
        {
            try
            {
                Uri serviceEndpoint = new Uri(Environment.GetEnvironmentVariable("CosmosEndPoint"));
                string key = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
                DocumentClient client = new DocumentClient(serviceEndpoint, key);
                var collectionUrl = UriFactory.CreateDocumentCollectionUri("StapOutData", "StepOutLogin");
                FeedOptions queryOptions = new FeedOptions { EnableCrossPartitionQuery = true };


                string query = $"SELECT c.id FROM c WHERE c.Email ='{Email}'";
                ReplaceUser DocId = client.CreateDocumentQuery<ReplaceUser>(collectionUrl, query, queryOptions).AsEnumerable().SingleOrDefault();

                //Fetch the Document to be updated
                Document doc = client.CreateDocumentQuery<Document>(collectionUrl, queryOptions)
                                            .Where(r => r.Id == DocId.id.ToString())
                                            .AsEnumerable()
                                            .SingleOrDefault();

                //Update some properties on the found resource
                doc.SetPropertyValue("Wachtwoord", Wachtwoord);

                //Now persist these changes to the database by replacing the original resource
                Document updated = await client.ReplaceDocumentAsync(doc);
                return new StatusCodeResult(200);


            }
            catch (Exception ex)
            {
                return new StatusCodeResult(404);
            }
        }
        */
    }
}
