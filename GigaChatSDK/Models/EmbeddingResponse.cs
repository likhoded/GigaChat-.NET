using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GigaChatSDK.Models
{
    public class EmbeddingResponse
    {
        [JsonPropertyName("object")]
        public string? @object { get; set; }

        [JsonPropertyName("data")]
        public List<EmbeddingData>? data { get; set; }

        [JsonPropertyName("model")]
        public string? model { get; set; }
    }
}
