using Newtonsoft.Json;
using System.Collections.Generic;

namespace StepOut.Models
{
    public class NotificationModel
    {
        [JsonProperty(propertyName: "available-user-data")]
        public AvailableUserDatas[] AvailableUserData { get; set; }
    }
    public class AvailableUserDatas
    {
        [JsonProperty(propertyName: "user-id")]
        public int UserId { get; set; }
        [JsonProperty(propertyName: "data-type")]
        public string Datatype { get; set; }
        [JsonProperty(propertyName: "url")]
        public string Url { get; set; }
    }
}
