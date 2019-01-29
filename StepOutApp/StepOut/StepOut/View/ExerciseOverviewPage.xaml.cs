using Microcharts;
using Plugin.Connectivity;
using SkiaSharp;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseOverviewPage : ContentPage
    {
        public EvaluatieBO Evaluatie { get; set; }
        public List<int> listHr = new List<int>();
        public ExerciseOverviewPage(List<int> listhr, EvaluatieBO eval)
        {
            InitializeComponent();
            Evaluatie = eval;
            listHr = listhr;
            Title = "Overzicht";
            StartUp();
            imgHeart.Source = "heart.png";
            imgHeart2.Source = "heart.png";
            imgHeart3.Source = "heart.png";
        }


        protected override bool OnBackButtonPressed()
        {
            //int count = Navigation.NavigationStack.Count;
            //Navigation.PopToRootAsync();
            //Navigation.PopAsync();
            return false;
        }

        protected override void OnDisappearing()
        {
            //Navigation.PopToRootAsync();
            if(Navigation.NavigationStack.Count != 0)
            {
                Navigation.PopToRootAsync();
            }
            Debug.WriteLine(Navigation.NavigationStack.Count);
            //this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 4]);
            //Navigation.PopToRootAsync();
            //Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 4];
        }

        private async void StartUp()
        {
            try
            {

                Evaluatie.data = listHr;
                if (CrossConnectivity.Current.IsConnected) await StepOutManager.AddEvaluations(Evaluatie);
                else await StepOutManager.WriteLocal(Evaluatie);
                lblAangeradenNiveau.Text = Application.Current.Properties[Evaluatie.WorkoutNaam].ToString();
                lblHuidigNiveau.Text = Application.Current.Properties["Difficulty"].ToString();

                if (listHr.Count != 0)
                {
                    List<Microcharts.Entry> lstentries = new List<Microcharts.Entry>();
                    foreach (var item in listHr)
                    {
                        if (item > 50 && item < 180)
                        {
                            if (item < 100)
                            {
                                lstentries.Add(new Microcharts.Entry(item)
                                {
                                    Color = SKColor.Parse("#FFDC00")
                                });
                            }
                            else if (item > 120)
                            {
                                lstentries.Add(new Microcharts.Entry(item)
                                {
                                    Color = SKColor.Parse("#FF4136")
                                });
                            }
                            else
                            {
                                lstentries.Add(new Microcharts.Entry(item)
                                {
                                    Color = SKColor.Parse("#FF851B")
                                });
                            }
                        }
                    }
                    var chart = new LineChart() { Entries = lstentries, LineAreaAlpha = 1, LineMode = LineMode.Straight };
                    chart.PointMode = PointMode.None;
                    chart.LineSize = 6;
                    chart.MinValue = listHr.Min();
                    chartView.Chart = chart;

                    lblMax.Text = listHr.Max().ToString();
                    lblMaxZone.Text = listHr.Max().ToString();
                    lblMin.Text = listHr.Min().ToString();
                    lblMinZone.Text = listHr.Min().ToString();
                    lblAvg.Text = ((int)listHr.Average()).ToString();
                    
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het inladen van de pagina, als deze fout zich blijft voordoen kan men best beter contact opnemen met de suport.", "Ok");
            }
        }

        private async void BtnChangeGroup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
