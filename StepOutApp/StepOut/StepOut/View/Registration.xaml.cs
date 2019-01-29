using dotMorten.Xamarin.Forms;
using StepOut.Authentication;
using StepOut.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.Diagnostics;
using StepOut.Models;
using Plugin.Connectivity;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        List<string> landen = new List<string>();
        List<string> landenFiltered = new List<string>();

        public Registration()
        {
            InitializeComponent();
            LoadContent();
        }

        private async void LoadContent()
        {
            try
            {
                imgLogo.Source = "stepoutlogo.png";
                landen = getCountryNames();
                entryLand.ItemsSource = landen;
            }
            catch (Exception)
            {
                await DisplayAlert("Fout", "Er is iets misgelopen bij het openen van de registratie pagina, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        private static List<string> getCountryNames()
        {
            List<string> CultureList = new List<string>();
            CultureInfo[] getcultureinfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getculture in getcultureinfo)
            {
                RegionInfo getregioninfo = new RegionInfo(getculture.LCID);
                if (!CultureList.Contains(getregioninfo.EnglishName))
                {
                    CultureList.Add(getregioninfo.DisplayName);
                }
            }
            CultureList.Sort();
            return CultureList;
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (!IsValidEmail(entryEmail.Text) || string.IsNullOrEmpty(entryEmail.Text))
                    {
                        lblMelding.Text = "Dit is geen geldig emailadres.";
                        
                    }
                    else
                    {
                        if (!await AuthenticateManager.CheckEmailExists(entryEmail.Text))
                        {
                            if (!string.IsNullOrEmpty(entryLand.Text) && !string.IsNullOrEmpty(entryName.Text) && !string.IsNullOrEmpty(entryPass.Text))
                            {
                                await AuthenticateManager.AddUser(entryName.Text, entryPass.Text, entryEmail.Text, entryLand.Text);
                                await Navigation.PopModalAsync();
                            }
                            else
                            {
                                lblMelding.Text = "Gelieve alle velden in te vullen.";
                            }
                        }
                        else
                        {
                            lblMelding.Text = "Email adres is al in gebruik";
                        }
                    }
                }
                else await DisplayAlert("Opgepast", "Je hebt internet nodig voor je te registreren.", "Ok");
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Melding", "Er is iet misgelopen bij het registreren, als deze fout zich blijft voordoen kan men beter contact opnemen met de support.", "Ok");
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
        }

        async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void EntryLand_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                landenFiltered = new List<string>();
                foreach (string land in landen)
                {
                    if (land.ToLower().Contains(e.NewTextValue.ToLower()))
                    {
                        landenFiltered.Add(land);
                    }
                }
                entryLand.ItemsSource = landenFiltered;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het automatisch aanvullen van de landen, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}