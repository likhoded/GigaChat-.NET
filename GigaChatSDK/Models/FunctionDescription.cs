using GigaChatSDK.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LD.Sber.GigaChatSDK.Models
{
    public class FunctionDescription
    {
        /// <summary>
        /// Название функции
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Описание функции
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Параметры функции
        /// </summary>
        [JsonPropertyName("parameters")]
        public FunctionParameters? Parameters { get; set; }

        /// <summary>
        /// Примеры использования
        /// </summary>
        [JsonPropertyName("few_shot_examples")]
        public List<FewShotExample>? FewShotExamples { get; set; }

        /// <summary>
        /// Параметры возврата
        /// </summary>
        [JsonPropertyName("return_parameters")]
        public ReturnParameters? ReturnParameters { get; set; }
    }
}
