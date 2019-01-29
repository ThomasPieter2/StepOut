using StepOut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercisesView : ContentView, INotifyPropertyChanged
    {

        public List<DisplayFicheBO> StartData { get; set; }


        public ExercisesView(List<DisplayFicheBO> fiche)
        {
            StartData = fiche;
            InitializeComponent();
            Startup();
        }

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

        private async Task Startup()
        {
            try
            {
                lvwFiches.ItemsSource = StartData;
                List<String> pickerData = new List<String>();
                pickerData.Add("All");
                foreach (DisplayFicheBO i in StartData)
                {
                    if (!pickerData.Contains(i.TargetMuscleGroup.ToString()))
                    {
                        pickerData.Add(i.TargetMuscleGroup);
                    }
                }
                pckGroup.ItemsSource = pickerData;
                pckGroup.SelectedItem = "All";
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het tonen van de geselecteerde oefening", "Ok");
            }
        }
        private async void lvwFiches_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                DisplayFicheBO f = (DisplayFicheBO)lvwFiches.SelectedItem;
                if(f != null)
                {
                    lvwFiches.SelectedItem = null;
                    scrFilter.Text = "";
                    pckGroup.SelectedItem = "All";
                    await Navigation.PushAsync(new MuscleGroupView(f) { Title = f.TargetMuscleGroup });
                }

            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout","Er is iets misgelopen bij het tonen van de geselecteerde oefening","Ok");
            }
            
        }

        private async void ScrFilter_SearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                string UserInput = scrFilter.Text;
                if (UserInput != "")
                {
                    List<DisplayFicheBO> Data = new List<DisplayFicheBO>();
                    foreach (DisplayFicheBO f in StartData)
                    {

                        if (f.WorkoutName.ToLower().Contains(UserInput.ToLower())) Data.Add(f);

                    }
                    lvwFiches.ItemsSource = Data;
                }
                else
                {
                    lvwFiches.ItemsSource = StartData;
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het filteren van de data, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
            
        }

        private async void PckGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (pckGroup.SelectedItem != null)
                {
                    if (pckGroup.SelectedItem.ToString() == "All")
                    {
                        lvwFiches.ItemsSource = StartData;
                    }
                    else
                    {
                        string UserInput = pckGroup.SelectedItem.ToString();
                        List<DisplayFicheBO> Data = new List<DisplayFicheBO>();
                        foreach (DisplayFicheBO f in StartData)
                        {
                            if (f.TargetMuscleGroup == UserInput) Data.Add(f);
                        }
                        lvwFiches.ItemsSource = Data;
                    }

                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het filteren van de data, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
            
           
        }

        private async void ScrFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string UserInput = scrFilter.Text;
                if (UserInput != "")
                {
                    List<DisplayFicheBO> Data = new List<DisplayFicheBO>();
                    foreach (DisplayFicheBO f in StartData)
                    {

                        if (f.WorkoutName.ToLower().Contains(UserInput.ToLower())) Data.Add(f);

                    }
                    lvwFiches.ItemsSource = Data;
                }
                else
                {
                    lvwFiches.ItemsSource = StartData;
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het filteren van de data, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
        }

        private async void LvwFiches_Refreshing(object sender, EventArgs e)
        {
            await Startup();
            lvwFiches.IsRefreshing = false;
        }
    }
}