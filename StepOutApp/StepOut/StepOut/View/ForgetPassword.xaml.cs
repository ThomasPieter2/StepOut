using Plugin.Connectivity;
using StepOut.Authorize;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgetPassword : ContentPage
	{
		public ForgetPassword ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (await AuthenticateManager.CheckEmailExists(entryEmail.Text))
                    {
                        await AuthenticateManager.GetPasswordReset(entryEmail.Text);
                        await Navigation.PopModalAsync();
                    }
                    else lblMelding.Text = "Het email adress is niet in gebruik";
                }
                else await DisplayAlert("Opgepast", "Je hebt internet nodig om je wachtwoord te reseten", "Ok");
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Melding", "Er is iets misgelopen bij het aanvragen vaan een nieuw passwoord", "Ok");
            }
        }
    }
}