using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayImagePage : ContentPage
    {
        public DisplayImagePage(List<ImageSource> fotos)
        {
            InitializeComponent();
            cvwUitleg.ItemsSource = fotos;
            
        }
    }
}