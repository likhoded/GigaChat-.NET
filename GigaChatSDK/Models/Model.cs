using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class Model
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("object")]
        public string @object { get; set; }
        [JsonPropertyName("owned_by")]
        public string owned_by { get; set; }
    }
}
