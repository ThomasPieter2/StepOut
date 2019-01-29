using Microcharts;
using SkiaSharp;
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
    public partial class SetOverviewPage : ContentPage
    {
        public EvaluatieBO Evaluatie { get; set; }
        public SetOverviewPage(EvaluatieBO eval)
        {
            InitializeComponent();
            Evaluatie = eval;

            imgHeart.Source = "heart.png";
            imgHeart2.Source = "heart.png";
            imgHeart3.Source = "heart.png";
            StartUp();
        }

        private async void StartUp()
        {
            try
            {
                imgHeart.Source = "heart.png";
                imgHeart2.Source = "heart.png";
                imgHeart3.Source = "heart.png";

                if (Evaluatie.data.Count != 0)
                {
                    List<Microcharts.Entry> lstentries = new List<Microcharts.Entry>();
                    foreach (var item in Evaluatie.data)//data.HeartRate)
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
                    chart.MinValue = Evaluatie.data.Min();
                    this.chartView.Chart = chart;

                    lblMax.Text = Evaluatie.data.Max().ToString();
                    lblMaxZone.Text = Evaluatie.data.Max().ToString();
                    lblMin.Text = Evaluatie.data.Min().ToString();
                    lblMinZone.Text = Evaluatie.data.Min().ToString();
                    lblAvg.Text = ((int)Evaluatie.data.Average()).ToString();
                }
                lblSet1.Text = Evaluatie.Set1.ToString();
                lblSet2.Text = Evaluatie.Set2.ToString();
                lblSet3.Text = Evaluatie.Set3.ToString();
                Title = "Evaluatie " + Evaluatie.Datum.Date.ToString("dd/MM/yyyy");
                lblDatum.Text = Evaluatie.Datum.ToString();
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het tonen van de hartslag grafiek, als deze fout zich blijft voordoen neemt men best contact op met de support", "OK");
            }
        }


        private void BtnChangeGroup_Clicked(object sender, EventArgs e)
        {

        }

    }
}