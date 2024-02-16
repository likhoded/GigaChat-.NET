using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikhodedDynamics.Sber.GigaChatSDK.Models
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public MessageContent? message { get; set; }

        [JsonPropertyName("index")]
        public int index { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? finish_reason { get; set; }
    }
}
