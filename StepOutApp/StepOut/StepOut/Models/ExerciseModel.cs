using Newtonsoft.Json;
using System.Collections.Generic;

namespace StepOut.Models
{
    public class ExerciseModel
    {
        [JsonProperty(propertyName: "exercises")]
        public string[] Exercises { get; set; }
    }
}
