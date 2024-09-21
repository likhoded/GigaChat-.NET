using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int prompt_tokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int completion_tokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int total_tokens { get; set; }
    }
}
