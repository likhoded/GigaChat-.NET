using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Security;
using System.Text.Json.Serialization;
using System.Text.Json;
using LikhodedDynamics.Sber.GigaChatSDK.Models;
using static LikhodedDynamics.Sber.GigaChatSDK.Models.MessageQuery;

namespace LikhodedDynamics.Sber.GigaChatSDK
{
    public class GigaChat
    {
        private string URL = "https://gigachat.devices.sberbank.ru/api/v1/";
        public Token Token { get; set; }
        /// <summary>
        /// Авторизационные данные
        /// </summary>
        private string? secretKey { get; set; }

        /// <summary>
        /// Версия API, к которой предоставляется доступ. Нужное значение параметра scope вы найдете в личном кабинете после создания проекта.
        /// GIGACHAT_API_PERS — доступ для физических лиц.
        /// GIGACHAT_API_CORP — доступ для юридических лиц.
        /// </summary>
        private bool isCommercial = false;
        /// <summary>
        /// true - включает игнорирование сертификатов безопасности
        /// Необходимо для систем имеющих проблемы с сертификатами МинЦифр.
        /// </summary>
        private bool ignoreTLS = true;

        public GigaChat(string secretKey, bool isCommercial, bool ignoreTLS)
        {
            this.secretKey = secretKey;
            this.isCommercial = isCommercial;
            this.ignoreTLS = ignoreTLS;
        }
        /// <summary>
        /// Генерация Токена
        /// </summary>
        /// <returns>Token.</returns>
        public async Task<Token> CreateTokenAsync()
        {
            try
            {
                Console.WriteLine("Creating Token");
                HttpClientHandler clientHandler = new HttpClientHandler();

                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }
                 
                using (var client = new HttpClient(clientHandler))
                {
                   
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ngw.devices.sberbank.ru:9443/api/v2/oauth");
                    var httpClientHandler = new HttpClientHandler();

                    request.Headers.Add("Authorization", "Bearer " + secretKey);
                    request.Headers.Add("RqUID", Guid.NewGuid().ToString());
                    if (isCommercial == true)
                    {
                        request.Content = new StringContent("scope=GIGACHAT_API_CORP");
                    }
                    else
                    {
                        request.Content = new StringContent("scope=GIGACHAT_API_PERS");
                    }
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Token = JsonSerializer.Deserialize<Token>(responseBody);
                    
                    return Token;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// Отправление запроса к модели
        /// </summary>
        /// <returns>Возвращает Response с ответом модели с учетом переданных сообщений..</returns>
        /// <param name="query">Запрос к модели.</param>
        public async Task<Response> CompletionsAsync(MessageQuery query)
        {
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                string responseBody;
                Response DeserializedResponse;

                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }
                using (var client = new HttpClient(clientHandler))
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL + "chat/completions");

                    request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);
                    
                    request.Content = new StringContent(JsonSerializer.Serialize(query));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    DeserializedResponse = JsonSerializer.Deserialize<Response>(responseBody);
                    Console.WriteLine(responseBody);
                    client.Dispose();
                }
                return DeserializedResponse;
            }
            else
            {
                return null;
            }
        }
        public async Task<Model> ModelAsync(string model)
        {
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                string responseBody;
                Model? DeserializedModel;

                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }

                using (var client = new HttpClient(clientHandler))
                {

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL + "models/" + model);

                    request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    DeserializedModel = JsonSerializer.Deserialize<Model>(responseBody);

                    client.Dispose();
                }
                return DeserializedModel;
            }
            else
            {
                return null;
            }
        }

    }
}
