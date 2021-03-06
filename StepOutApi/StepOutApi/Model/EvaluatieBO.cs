﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    public class EvaluatieBO
    {
        public List<int> data { get; set; }
        public string Gebruik { get { return "Evaluatie"; } }
        public string Email { get; set; }
        public string WorkoutNaam { get; set; }
        public string Graad { get; set; }
        public string Variatie { get; set; }
        public DateTime Datum { get; set; }
        public int Set1 { get; set; }
        public int Set2 { get; set; }
        public int Set3 { get; set; }
        public int Moeilijkheid { get; set; }
    }
}
