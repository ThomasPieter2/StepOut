using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StepOut.Models
{
    public static class CacheManager
    {
        //voor logged in user weg te schrijven kan je gebruik maken van: Application.Current.Properties["ID"] = Email; en om uit te lezen: Application.Current.Properties["ID"].ToString();

        // lijst van bestaande bestanden die gebruitk worden om data lokaal op te slaan.
        public static List<string> filenames = new List<string>() { "Fiche.json", "Evaluatie.json" };

        /// <summary>
        /// Haalt alle fiches op uit de database en chached ze.
        /// </summary>
        /// <returns></returns>









        #region Evaluatie
        /// <summary>
        /// Evaluatie ophalen voor huidge gebruiker
        /// </summary>
        /// <returns>EvalutatieBO</returns>
        public static async Task<List<EvaluatieBO>> GetLocalEvaluatie()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Evaluatie.txt");
                if (File.Exists(filename) == true)
                {
                    var data = File.ReadAllText(filename);
                    if (data != null)
                    {
                        List<EvaluatieBO> eval = JsonConvert.DeserializeObject<List<EvaluatieBO>>(data);
                        return eval;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task SetSyncLocalEvaluation(EvaluatieBO eval)
        {
            try
            {

                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "EvaluationOffline.txt");
                List<EvaluatieBO> evaluatie = new List<EvaluatieBO>();
                evaluatie.Add(eval);
                var data = JsonConvert.SerializeObject(evaluatie);
                if (File.Exists(filename) == true)
                {
                    List<EvaluatieBO> evaluaties = new List<EvaluatieBO>();
                    evaluaties = JsonConvert.DeserializeObject<List<EvaluatieBO>>(File.ReadAllText(filename));
                    evaluaties.Add(eval);
                    File.Delete(filename);
                    var ldata = JsonConvert.SerializeObject(evaluaties);
                    File.WriteAllText(filename, ldata);
                }
                else File.WriteAllText(filename, data);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<EvaluatieBO>> GetSyncLocalEvaluation() //nog te implementeren.
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "EvaluationOffline.txt");
                if (File.Exists(filename) == true)
                {
                    var data = File.ReadAllText(filename);
                    if (data != null)
                    {
                        List<EvaluatieBO> eval = JsonConvert.DeserializeObject<List<EvaluatieBO>>(data);
                        return eval;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal static void RemoveAllSyncEvaluations()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "EvaluationOffline.txt");
                if (File.Exists(filename) == true) File.Delete(filename);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal static void RemoveAllEvaluations()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Evaluatie.txt");
                if (File.Exists(filename) == true) File.Delete(filename);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Fiche

        /// <summary>
        /// Data naar local files wegschrijven
        /// </summary>
        /// <param name="fiche">De lijst met alle fichedata die je wilt wegschrijven naar het bestand</param>
        public static async void WriteLocalFicheBO(List<FicheBO> fiche)
        {

            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Fiche.txt");
                var data = JsonConvert.SerializeObject(fiche);
                File.WriteAllText(filename, data);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<FicheBO>> GetLocalFicheBO()
        {

            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Fiche.txt");
                if (File.Exists(filename) == true)
                {
                    var data = File.ReadAllText(filename);
                    if (data != null)
                    {
                        List<FicheBO> fiche = JsonConvert.DeserializeObject<List<FicheBO>>(data);
                        return fiche;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void RemoveAllFichesBO()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Fiche.txt");
                if (File.Exists(filename) == true) File.Delete(filename);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal static async void WriteLocalEvaluatie(List<EvaluatieBO> data)
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Evaluatie.txt");
                var sdata = JsonConvert.SerializeObject(data);

                if (File.Exists(filename) == true)
                {
                    List<EvaluatieBO> evaluaties = new List<EvaluatieBO>();
                    evaluaties = JsonConvert.DeserializeObject<List<EvaluatieBO>>(File.ReadAllText(filename));
                    foreach (EvaluatieBO eval in data)
                    {
                        evaluaties.Add(eval);
                        File.Delete(filename);
                        var ldata = JsonConvert.SerializeObject(evaluaties);
                        File.WriteAllText(filename, ldata);
                    }

                }
                else File.WriteAllText(filename, sdata);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void WriteEvaluatie(List<EvaluatieBO> data)
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Evaluatie.txt");
                var sdata = JsonConvert.SerializeObject(data);
                File.WriteAllText(filename, sdata);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static async Task WriteLog(Exception ex)
        {
            try
            {
                Logging log = new Logging();
                log.ErrorDateTime = DateTime.Now;
                log.ErrorMessage = ex.Message;
                List<Logging> logs = new List<Logging>();
                logs.Add(log);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Log.txt");
                if (File.Exists(filename) == true)
                {
                    var PrevData = File.ReadAllText(filename);
                    List<Logging> PrevLogs = JsonConvert.DeserializeObject<List<Logging>>(PrevData);
                    foreach (Logging logging in logs)
                    {
                        PrevLogs.Add(log);
                    }
                    var sdata = JsonConvert.SerializeObject(PrevLogs);
                    File.Delete(filename);
                    File.WriteAllText(filename, sdata);
                }
                else
                {
                    var sdata = JsonConvert.SerializeObject(logs);
                    File.WriteAllText(filename, sdata);
                }

            }

            catch (Exception e)
            {
                throw ex;
            }
        }

        internal static async Task<List<Logging>> GetLog()
        {

            try
            {
                List<Logging> fiche = new List<Logging>();
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filename = Path.Combine(path, "Log.txt");
                if (File.Exists(filename) == true)
                {
                    var data = File.ReadAllText(filename);
                    if (data != null)
                    {
                        fiche = JsonConvert.DeserializeObject<List<Logging>>(data);
                        return fiche;
                    }
                }
                return fiche;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        #endregion

    }
}
