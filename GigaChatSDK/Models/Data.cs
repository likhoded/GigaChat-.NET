using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
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
