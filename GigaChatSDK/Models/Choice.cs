using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public MessageContent? Message { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }
}
