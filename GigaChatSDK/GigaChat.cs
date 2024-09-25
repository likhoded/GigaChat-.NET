using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;

using LD.Sber.GigaChatSDK.Utils;
using LD.Sber.GigaChatSDK.Models;
using LD.Sber.GigaChatSDK.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Linq;

namespace LD.Sber.GigaChatSDK
{
    public class GigaChat : IGigaChat
    {

        private static readonly string baseUrl = "https://gigachat.devices.sberbank.ru/api/v1/";

        private readonly bool saveImage;
        private string saveDirectory { get; set; } = Directory.GetCurrentDirectory();

        private readonly ITokenService tokenService;
        private readonly IHttpService httpService;

        public GigaChat(ITokenService tokenService, IHttpService httpService, bool saveImage, string? SaveDirectory = null)
        {
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            this.saveDirectory = saveDirectory ?? Directory.GetCurrentDirectory();
            this.saveImage = saveImage;
        }
        /// <summary>
        /// Получение нового токена
        /// </summary>
        /// <returns>Возвращает экземпляр токена</returns>
        public async Task<Token> CreateTokenAsync()
        {
            return await tokenService.CreateTokenAsync();
        }
        /// <summary>
        /// Отправление запроса к модели
        /// </summary>
        /// <returns>Response с ответом модели с учетом переданных сообщений..</returns>
        /// <param name="query">Запрос к модели в виде объекта запроса.</param>
        public async Task<Response?> CompletionsAsync(MessageQuery query)
        {
            await ValidateToken();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}chat/completions") 
            {
                Headers = { { "Authorization", $"Bearer {tokenService.Token.AccessToken}" } },
                Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json")
            };

            var responseBody = await httpService.SendAsync(request);

            return JsonSerializer.Deserialize<Response>(responseBody);
        }
        /// <summary>
        /// Отправление запроса к модели
        /// </summary>
        /// <returns>Response с ответом модели с учетом переданных сообщений..</returns>
        /// <param name="message">Запрос к модели в виде 1 строки с непосредственно с запросом.</param>
        public async Task<Response?> CompletionsAsync(string role, string message)
        {
            await ValidateToken();
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            var query = new MessageQuery();
            query.messages.Add(new MessageContent(role, message));
            return await CompletionsAsync(query);
        }
        /// <summary>
        /// Создание эмбеддинга
        /// Доступно не для всех моделей
        /// </summary>
        /// <returns>EmbeddingResponse с векторным представлением соответствующих текстовых запросов. 
        /// Индекс объекта с векторным представлением (поле index) соответствует индексу строки в массиве input запроса.</returns>
        /// <param name="request">Запрос к модели в виде объекта запроса.</param>
        public async Task<EmbeddingResponse?> EmbeddingAsync(EmbeddingRequest request)
        {
            try
            {
                await ValidateToken();

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}embeddings");
                httpRequest.Headers.Add("Authorization", $"Bearer {tokenService.Token.AccessToken}");
                httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var responseBody = await httpService.SendAsync(httpRequest);
                return JsonSerializer.Deserialize<EmbeddingResponse>(responseBody);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Ошибка десериализации ответа.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Неизвестная ошибка при получении встраивания: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Получение изображения по идентификатору
        /// </summary>
        /// <returns>Возвращает файл изображения в бинарном представлении, в формате JPG..</returns>
        /// <param name="fileId">Идентификатор изображения</param>
        public async Task<byte[]?> GetImageAsByteAsync(string fileId)
        {
            try
            {
                await ValidateToken();

                if (string.IsNullOrWhiteSpace(fileId))
                    throw new ArgumentException("File ID cannot be empty.", nameof(fileId));

                var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/files/{fileId}/content");
                request.Headers.Add("Authorization", $"Bearer {tokenService.Token.AccessToken}");

                var responseBody = await httpService.SendAsync(request);
                var fileData = Convert.FromBase64String(responseBody);

                if (saveImage)
                {
                    var filePath = Path.Combine(saveDirectory, $"{fileId}.jpg");
                    await File.WriteAllBytesAsync(filePath, fileData);
                }

                return fileData;
            }
            catch (FormatException ex)
            {
                throw new ApplicationException("Ошибка преобразования данных изображения.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Неизвестная ошибка при получении изображения: {ex.Message}", ex);
            }
        }

        public string? GetFileId(string messageContent)
        {
            if (string.IsNullOrWhiteSpace(messageContent))
                throw new ArgumentException("Message content cannot be empty.", nameof(messageContent));

            var pattern = "<img\\s+src=\"(.*?)\"";
            var match = Regex.Match(messageContent, pattern);
            return match.Success ? match.Groups[1].Value : null;
        }

        private async Task ValidateToken()
        {
            if (tokenService.ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
                await CreateTokenAsync();
        }
    }
}
