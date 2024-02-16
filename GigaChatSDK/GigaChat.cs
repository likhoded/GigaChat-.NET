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
using GigaChatSDK.Models;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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
        private bool isCommercial { get; set; } = false;
        /// <summary>
        /// true - включает игнорирование сертификатов безопасности
        /// Необходимо для систем имеющих проблемы с сертификатами МинЦифр.
        /// </summary>
        private bool ignoreTLS { get; set; } = true;

        private bool saveImage { get; set; } = false;

        public string SaveDirectory { get; set; } = Directory.GetCurrentDirectory();
        private long? ExpiresAt { get; set; }
        public GigaChat(string secretKey, bool isCommercial, bool ignoreTLS, bool saveImage)
        {
            this.secretKey = secretKey;
            this.isCommercial = isCommercial;
            this.ignoreTLS = ignoreTLS;
            this.saveImage = saveImage;
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
                    ExpiresAt = Token.ExpiresAt;
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
        /// <returns>Response с ответом модели с учетом переданных сообщений..</returns>
        /// <param name="query">Запрос к модели в виде объекта запроса.</param>
        public async Task<Response?> CompletionsAsync(MessageQuery query)
        {
            if(ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
            {
                await CreateTokenAsync();
            }
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                string responseBody;
                Response? DeserializedResponse;

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
        /// <summary>
        /// Отправление запроса к модели
        /// </summary>
        /// <returns>Response с ответом модели с учетом переданных сообщений..</returns>
        /// <param name="_message">Запрос к модели в виде 1 строки с непосредственно с запросом.</param>
        public async Task<Response?> CompletionsAsync(string _message)
        {
            if (ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
            {
                await CreateTokenAsync();
            }
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                string responseBody;
                Response? DeserializedResponse;

                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }

                MessageQuery query = new MessageQuery();
                query.messages.Add(new MessageContent("user", _message));

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

        /// <summary>
        /// Создание эмбеддинга
        /// Доступно для моделей Plus и Pro
        /// </summary>
        /// <returns>EmbeddingResponse с векторным представлением соответствующих текстовых запросов. 
        /// Индекс объекта с векторным представлением (поле index) соответствует индексу строки в массиве input запроса.</returns>
        /// <param name="_request">Запрос к модели в виде объекта запроса.</param>
        public async Task<EmbeddingResponse?> EmbeddingAsync(EmbeddingRequest _request)
        {
            if (ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
            {
                await CreateTokenAsync();
            }
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                string responseBody;
                EmbeddingResponse? DeserializedResponse;
                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }
                using (var client = new HttpClient(clientHandler))
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL + "embeddings");

                    request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);

                    request.Content = new StringContent(JsonSerializer.Serialize(_request));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    DeserializedResponse = JsonSerializer.Deserialize<EmbeddingResponse>(responseBody);
                    
                    client.Dispose();
                }
                return DeserializedResponse;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получение изображения по идентификатору
        /// </summary>
        /// <returns>Возвращает файл изображения в бинарном представлении, в формате JPG..</returns>
        /// <param name="fileId">Идентификатор изображения</param>
        public async Task<byte[]?> GetImageAsByteAsync(string fileId)
        {
            if (Token != null)
            {
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
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL + $"/files/{fileId}/content");

                    request.Headers.Add("Accept", "application/jpg");
                    request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    if (saveImage == true)
                    {
                        System.IO.File.WriteAllBytes(Path.Combine(SaveDirectory, fileId + ".jpg"), await response.Content.ReadAsByteArrayAsync());
                    }
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получение списка моделей
        /// </summary>
        /// <returns>Model</returns>
        /// <param name="model">Идентификатор изображения</param>
        public async Task<Model?> ModelsAsync()
        {
            if (ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
            {
                await CreateTokenAsync();
            }
            if (Token != null)
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                Model? DeserializedModel = null;

                if (ignoreTLS == true)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => { return true; };
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                }

                using (var client = new HttpClient(clientHandler))
                {

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL + "models/");

                    request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
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

        public string? GetFileId(string _messageContent)
        {
            string pattern = "<img\\s+src=\"(.*?)\"";
            Match match = Regex.Match(_messageContent, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }
    }
}
