using Plugin.Connectivity;
using StepOut.Authorize;
using StepOut.Models;
using StepOut.View;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StepOut
{
    public partial class MainPage : ContentPage
    {
        public string Email { get; set; }

        public MainPage()
        {
            InitializeComponent();
            StartUp();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async Task<bool> CheckConnection()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        private async Task StartUp()
        {
            imgLogo.Source = "stepoutlogo.png";
            lblWwVergeten.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnLblWwVergetenClicked()), });

            //if (Application.Current.Properties.ContainsKey("Current_User"))
            //{
            //    await Navigation.PushModalAsync(new NavigationPage(new MasterPage()));
            //}
            //else
            //{
            //    return;
            //}
        }

        private async void OnLblWwVergetenClicked()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected) await Navigation.PushModalAsync(new ForgetPassword());
                else await DisplayAlert("Melding", "Er is internet nodig voor deze pagina", "ok");
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Melding", "Er is een fout opgekomen bij het openen van de pagina, als deze fout zich blijft voordoen gelieve contact op te nemen met de support","Ok");
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblMelding.Text = "";
                if (await CheckConnection())
                {
                    Email = entryName.Text.ToLower();

                    string password = Regex.Replace(entryPass.Text, @"\t|\n|\r", "");
                    if (await AuthenticateManager.CheckUserLogin(Email, password))
                    {
                        Application.Current.Properties["Current_User"] = Email;
                        await Application.Current.SavePropertiesAsync();
                        var answer = await DisplayAlert("Verbinden met hartslag sensor?", "Wilt u verbinden met een hartslag sensor via bluetooth?", "Ja", "Nee");

                        if (answer) await Navigation.PushModalAsync(new ConnectBluetoothPage());
                        else
                        {
                            await Navigation.PopModalAsync();
                            //Navigation.RemovePage(this);
                        }


                        //Debug.WriteLine(Navigation.NavigationStack.Count);

                    }
                    else
                    {
                        lblMelding.Text = "Onbekende combinatie";
                    }
                }
                else
                {
                    await DisplayAlert("Waarschuwing", "Zet uw WiFi/4G aan voordat u verder gaat!", "Ok");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het inloggen, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        private async void btnRegistration_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await CheckConnection())
                {
                    await Navigation.PushModalAsync(new Registration());
                }
                else
                {
                    await DisplayAlert("Warning", "Please turn on wifi/4g to proceed", "Ok");
                    await Navigation.PushAsync(new Registration());
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de registratie pagina, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

    }
}
