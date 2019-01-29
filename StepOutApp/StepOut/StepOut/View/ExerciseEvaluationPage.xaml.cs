using Plugin.Connectivity;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseEvaluationPage : ContentPage
    {
        public DisplayVariatyBO Variatie { get; set; }
        public DisplayFicheBO Fiche { get; set; }
        public EvaluatieBO Evaluatie { get; set; }
        public List<int> listHr = new List<int>();

        public ExerciseEvaluationPage(DisplayVariatyBO variatie, DisplayFicheBO fiche, EvaluatieBO eval, List<int> listhr)
        {
            InitializeComponent();
            Fiche = fiche;
            Evaluatie = eval;
            Variatie = variatie;
            listHr = listhr;
            Title = "Evaluatie";
            StartUp();
        }

        private async void StartUp()
        {
            try
            {
                imgOef.Source = Variatie.Foto[0];
                if (Variatie.Naam != "") lblOef.Text = Variatie.Naam;
                else lblOef.Text = Fiche.WorkoutName;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het inladen van deze pagina, als deze fout zich blijft voordoen gelieve contact op te nemen met de suport.", "Ok");
            }
        }

        private async void btnBevestig_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (entryReps.Text == null) Evaluatie.Set3 = int.Parse(entryReps.Placeholder);
                    else Evaluatie.Set3 = int.Parse(entryReps.Text.Replace(",", ""));



                    if (Evaluatie.Moeilijkheid <= 25)
                    {
                        switch (Evaluatie.Graad)
                        {
                            case "Easy":
                                Application.Current.Properties[Fiche.WorkoutName] = "Basic";
                                break;
                            case "Basic":
                                Application.Current.Properties[Fiche.WorkoutName] = "Hard";
                                break;
                            case "Hard":
                                Application.Current.Properties[Fiche.WorkoutName] = "Hard";
                                break;
                        }
                    }
                    //else if (Evaluatie.Moeilijkheid < 60 && Evaluatie.Moeilijkheid > 25)
                    //{
                    //    //niet nodig om aan te passen als het al bestaat :)
                    //    if (!Application.Current.Properties.ContainsKey(Fiche.WorkoutName)) Application.Current.Properties[Fiche.WorkoutName] = Application.Current.Properties["Variatie"];
                    //}
                    else if (Evaluatie.Moeilijkheid >= 60)
                    {
                        switch (Evaluatie.Graad)
                        {
                            case "Hard":
                                Application.Current.Properties[Fiche.WorkoutName] = "Basic";
                                break;
                            case "Basic":
                                Application.Current.Properties[Fiche.WorkoutName] = "Easy";
                                break;
                            case "Easy":
                                Application.Current.Properties[Fiche.WorkoutName] = "Easy";
                                break;
                        }
                    }
                    await Navigation.PushAsync(new ExerciseOverviewPage(listHr, Evaluatie));
                }
                else
                {
                    await Navigation.PushAsync(new ExerciseOverviewPage(listHr, Evaluatie));
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het opslaan van de data van deze oefening controleer of je een realistische waarde hebt ingegeven, als deze fout zich blijft voordoen gelieve contact op te nemen met de suport", "Ok");
            }
        }

        private async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                double test = e.NewValue;
                Debug.WriteLine(test);
                Debug.WriteLine("test");
                Evaluatie.Moeilijkheid = Convert.ToInt32(test);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het uitlezen van de slider, als deze fout zich blijft voordoen gelieve contact op te nemen met de support.", "Ok");
            }
        }
    }
}