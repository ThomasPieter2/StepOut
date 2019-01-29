using Newtonsoft.Json;

namespace StepOut.Models
{
    public class TransactionExerciseModel
    {
        [JsonProperty(propertyName: "transaction-id")]
        public int TransctionId { get; set; }
        [JsonProperty(propertyName: "resource-uri")]
        public string ResourceUri { get; set; }
    }
}
