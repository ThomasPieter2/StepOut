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
    public partial class ProfileView : ContentView
    {
        public ProfileView()
        {
            InitializeComponent();
            lblEmail1.Text = "Email:";
            lblEmail.Text = Application.Current.Properties["Current_User"].ToString();
        }

        private async void btnResetPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ResetPasworPage());
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await App.Current.MainPage.DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de passwoord reset pagina, als deze fout zich blijft voordoen kan je beter contact opnemen met de suport", "Ok");
            }
        }
    }
}