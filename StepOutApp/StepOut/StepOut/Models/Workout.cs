using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepOut.Models
{
    public class FicheBO
    {
        public Guid FicheId { get; set; }
        public int Workout { get; set; }
        public string WorkoutName { get; set; }
        public string TargetMuscleGroup { get; set; }
        public List<String> MusclePictures { get; set; }
        //private List<ImageSource> img;

        //public List<ImageSource> MusclePictures
        //{
        //    get { return img; }
        //    set
        //    {
        //        foreach (var i in value)
        //        {
        //            img.Add(ImageSource.FromResource($"StepOut.Assets.{i.tostring()}")); 
        //        }
        //    }
        //}
        public ScoreBO Score { get; set; }
        public List<MoeilijkheidsgradenBO> MoeilijkheidsGraden { get; set; }
    }

    public class ScoreBO
    {
        public int Excellent { get; set; }
        public int Crazy { get; set; }
        public int Insane { get; set; }
    }

    public class MoeilijkheidsgradenBO
    {
        public string Graad { get; set; }
        public List<VariatyBO> Variaties { get; set; }
    }

    public class VariatyBO
    {
        public string Naam { get; set; }
        public List<string> Uitleg { get; set; }
        
        public List<String> Foto { get; set; }
        //private List<ImageSource> img;
        //public List<ImageSource> Foto
        //{
        //    get { return img; }
        //    set
        //    {
        //        foreach (var i in value)
        //        {
        //            img.Add(ImageSource.FromResource($"StepOut.Assets.{value}"));
        //        }
        //    }
        //}

    }
}
