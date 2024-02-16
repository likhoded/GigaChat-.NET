using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikhodedDynamics.Sber.GigaChatSDK.Models
{
    public class Response
    {
        [JsonPropertyName("choices")]
        public List<Choice>? choices { get; set; }

        [JsonPropertyName("created")]
        public int created { get; set; }

        [JsonPropertyName("model")]
        public string? model { get; set; }

        [JsonPropertyName("usage")]
        public Usage? usage { get; set; }

        [JsonPropertyName("object")]
        public string? @object { get; set; }
    }
}
