using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikhodedDynamics.Sber.GigaChatSDK.Models
{
    public class Data
    {
        [JsonPropertyName("data")]
        public MessageQuery data { get; set; }

        public Data()
        {
            data = new MessageQuery();
        }
    }
}
