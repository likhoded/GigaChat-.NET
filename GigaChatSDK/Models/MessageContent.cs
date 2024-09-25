using System;
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
        public string Role { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>

        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Идентификатор, объединяющий массив функций, переданных в запросе.
        /// </summary>
        [JsonPropertyName("functions_state_id")]
        public Guid? FunctionsStateId { get; set; }
        [JsonPropertyName("function_call")]
        public FunctionCall? FunctionCall { get; set; }

        public MessageContent(string role, string content, Guid? functionsStateId = null, FunctionCall? functionCall = null)
        {
            this.Role = role;
            this.Content = content;
            this.FunctionsStateId = functionsStateId;
            this.FunctionCall = functionCall;
        }
    }
}
