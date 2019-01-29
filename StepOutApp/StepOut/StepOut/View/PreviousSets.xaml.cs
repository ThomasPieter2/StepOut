using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using StepOut.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviousSets : ContentPage, INotifyPropertyChanged
    {
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public DisplayFicheBO Fiche { get; set; }
        public PreviousSets(DisplayFicheBO fiche)
        {
            Title = "Vorige evaluaties";
            InitializeComponent();
            Fiche = fiche;
            actLoading.IsEnabled = true;
            actLoading.IsVisible = true;
            lvwSets.IsVisible = false;
            StartUp();

        }


        private async Task StartUp()
        {
            try
            {
                if (IsBusy) return;
                lblNoResults.IsVisible = false;
                List<EvaluatieBO> evaluaties = new List<EvaluatieBO>();
                List<EvaluatieBO> data = new List<EvaluatieBO>();
                await StepOutManager.SyncLocalAllEvaluations();
                if (CrossConnectivity.Current.IsConnected)
                {
                    //await ja nee, nog te testen
                    evaluaties = await StepOutManager.GetAllEvaluaties(Application.Current.Properties["Current_User"].ToString());
                }
                else
                {
                    evaluaties = await CacheManager.GetLocalEvaluatie();
                }
                foreach (EvaluatieBO evaluatie in evaluaties)
                {
                    if (evaluatie.Graad == Application.Current.Properties["Difficulty"].ToString() && evaluatie.Variatie == Application.Current.Properties["Variatie"].ToString() && evaluatie.WorkoutNaam == Fiche.WorkoutName && evaluatie.Email == Application.Current.Properties["Current_User"].ToString()) data.Add(evaluatie);
                }
                actLoading.IsEnabled = false;
                actLoading.IsVisible = false;
                lvwSets.IsVisible = true;
                if (data.Count == 0) lblNoResults.Text = "Deze oefening werd nog niet eerder uitgevoerd."; lblNoResults.IsVisible = true;//await DisplayAlert("Opgepast", "Nog geen vorige registraties beschikbaar", "ok");
                lvwSets.ItemsSource = data;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het weergeven van de vorige registraties, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        private async void LvwSets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                EvaluatieBO eval = (EvaluatieBO)lvwSets.SelectedItem;
                if (eval != null)
                {
                    await Navigation.PushAsync(new SetOverviewPage(eval));
                }
                lvwSets.SelectedItem = null;

            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van het overzicht, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        private async void LvwSets_Refreshing(object sender, EventArgs e)
        {

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    List<EvaluatieBO> evaluaties = new List<EvaluatieBO>();
                    List<EvaluatieBO> data = new List<EvaluatieBO>();
                    await StepOutManager.SyncLocalAllEvaluations();//await ja nee, nog te testen
                    evaluaties = await StepOutManager.GetAllEvaluaties(Application.Current.Properties["Current_User"].ToString());
                    foreach (EvaluatieBO evaluatie in evaluaties)
                    {
                        if (evaluatie.Graad == Application.Current.Properties["Difficulty"].ToString() && evaluatie.Variatie == Application.Current.Properties["Variatie"].ToString() && evaluatie.WorkoutNaam == Fiche.WorkoutName) data.Add(evaluatie);
                    }
                    lvwSets.ItemsSource = data;
                }
                lvwSets.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Melding","Er is iets misgelopen bij het refreshen van de lijst, als deze fout zich blijft voordoen neemt men best contact op met de support","Ok");
            }
        }
    }
}