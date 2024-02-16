using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace GigaChatSDK.Models
{
    public class EmbeddingRequest
    {
        [JsonPropertyName("models")]
        public string models { get; set; }

        [JsonPropertyName("input")]
        public List<string> input { get; set; }

        public EmbeddingRequest(string models = "Embeddings", List<string> input = null)
        {
            List<string> inputs = new List<string>();
            this.models = models;
            this.input = input ?? inputs;
        }
    }
}
