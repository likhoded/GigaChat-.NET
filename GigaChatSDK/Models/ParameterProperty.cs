using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GigaChatSDK.Models
{
    public class ParameterProperty
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
