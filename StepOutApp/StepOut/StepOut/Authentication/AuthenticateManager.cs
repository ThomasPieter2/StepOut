using Newtonsoft.Json;
using StepOut.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StepOut.Models;

namespace StepOut.Authorize
{
    public static class AuthenticateManager
    {
        #region Login //nog niet getest
        public static async Task AddUser(string gebruiker, string password, string email, string land)
        {

            using (HttpClient client = new HttpClient())
            {
                string url = "https://stepoutapi.azurewebsites.net/api/PostAddUser"; //nog aan te passen naar login api

                try
                {
                    UserBO newuser = new UserBO()
                    {
                        Email = email,
                        Naam = gebruiker,
                        Wachtwoord = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                        //Token = null,
                        Land = land

                    };
                    string json = JsonConvert.SerializeObject(newuser);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Succesfully added");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        internal static async Task GetPasswordReset(string Email)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                String url = $"https://stepoutapi.azurewebsites.net/api/ResetPassword/{Email}"; //link nog te corrigerenvan de api
                String json = await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal static async Task ResetPassword(string Email,string newpassword)
        {
            try
            {
                string Wachtwoord = BCrypt.Net.BCrypt.EnhancedHashPassword(newpassword);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                String url = $"https://stepoutapi.azurewebsites.net/api/UpdatePasswoord/{Email}/{Wachtwoord}"; //link nog te corrigerenvan de api
                String json = await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static async Task<Boolean> CheckUserLogin(string Email, string wachtwoord)
        {
            try
            {
                if (Email != null && Email != "" && Email != "" && wachtwoord != null && wachtwoord != string.Empty && wachtwoord != "")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    String url = $"https://stepoutapi.azurewebsites.net/api/UserLogin/{Email}"; //link nog te corrigerenvan de api
                    String json = await client.GetStringAsync(url);
                    Debug.WriteLine("Json: " + json);
                    if (json != null && json != string.Empty && json != "")
                    {
                        UserBO data = JsonConvert.DeserializeObject<UserBO>(json);
                        string password = BCrypt.Net.BCrypt.EnhancedHashPassword(wachtwoord);
                        Debug.WriteLine(password);
                        bool valid = BCrypt.Net.BCrypt.EnhancedVerify(wachtwoord, data.Wachtwoord);
                        return valid;
                    }
                    return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Try-Catch methode failed");
                throw ex;
            }
        }
        public static async Task AddToken(UserBO user)
        {

            using (HttpClient client = new HttpClient())
            {
                string url = "https://stepoutapi.azurewebsites.net/api/UpdateToken"; //nog aan te passen naar login api

                try
                {
                    string json = JsonConvert.SerializeObject(user);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Succesfully added");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<Boolean> CheckEmailExists(string email)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                String url = string.Format($"https://stepoutapi.azurewebsites.net/api/EmailExists/{email}"); //link nog te corrigerenvan de api
                String json = await client.GetStringAsync(url);
                if (json == "true") return true;
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Try-Catch methode failed");
                throw ex;
            }
        }

        public static async Task<UserBO> GetToken(string email)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                String url = $"https://stepoutapi.azurewebsites.net/api/GetToken/{email}";
                String json = await client.GetStringAsync(url);
                Debug.WriteLine("Json: " + json);
                if (json != null)
                {
                    UserBO data = JsonConvert.DeserializeObject<UserBO>(json);
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Try-Catch methode failed");
                throw ex;
            }
        }
        #endregion
    }
}
