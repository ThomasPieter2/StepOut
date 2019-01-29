using Plugin.Connectivity;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StepOut.BluetoothConnect;
using Plugin.BLE.Abstractions.Contracts;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public List<DisplayFicheBO> Fiches { get; set; }
        public MasterPage()
        {
            InitializeComponent();
            StartUp();
        }

        protected override void OnDisappearing()
        {
            //lblEmail.Text = Application.Current.Properties["Current_User"].ToString();
        }


        //private void OnPresentedChanged(object sender, EventArgs e) { lblEmail.Text = Application.Current.Properties["Current_User"].ToString(); }

        private async void StartUp()
        {
            try
            {
                if (!Application.Current.Properties.ContainsKey("Current_User"))
                {
                    //Application.Current.Properties["Current_User"] = 0;

                    await Navigation.PushModalAsync(new MainPage());

                }

                Fiches = await StepOutManager.GetAllFiches();
                detailPage.Title = "Random";
                detailPage.Content = new MuscleGroupPage(Fiches);
                imgMuscle.Source = "muscle.png";
                imgProfile.Source = "user.png";
                imgShuffle.Source = "shuffle2.png";
                imgPlaceholder.Source = "placeholder.png";
                imgSignout.Source = "signout.png";
                imgBluethooth.Source = "bluethooth.png";
                
                
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", ex.Message , "OK");
            }
        }

        protected override async void OnAppearing()
        {
            //base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("Current_User")) lblEmail.Text = Application.Current.Properties["Current_User"].ToString();


            //else
            //{
            //    //this.Navigation.RemovePage (this.Navigation.NavigationStack [this.Navigation.NavigationStack.Count - 2]);

            //    //Debug.WriteLine("Currently logged in user:" + Application.Current.Properties["Current_User"]);
            //}
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }

        async void OnRandomTapped(object sender, EventArgs e)
        {
            try
            {
                this.IsPresented = false;
                detailPage.Title = "Random";
                detailPage.Navigation.PopToRootAsync();
                detailPage.Content = new MuscleGroupPage(Fiches);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het tonen van de random oefening, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
        }

        async void OnExercisesTapped(object sender, EventArgs e)
        {
            try
            {
                this.IsPresented = false;
                detailPage.Title = "Oefeningen";
                detailPage.Navigation.PopToRootAsync();
                detailPage.Content = new ExercisesView(Fiches);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het tonen van de oefeningen, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
        }

        async void OnProfileTapped(object sender, EventArgs e)
        {
            try
            {
                this.IsPresented = false;
                detailPage.Title = "Profiel";
                detailPage.Navigation.PopToRootAsync();
                detailPage.Content = new ProfileView();
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het tonen van de oefeningen, als deze fout zich blijft voordoeng neemt men best contact op met de support", "OK");
            }
        }

        public async void OnSignOutTapped(object sender, EventArgs e)
        {
            try
            {
                this.IsPresented = false;
                if(Application.Current.Properties.ContainsKey("Current_User")) Application.Current.Properties.Remove("Current_User");

                await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
                CacheManager.RemoveAllSyncEvaluations();
                if (CrossConnectivity.Current.IsConnected) await StepOutManager.SyncAllFichesBO();
                await Application.Current.SavePropertiesAsync();
                StepOutManager.CurrentUser = "";

                BluetoothManager bluetoothManager = new BluetoothManager();
                IDevice device = CacheProvidor.Get<IDevice>("device");
                if(device != null) await bluetoothManager.Disconnect(device, CrossBluetoothLE.Current.Adapter);
                //await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
                //detailPage.Title = "Profile";
                //detailPage.Content = new ProfileView();
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het uitloggen, als deze fout zich blijft voordoen neemt men best contact op met de support.", "OK");
            }
        }

        private async void OnBluethoothTap(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ConnectBluetoothPage());
            }
            catch (Exception ex)
            {

                await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de bleuthooth pagina, als deze fout zich blijft voordoen neemt men best contact op met de support.", "OK");
            }
        }

        //private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.IsPresented = false;
        //        detailPage.Title = "Wachtwoord wijzigen";
        //        //detailPage.Content = new ResetPasworPage();
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de bleuthooth pagina, als deze fout zich blijft voordoen neemt men best contact op met de support.", "OK");
        //    }
        //}
    }
}