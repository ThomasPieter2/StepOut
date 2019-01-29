using Newtonsoft.Json;
using System;

namespace StepOut.Models
{
    public class UserModel
    {
        [JsonProperty(propertyName: "polar-user-id")]
        public int UserId { get; set; }

        [JsonProperty(propertyName: "member-id")]
        public string MemberId { get; set; }

        [JsonProperty(propertyName: "registration-date")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty(propertyName: "first-name")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "last-name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "birthdate")]
        public string BirthDate { get; set; }

        [JsonProperty(propertyName: "gender")]
        public string Gender { get; set; }

        [JsonProperty(propertyName: "weight")]
        public double Weight { get; set; }

        [JsonProperty(propertyName: "height")]
        public double Height { get; set; }

        [JsonProperty(propertyName: "extra-info")]
        public object[] ExtraInfo { get; set; }
    }
}