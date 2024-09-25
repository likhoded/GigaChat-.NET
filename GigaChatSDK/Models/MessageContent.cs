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

        /// <summary>
        /// Идентификатор, объединяющий массив функций, переданных в запросе.
        /// </summary>
        [JsonPropertyName("functions_state_id")]
        public Guid? functionsStateId { get; set; }
        [JsonPropertyName("function_call")]
        public FunctionCall? functionCall { get; set; }

        public MessageContent(string role, string content, Guid? functionsStateId = null, FunctionCall? functionCall = null)
        {
            this.role = role;
            this.content = content;
            this.functionsStateId = functionsStateId;
            this.functionCall = functionCall;
        }
    }
}
