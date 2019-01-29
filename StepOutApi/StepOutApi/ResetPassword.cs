using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StepOutApi.Model;
using System.Text;
using System.Linq;
using System.Net.Http;

namespace StepOutApi
{
    public static class ResetPassword
    {
        [FunctionName("ResetPassword")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ResetPassword/{Email}")] HttpRequest req, ILogger log, string Email)
        {
            try
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                char[] token = (Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                string resetcode = "";
                foreach (char item in token)
                {
                    resetcode += item.ToString();
                }

                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendFormat("Beste {0},", Email);
                mailBody.AppendFormat("<br/>");
                mailBody.AppendFormat("<p>Hieronder vindt u uw tijdelijke wachtwoord, gelieve dit wachtwoord zo snel mogelijk aan te passen.</p>");
                mailBody.AppendFormat("<br/>");
                mailBody.AppendFormat($"<p>Tijdelijke wachtwoord:<h2> {resetcode}");

                string Wachtwoord = BCrypt.Net.BCrypt.EnhancedHashPassword(resetcode);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                String url = $"https://stepoutapi.azurewebsites.net/api/UpdatePasswoord/{Email}/{Wachtwoord}"; //link nog te corrigerenvan de api
                String json = await client.GetStringAsync(url);


                Mailer.SendEmail("noreply@StepOut.be", Email, "StepOut wachtwoord vergeten", mailBody.ToString());
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }

        }
    }
}
