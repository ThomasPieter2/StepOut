using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AiForms.Effects;
using StepOut.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreakExercise : ContentPage, INotifyPropertyChanged
    {
        public DisplayVariatyBO Variatie { get; set; }
        public DisplayFicheBO Fiche { get; set; }
        public int Set { get; set; }


        public BreakExercise(DisplayVariatyBO variatie, DisplayFicheBO fiche, int set)
        {
            InitializeComponent();
            Variatie = variatie;
            Set = set;
            Fiche = fiche;
            StartUp();
            //StartTimer(0, 0, 60);

            //lblReps.SetBinding(AddNumberPicker.CommandProperty, Number);
        }

        private async void StartUp()
        {
            try
            {
                imgEx.Source = Variatie.Foto[0];
                Title = "Pauze";
                if (Variatie.Naam != "") lblOef.Text = Variatie.Naam;
                else lblOef.Text = Fiche.WorkoutName;
                StartTimer(0, 1, 0);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Fout bij het inladen van de pagina", "Ok");
            }
        }

        private async void BtnChangeGroup_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (entryReps.Text == null) Application.Current.Properties["Set"] = int.Parse(entryReps.Placeholder);
                else
                {
                    Application.Current.Properties["Set"] = int.Parse( entryReps.Text.Replace(",",""));
                }
                    
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Controleer of je aantal herhalingen correct is.", "Ok");
            }
        }

        public async void StartTimer(int h, int m, int sec)
        {
            try
            {
                int hour = h;
                int mins = m;
                int counter = sec;

                TimeSpan time = new TimeSpan(0, 0, 1);

                Device.StartTimer(time, () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        counter = counter - 1;
                    //lblMiliseconds.Text = Convert.ToString((DateTime.Now - starttime).TotalMilliseconds).Substring(3,4);
                    if (counter < 0)
                        {
                            counter = 59;
                            mins = mins - 1;
                            if (mins < 0)
                            {
                                mins = 59;
                                hour = hour - 1;
                                if (hour < 0)
                                {
                                    hour = 0;
                                    mins = 0;
                                    counter = 0;
                                }
                            }
                        }

                    //lblMinutes.Text = string.Format("{0:00}:{1:00}:{2:00}", hour, mins, counter);
                    lblSeconds.Text = string.Format("{0:00}", counter);
                        lblMinutes.Text = string.Format("{0:00}", mins);
                    });
                    if (hour == 0 && mins == 0 && counter == 0)
                    {
                    //Code to call next form
                    try
                        {
                            Navigation.PopAsync();
                        }
                        catch
                        {
                            return false;
                        }

                        return false;
                    }
                    else
                    {
                        return true;
                    }

                });
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen met de timer, als deze fout blijft voorkomen gelieve contact op te nemen met de support", "Ok");
            }
        }

        public void OnClick()
        {
            Debug.WriteLine("test");
        }
    }
}