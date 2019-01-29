using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepOut.Models
{
    public class DisplayFicheBO
    {
        public Guid FicheId { get; set; }
        public int Workout { get; set; }
        public string WorkoutName { get; set; }
        public string TargetMuscleGroup { get; set; }
        public List<ImageSource> MusclePictures { get; set; }
        public ScoreBO Score { get; set; }
        public List<DisplayMoeilijkheidsgradenBO> MoeilijkheidsGraden { get; set; }
    }

    public class DisplayMoeilijkheidsgradenBO
    {
        public string Graad { get; set; }
        public List<DisplayVariatyBO> Variaties { get; set; }
    }

    public class DisplayVariatyBO
    {
        public string Naam { get; set; }
        public List<string> Uitleg { get; set; }
        public List<ImageSource> Foto { get; set; }
        
        //private ImageSource img;
        //
        //public ImageSource Image
        //{
        //    get { return img; }
        //    set
        //    {
        //        if(typeof(img) == ImageSource)
        //        img = value;
        //    }
        //}

    }

}
