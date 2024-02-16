using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LikhodedDynamics.Sber.GigaChatSDK.Models
{
    public class Token
    {
        /// <summary>
        /// Токен
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// Время истечения в Unix
        /// </summary>
        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }
        [JsonConstructor]
        public Token() { }
        
        public Token(string access_token, long expires_at) { AccessToken = access_token; ExpiresAt = expires_at; }
    }
}
