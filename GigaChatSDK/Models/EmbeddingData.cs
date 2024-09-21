using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class EmbeddingData
    {
        [JsonPropertyName("object")]
        public string @object { get; set; }

        [JsonPropertyName("embedding")]
        public List<float> embedding { get; set; }

        [JsonPropertyName("index")]
        public int index { get; set; }

        public EmbeddingData(string @object = "embedding",List<float> embedding = null, int index = 0)
        {
            List<float> embeddings = new List<float>();
            this.@object = @object;
            this.embedding = embedding ?? embeddings;
            this.index = index;
        }
    }
}
