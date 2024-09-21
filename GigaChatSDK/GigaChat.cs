using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;

using LD.Sber.GigaChatSDK.Models;
using LD.Sber.GigaChatSDK.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace LD.Sber.GigaChatSDK
{
    public class GigaChat : IGigaChat
    {
        private readonly bool saveImage;
        private static readonly string baseUrl = "https://gigachat.devices.sberbank.ru/api/v1/";

        private ITokenService tokenService { get; }
        private IHttpService httpService { get; }

        private string saveDirectory { get; set; } = Directory.GetCurrentDirectory();

        public GigaChat(ITokenService tokenService, IHttpService httpService, bool saveImage)
        {
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            this.saveImage = saveImage;
        }

        public async Task<Token> CreateTokenAsync()
        {
            return await tokenService.CreateTokenAsync();
        }

        public async Task<Response?> CompletionsAsync(MessageQuery query)
        {
            ValidateToken();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}chat/completions");
            request.Headers.Add("Authorization", $"Bearer {tokenService.Token.AccessToken}");
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var responseBody = await httpService.SendAsync(request);
            return JsonSerializer.Deserialize<Response>(responseBody);
        }

        public async Task<Response?> CompletionsAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            var query = new MessageQuery();
            query.messages.Add(new MessageContent("user", message));
            return await CompletionsAsync(query);
        }

        public async Task<EmbeddingResponse?> EmbeddingAsync(EmbeddingRequest request)
        {
            try
            {
                ValidateToken();

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

        public async Task<byte[]?> GetImageAsByteAsync(string fileId)
        {
            try
            {
                ValidateToken();

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

        private void ValidateToken()
        {
            if (tokenService.ExpiresAt < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
                CreateTokenAsync().Wait();
        }
    }
}
