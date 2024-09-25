using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class Model
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("object")]
        public string? @object { get; set; }
        [JsonPropertyName("owned_by")]
        public string? OwnedBy { get; set; }
    }
}
