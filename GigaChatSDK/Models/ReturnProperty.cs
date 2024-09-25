using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class ReturnProperty
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
