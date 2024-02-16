using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikhodedDynamics.Sber.GigaChatSDK.Models
{
    /// <summary>
    /// В модели MessageQuery хранятся все данные запроса, в случае, если вам необходимо только передать, 
    /// только сообщения, не указывайте значения остальных аргументов при иницилизации класса
    /// </summary>
    public class MessageQuery
    {
        /// <summary>
        /// Название модели
        /// По умолчанию: GigaChat:latest
        /// </summary>
        [JsonPropertyName("model")]
        public string model { get; set; }
        /// <summary>
        /// Массив передаваемых сообщений
        /// По умолчанию: -
        /// </summary>

        [JsonPropertyName("messages")]
        public List<MessageContent> messages { get; set; }
        /// <summary>
        /// Температура выборки в диапазоне от ноля до двух. Чем выше значение, тем более случайным будет ответ модели.
        /// По умолчанию: 0.87
        /// </summary>

        [JsonPropertyName("temperature")]
        public float temperature { get; set; }
        /// <summary>
        /// Параметр используется как альтернатива temperature. 
        /// Задает вероятностную массу токенов, которые должна учитывать модель. 
        /// Так, если передать значение 0.1, модель будет учитывать только токены, чья вероятностная масса входит в верхние 10%.
        /// По умолчанию: 0.47
        /// </summary>
        [JsonPropertyName("top_p")]
        public float top_p { get; set; }
        /// <summary>
        /// Количество вариантов ответов, которые нужно сгенерировать для каждого входного сообщения
        /// По умолчанию: 1
        /// </summary>
        [JsonPropertyName("n")]
        public long n { get; set; }
        /// <summary>
        /// Указывает, что сообщения надо передавать по частям в потоке.
        /// По умолчанию: false
        /// </summary>
        [JsonPropertyName("stream")]
        public bool stream { get; set; }
        /// <summary>
        /// Максимальное количество токенов, которые будут использованы для создания ответов
        /// По умолчанию: 512
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public long max_tokens { get; set; }
        public MessageQuery(List<MessageContent>? messages = null, string model = "GigaChat:latest",  float temperature = 0.87f, float top_p = 0.47f, long n = 1, bool stream = false, long max_tokens = 512)
        {
            List<MessageContent> Contents = new List<MessageContent>();
            this.model = model;
            this.messages = messages ?? Contents;
            this.temperature = temperature;
            this.top_p = top_p;
            this.n = n;
            this.stream = stream;
            this.max_tokens = max_tokens;
        }
    }
}
