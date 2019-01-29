using Newtonsoft.Json;
using System;

namespace StepOut.Models
{
    public class ExerciseInfoModel
    {
        public string Email { get; set; }

        [JsonProperty(propertyName: "upload-time")]
        public DateTime UploadTime { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "start-time")]
        public string StartTime { get; set; }

        [JsonProperty(propertyName: "duration")]
        public string Duration { get; set; }

        [JsonProperty(propertyName: "heart-rate")]
        public HR HeartRate { get; set; }

        [JsonProperty(propertyName: "training-load")]
        public float TrainingLoad { get; set; }

        [JsonProperty(propertyName: "sport")]
        public string Sport { get; set; }
    }

    public class HR
    {
        [JsonProperty(propertyName: "average")]
        public int Average { get; set; }

        [JsonProperty(propertyName: "maximum")]
        public int Maximum { get; set; }
    }

}
