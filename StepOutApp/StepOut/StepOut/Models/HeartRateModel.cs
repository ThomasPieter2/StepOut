using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class HeartRateModel
{
    [JsonProperty(propertyName: "graphId")]
    public string GraphId { get; set; }
    [JsonProperty(propertyName:"data")]
    public List<int> HeartRate { get; set; }
}


