using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace LD.Sber.GigaChatSDK.Models
{
    public class EmbeddingRequest
    {
        [JsonPropertyName("models")]
        public string Models { get; set; }

        [JsonPropertyName("input")]
        public List<string> Input { get; set; }

        public EmbeddingRequest(string models = "Embeddings", List<string> input = null)
        {
            List<string> inputs = new List<string>();
            this.Models = models;
            this.Input = input ?? inputs;
        }
    }
}
