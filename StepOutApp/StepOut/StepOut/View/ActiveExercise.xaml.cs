using Plugin.BLE.Abstractions.Contracts;
using StepOut.BluetoothConnect;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveExercise : ContentPage
    {
        BluetoothManager btManager = new BluetoothManager();
        public int hr;
        bool reading = true;
        List<int> hrList = new List<int>();

        public DisplayVariatyBO Variatie { get; set; }
        public DisplayFicheBO Fiche { get; set; }
        public int Sets { get; set; }
        public EvaluatieBO eval { get; set; }
        public ActiveExercise(DisplayVariatyBO variatie, DisplayFicheBO fiche)
        {
            InitializeComponent();
            Fiche = fiche;
            Variatie = variatie;
            Sets = 1;
            eval = new EvaluatieBO();
            eval.Email = Application.Current.Properties["Current_User"].ToString();
            eval.Datum = DateTime.Now;
            eval.WorkoutNaam = Fiche.WorkoutName;
            eval.Variatie = variatie.Naam;
            eval.Graad = Application.Current.Properties["Difficulty"].ToString();
            StartUp();
        }

        private async void StartUp()
        {
            try
            {
                imgEx.Source = Variatie.Foto[0];
                Title = "Set   " + Sets.ToString() + "/3";
                lblOef.Text = Variatie.Naam;
                lblRepetities.Text = Fiche.Score.Excellent.ToString();

                imgHeart.Source = "heart.png";
                imgHeart2.Source = "heart.png";



                reading = true;

                if (Variatie.Naam != "") lblOef.Text = Variatie.Naam;
                else lblOef.Text = Fiche.WorkoutName;

                var characteristic = CacheProvidor.Get<ICharacteristic>("characteristic");

                if (characteristic != null) await getHeartRate(characteristic);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Fout bij het laden van de pagina", "Ok");
            }
        }

        private async void BtnKlaar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Sets == 3)
                {
                    reading = false;
                    HeartRateModel hrm = new HeartRateModel()
                    {
                        HeartRate = hrList
                    };
                    eval.Set2 = int.Parse(Application.Current.Properties["Set"].ToString());
                    await Navigation.PushAsync(new ExerciseEvaluationPage(Variatie, Fiche, eval, hrList));
                }
                else
                {
                    switch (Sets)
                    {
                        case 2:
                            eval.Set1 = int.Parse(Application.Current.Properties["Set"].ToString());
                            break;
                    }
                    Sets += 1;
                    Title = "Set " + Sets.ToString() + "/3";
                    await Navigation.PushAsync(new BreakExercise(Variatie, Fiche, Sets));
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", ex.Message + "Probeer het later opnieuw", "Ok");
            }
        }

        #region Live HeartRate

        private async Task getHeartRate(ICharacteristic characteristic)
        {
            try
            {
                while (reading)
                {
                    hr = await btManager.ReadHeartRate(characteristic);
                    hrList.Add(hr);
                    lblHuidig.Text = hr.ToString();
                    foreach (var hra in hrList)
                    {

                    }
                    double avgHr = hrList.Average();
                    lblGemiddeld.Text = Convert.ToInt16(avgHr).ToString();
                    await Task.Delay(950);
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Fout bij het uitlezen van de live heartrate, probeer dit later opnieuw", "Ok");
            }
        }

        #endregion
    }
}