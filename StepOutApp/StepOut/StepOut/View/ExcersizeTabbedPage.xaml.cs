using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepOut.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcersizeTabbedPage : TabbedPage
    {
        public DisplayFicheBO Data { get; set; }

        public ExcersizeTabbedPage(DisplayFicheBO data, string difficulty)
        {
            InitializeComponent();
            Data = data;
            Startup(difficulty);
        }

        private async void Startup(string difficulty)
        {
            try
            {
                Title = Data.TargetMuscleGroup;
                List<DisplayVariatyBO> variaties = GetVariatiesForDifficulty(Data.MoeilijkheidsGraden, difficulty);
                CreateTabbedPages(variaties);
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets foutgelopen bij het ophalen van de informatie, als deze fout zich blijft voordoen kan men best contact opnemen met de support.", "Ok");
            }
        }

        private async void CreateTabbedPages(List<DisplayVariatyBO> variaties)
        {
            try
            {
                if (variaties.Count == 1) this.Children.Add(new ExercisePage(variaties[0], Data) { Title = Data.TargetMuscleGroup });
                else
                {
                    foreach (DisplayVariatyBO i in variaties)
                    {
                        if (i.Naam == "") this.Children.Add(new ExercisePage(i, Data) { Title = Data.WorkoutName });
                        else this.Children.Add(new ExercisePage(i, Data) { Title = i.Naam });

                    }
                }
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets foutgelopen bij het aanmaken van de verschillende tabs voor de oefening. Als deze fout zich blijft voordoen kan men best contact opnemen met de support.", "Ok");
            }
            
        }

        private List<DisplayVariatyBO> GetVariatiesForDifficulty(List<DisplayMoeilijkheidsgradenBO> ex, string difficulty)
        {
            List<DisplayVariatyBO> variaties = new List<DisplayVariatyBO>();
            foreach (DisplayMoeilijkheidsgradenBO i in ex)
            {
                if (i.Graad == difficulty)
                {
                    foreach (DisplayVariatyBO v in i.Variaties)
                    {
                        variaties.Add(v);
                    }
                }
            }
            return variaties;
        }
    }
}