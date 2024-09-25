using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class FewShotExample
    {
        [JsonPropertyName("request")]
        public string? Request { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object>? Params { get; set; }
    }
}
