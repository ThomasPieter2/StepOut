using Plugin.Connectivity;
using StepOut.Authorize;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasworPage : ContentPage
    {
        public MasterPage Master { get; set; }
        public ResetPasworPage()
        {
            InitializeComponent();
            //Master = master;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    lblMelding.Text = "";
                    string password = Regex.Replace(entryOldPass.Text, @"\t|\n|\r", "");
                    if (await AuthenticateManager.CheckUserLogin(Application.Current.Properties["Current_User"].ToString(), password))
                    {
                        if (entrynewPass2.Text == entrynewPass1.Text)
                        {
                            await AuthenticateManager.ResetPassword(Application.Current.Properties["Current_User"].ToString(), entrynewPass2.Text);
                            await Navigation.PopAsync();
                        }
                        else lblMelding.Text = "De 2 opgegeven nieuwe wachtwoorden zijn niet gelijk.";
                    }
                    else lblMelding.Text = "Oude wachtwoord is niet correct";
                }
                else await App.Current.MainPage.DisplayAlert("Opgepast", "voor je wachtwoord opnieuw in te stellen heb je internet nodig.", "Ok");
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Melding","Er is iets foutgelopen bij het herinstellen van het wachtwoord, controleer of je niet een spatie teveel hebt. Als dit niet het geval is en het werkt nog niet neem je best even contact op met de support.", "ok");
            }
        }
    }
}