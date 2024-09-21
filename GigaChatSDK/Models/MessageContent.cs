using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    /// <summary>
    /// В классе MessageContent хранятся все данные конкретного сообщения
    /// </summary>
    public class MessageContent
    {
        /// <summary>
        /// Роль отправителя
        /// </summary>

        [JsonPropertyName("role")]
        public string role { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>

        [JsonPropertyName("content")]
        public string content { get; set; }

        public MessageContent(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }
}
