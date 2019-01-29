using Plugin.Connectivity;
using StepOut.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StepOut
{
    public partial class App : Application
    {

        public String LoggedInUser { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MasterPage());
            //MainPage = new NavigationPage(new MainPage());
        }

        protected async override void OnStart()
        {
            if (Current.Properties.ContainsKey("Current_User"))
            {
                var answer = await MainPage.DisplayAlert("Verbinden met hartslag sensor?", "Wilt u verbinden met een hartslag sensor via bluetooth?", "Ja", "Nee");
                if (answer)
                {
                    //await MainPage.Navigation.PopModalAsync();
                    await MainPage.Navigation.PushModalAsync(new ConnectBluetoothPage());
                }
                else return;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
