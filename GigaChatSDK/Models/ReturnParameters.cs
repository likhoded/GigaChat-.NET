using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class ReturnParameters
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, ReturnProperty> Properties { get; set; }

        [JsonPropertyName("required")]
        public List<string> Required { get; set; }
    }
}
