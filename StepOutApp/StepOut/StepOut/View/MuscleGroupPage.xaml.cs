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
    public partial class MuscleGroupPage : ContentView
    {
        public DisplayFicheBO Data { get; set; }
        public List<DisplayFicheBO> Fiches { get; set; }

        public MuscleGroupPage(List<DisplayFicheBO> fiches)
        {
            InitializeComponent();
            Fiches = fiches;
            Startup();
        }

        private async void Startup()
        {
            try
            {
                Data = await Randomfiche();
                lblSpiergroep.Text = Data.TargetMuscleGroup;
                imgMain.Source = Data.MusclePictures[0];
                imgplay.Source = "play.png";
                imgplay2.Source = "play.png";
                imgplay3.Source = "play.png";
                if (Application.Current.Properties.ContainsKey(Data.WorkoutName))
                {
                    switch (Application.Current.Properties[Data.WorkoutName].ToString())
                    {
                        case "Easy":
                            lblEasyRecomended.IsVisible = true;
                            lblHeavyRecomended.IsVisible = false;
                            lblBasicRecomended.IsVisible = false;
                            break;
                        case "Basic":
                            lblBasicRecomended.IsVisible = true;
                            lblEasyRecomended.IsVisible = false;
                            lblHeavyRecomended.IsVisible = false;
                            break;
                        case "Hard":
                            lblHeavyRecomended.IsVisible = true;
                            lblBasicRecomended.IsVisible = false;
                            lblEasyRecomended.IsVisible = false;
                            break;
                    }
                }
                else
                {
                    Application.Current.Properties[Data.WorkoutName] = "Easy";
                    lblEasyRecomended.IsVisible = true;
                    lblHeavyRecomended.IsVisible = false;
                    lblBasicRecomended.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                //Startup(); //indien er iets foutloopt bij het filteren van een random fiche een nieuwe random fiche ophal

            }


        }

        private async Task<DisplayFicheBO> Randomfiche()
        {
            try
            {
                Random rdm = new Random();
                int max = Fiches.Count - 1;
                return Fiches[rdm.Next(0,max)]; 
                
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het ophalen van een random oefening, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
            return null;
        }

        private async void BtnChangeGroup_Clicked(object sender, EventArgs e)
        {
            Startup();
        }

        async void OnEasyTapped(object sender, EventArgs e)
        {
            try
            {
                Application.Current.Properties["Difficulty"] = Data.MoeilijkheidsGraden[0].Graad;
                if (Data.MoeilijkheidsGraden[0].Variaties.Count == 1)
                {
                    Application.Current.Properties["Variatie"] = Data.MoeilijkheidsGraden[0].Variaties[0].Naam;
                    await Navigation.PushAsync(new ExercisePage(Data.MoeilijkheidsGraden[0].Variaties[0], Data) { Title = Data.TargetMuscleGroup });
                }
                else
                {
                    await Navigation.PushAsync(new ExcersizeTabbedPage(Data, "Easy"));
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de oefeningen op niveau easy, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }


        }

        async void OnBasicTapped(object sender, EventArgs e)
        {
            try
            {
                Application.Current.Properties["Difficulty"] = Data.MoeilijkheidsGraden[1].Graad;
                if (Data.MoeilijkheidsGraden[1].Variaties.Count == 1)
                {
                    Application.Current.Properties["Variatie"] = Data.MoeilijkheidsGraden[1].Variaties[0].Naam;
                    await Navigation.PushAsync(new ExercisePage(Data.MoeilijkheidsGraden[1].Variaties[0], Data) { Title = Data.TargetMuscleGroup });
                }
                else
                {
                    await Navigation.PushAsync(new ExcersizeTabbedPage(Data, "Basic"));
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de oefeningen op niveau basic, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        async void OnHeavyTapped(object sender, EventArgs e)
        {
            try
            {
                Application.Current.Properties["Difficulty"] = Data.MoeilijkheidsGraden[2].Graad;
                if (Data.MoeilijkheidsGraden[2].Variaties.Count == 1)
                {
                    Application.Current.Properties["Variatie"] = Data.MoeilijkheidsGraden[2].Variaties[0].Naam;
                    await Navigation.PushAsync(new ExercisePage(Data.MoeilijkheidsGraden[2].Variaties[0], Data) { Title = Data.TargetMuscleGroup });
                }
                else
                {
                    await Navigation.PushAsync(new ExcersizeTabbedPage(Data, "Hard"));
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de oefeningen op niveau hard, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }
    }
}
