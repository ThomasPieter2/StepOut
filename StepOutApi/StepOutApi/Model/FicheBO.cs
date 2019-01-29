using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    public class FicheBO
    {
        public Guid FicheId { get; set; }
        public int Workout { get; set; }
        public string WorkoutName { get; set; }
        public string TargetMuscleGroup { get; set; }
        public List<String> MusclePictures { get; set; }
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
        public List<string> Foto { get; set; }
    }



}

