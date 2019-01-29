using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    public class FicheV2
    {
        public String Gebruik { get { return "Fiche"; }}
        public Guid FicheId { get; set; }
        public int Workout { get; set; }
        public string WorkoutName { get; set; }
        public string TargetMuscleGroup { get; set; }
        public List<String> MusclePictures { get; set; }
        public ScoreBO Score { get; set; }
        public List<MoeilijkheidsgradenBO> MoeilijkheidsGraden { get; set; }
    }
}
