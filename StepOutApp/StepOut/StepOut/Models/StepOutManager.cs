using Newtonsoft.Json;
using Plugin.Connectivity;
using StepOut.Authorize;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StepOut.Models
{
    public static class StepOutManager
    {
        public static string CurrentUser = "delaet.ruben@gmail.com";



        /// <summary>
        /// converteren van List<FicheBO> naar List<DisplayFicheBO> dit is nodig om alle images te kunnen gebruiken met bindings</FicheBO>
        /// </summary>
        /// <param name="fiche"></param>
        /// <returns>DisplayFicheBO[]</returns>
        public static List<DisplayFicheBO> GetConvertedWorkouts(List<FicheBO> fiche)
        {
            try
            {
                List<DisplayFicheBO> fiches = new List<DisplayFicheBO>();
                foreach (FicheBO fi in fiche)
                {
                    DisplayFicheBO f = new DisplayFicheBO();
                    f.FicheId = fi.FicheId;
                    List<ImageSource> images = new List<ImageSource>();
                    foreach (string item in fi.MusclePictures)
                    {
                        images.Add(new UriImageSource
                        {
                            Uri = new Uri($"https://stepoutapidata.blob.core.windows.net/img/{item}"),
                            CachingEnabled = true,
                            CacheValidity = new TimeSpan(30, 0, 0, 0)
                        });



                    }
                    f.MusclePictures = images;
                    if (fi.MoeilijkheidsGraden != null)
                    {
                        List<DisplayMoeilijkheidsgradenBO> ldm = new List<DisplayMoeilijkheidsgradenBO>();
                        foreach (MoeilijkheidsgradenBO mi in fi.MoeilijkheidsGraden)
                        {
                            List<DisplayVariatyBO> ldv = new List<DisplayVariatyBO>();
                            foreach (VariatyBO vi in mi.Variaties)
                            {
                                List<ImageSource> pictures = new List<ImageSource>();
                                foreach (var si in vi.Foto)
                                {
                                    pictures.Add(new UriImageSource
                                    {
                                        Uri = new Uri($"https://stepoutapidata.blob.core.windows.net/img/{si}"),
                                        CachingEnabled = true,
                                        CacheValidity = new TimeSpan(30, 0, 0, 0)
                                    });
                                }
                                DisplayVariatyBO dv = new DisplayVariatyBO()
                                {
                                    Foto = pictures,
                                    Naam = vi.Naam,
                                    Uitleg = vi.Uitleg
                                };
                                ldv.Add(dv);
                            }
                            DisplayMoeilijkheidsgradenBO dm = new DisplayMoeilijkheidsgradenBO()
                            {
                                Graad = mi.Graad,
                                Variaties = ldv
                            };
                            ldm.Add(dm);
                        }
                        f.MoeilijkheidsGraden = ldm;
                    }
                    f.Score = fi.Score;
                    f.TargetMuscleGroup = fi.TargetMuscleGroup;
                    f.WorkoutName = fi.WorkoutName;
                    f.Workout = fi.Workout;
                    fiches.Add(f);
                }
                return fiches;
            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het converteren van de workouts.");
            }
        }

        #region Alle Get's met caching etc
        public static async Task<List<DisplayFicheBO>> GetAllFiches()
        {
            List<FicheBO> data = new List<FicheBO>();
            try
            {
                data = CacheProvidor.Get<List<FicheBO>>("Fiche");
                if (data == null)
                {
                    if (await CacheManager.GetLocalFicheBO() == null)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        string url = "https://stepoutapi.azurewebsites.net/api/V3/GetFiches";
                        string json = await client.GetStringAsync(url);
                        Debug.WriteLine("Json: " + json);
                        if (json != null)
                        {
                            data = JsonConvert.DeserializeObject<List<FicheBO>>(json);
                            CacheProvidor.Set<List<FicheBO>>("Fiche", data, DateTime.Now.AddDays(1));
                            CacheManager.WriteLocalFicheBO(data);
                        }
                    }
                    else
                    {
                        data = await CacheManager.GetLocalFicheBO();
                        CacheProvidor.Set<List<FicheBO>>("Fiche", data, DateTime.Now.AddDays(1));
                    }
                }
                return StepOutManager.GetConvertedWorkouts(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het ophalen van alle workouts, controleer of je verbinding hebt met het internet, indien dit de fout niet oplost kan men best contact opnemen met de support.");
            }
        }

        public static async Task SyncAllFichesBO()
        {
            try
            {
                CacheManager.RemoveAllFichesBO();
                CacheProvidor.Set("Fiche", "", DateTime.Now);
                List<FicheBO> data = new List<FicheBO>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string url = "https://stepoutapi.azurewebsites.net/api/V3/GetFiches";
                string json = await client.GetStringAsync(url);
                Debug.WriteLine("Json: " + json);
                if (json != null)
                {
                    data = JsonConvert.DeserializeObject<List<FicheBO>>(json);
                    CacheProvidor.Set<List<FicheBO>>("Fiche", data, DateTime.Now.AddDays(1));
                    CacheManager.WriteLocalFicheBO(data);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het synchroniseren van alle workouts");
            }

        }


        public static async Task<List<EvaluatieBO>> GetAllEvaluaties(string Email)
        {
            List<EvaluatieBO> data = new List<EvaluatieBO>();
            try
            {

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string url = $"https://stepoutapi.azurewebsites.net/api/V3/GetEvaluaties/{Email}";
                string json = await client.GetStringAsync(url);
                Debug.WriteLine("Json: " + json);
                if (json != null)
                {
                    data = JsonConvert.DeserializeObject<List<EvaluatieBO>>(json);
                    CacheProvidor.Set<List<EvaluatieBO>>("Evaluatie", data, DateTime.Now.AddDays(1));
                    CacheManager.WriteEvaluatie(data);
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het ophalen van alle evaluaties.");
            }
        }
        public static async Task SyncLocalAllEvaluations()
        {
            try
            {
                List<EvaluatieBO> SyncData = new List<EvaluatieBO>();
                SyncData = await CacheManager.GetSyncLocalEvaluation();
                if (SyncData != null)
                {

                    CacheManager.WriteLocalEvaluatie(SyncData);
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            foreach (EvaluatieBO evaluatie in SyncData)
                            {

                                await AddEvaluations(evaluatie);
                            }
                        }
                        CacheManager.RemoveAllSyncEvaluations();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het Lokaal synchroniseren van de evaluaties.");
            }
        }

        /// <summary>
        /// Alle evaluaties ophalen uit de cloud, alle lokale evaluaties verwijderen
        /// </summary>
        /// <param name="Email">De desbetredende gebruiker</param>
        /// <returns></returns>
        public static async Task SyncAllEvaluations(string Email)
        {
            try
            {
                CacheManager.RemoveAllEvaluations();
                CacheProvidor.Set("Evaluatie", "", DateTime.Now);
                List<EvaluatieBO> data = new List<EvaluatieBO>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string url = "https://stepoutapi.azurewebsites.net/api/V3/GetEvaluaties/{Email}";
                string json = await client.GetStringAsync(url);
                Debug.WriteLine("Json: " + json);
                if (json != null)
                {
                    data = JsonConvert.DeserializeObject<List<EvaluatieBO>>(json);
                    CacheProvidor.Set<List<EvaluatieBO>>("Evaluatie", data, DateTime.Now.AddDays(1));
                    CacheManager.WriteEvaluatie(data);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het synchroniseren van de evaluaties.");
            }
        }
        public static async Task AddEvaluations(EvaluatieBO evaluatie)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://stepoutapi.azurewebsites.net/api/V3/PostEvaluatie";
                    string json = JsonConvert.SerializeObject(evaluatie);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode) Debug.WriteLine("Succesfully added");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het wegschrijven van de evaluaties.");

            }
        }
        public static async Task WriteLocal(EvaluatieBO evaluatie)
        {
            try
            {
                await CacheManager.SetSyncLocalEvaluation(evaluatie);
            }
            catch (Exception ex)
            {

                throw new Exception("Fout bij het wegschrijven van een evaluatie");
            }
        }


        public static async Task Writelog(Exception e)
        {
            try
            {
                List<Logging> Logs = await CacheManager.GetLog();
                if (CrossConnectivity.Current.IsConnected)
                {
                    Logging Log = new Logging();
                    Log.ErrorDateTime = DateTime.Now;
                    Log.ErrorMessage = e.Message;
                    Logs.Add(Log);
                    using (HttpClient client = new HttpClient())
                    {
                        string url = "https://stepoutapi.azurewebsites.net/api/UpdateLogs";
                        string json = JsonConvert.SerializeObject(Logs);
                        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode) Debug.WriteLine("Succesfully added");
                    }
                }
                else await CacheManager.WriteLog(e);
            }
            catch (Exception ex)
            {
                throw new Exception("Fout bij het wegschrijven van de logging.");
            }
        }

        //public static async Task SyncLog()
        //{
        //    try
        //    {
        //        List<Logging> Logs = await CacheManager.GetLog();

        //        if (Logs.Count != 0 && !CrossConnectivity.Current.IsConnected)
        //        {
        //            using (HttpClient client = new HttpClient())
        //            {
        //                string url = "http://stepoutapi.azurewebsites.net/api/V3/PostEvaluatie";
        //                string json = JsonConvert.SerializeObject(Logs);
        //                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //                var response = await client.PostAsync(url, content);
        //                if (response.IsSuccessStatusCode) Debug.WriteLine("Succesfully added");
        //            } 
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Fout bij het wegschrijven van de logging.");
        //    }
        //}
        #endregion
    }
}
