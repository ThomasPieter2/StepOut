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
    public partial class ExercisePage : ContentPage
    {
        public int stapNmr = 1;
        public DisplayVariatyBO Variatie { get; set; }
        public DisplayFicheBO Fiche { get; set; }
        public ExercisePage(DisplayVariatyBO variatie, DisplayFicheBO fiche)
        {
            InitializeComponent();
            Fiche = fiche;
            Variatie = variatie;
            Startup();
        }

        private async void Startup()
        {
            try
            {
                imgOef.Source = Variatie.Foto[0];
                cvwUitleg.ItemsSource = Variatie.Uitleg;
                if (Variatie.Naam != "") lblOef.Text = Variatie.Naam;
                else lblOef.Text = Fiche.WorkoutName;
                imgList.Source = "list.png";
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "er is iets misgelopen bij het inladen van de pagina, als deze fout zich blijft voordoen kan men best contact opnemen met de support.", "Ok");
            }
        }


        private async void OnTapped(object sender, EventArgs e)
        {
            //   await Navigation.PushAsync();
        }

        private async void BtnChangeGroup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActiveExercise(Variatie, Fiche));
        }

        private void CvwUitleg_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {
            Debug.WriteLine(e.Direction);
        }

        async void OnListTapped(object sender, EventArgs e)
        {
            try
            {
                //if (CrossConnectivity.Current.IsConnected)
                //{

                    Application.Current.Properties["Variatie"] = Variatie.Naam;
                    await Navigation.PushAsync(new PreviousSets(Fiche));
                //}
                //else
                //{
                //    await DisplayAlert("Alert", "For this feature you need connection with the internet", "Ok");
                //}
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de vorige resultaten, als deze fout zich blijft voordoen neemt men best contact op met de support.", "Ok");
            }
        }

        private async void onImageTap(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DisplayImagePage(Variatie.Foto) { Title = "Afbeeldingen" });
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het bekijken van de foto, als deze fout zich blijft voordoen neemt men best contact op met de support.", "Ok");

            }
        }
    }
}