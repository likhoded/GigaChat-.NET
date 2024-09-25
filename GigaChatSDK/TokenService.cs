using LD.Sber.GigaChatSDK.Interfaces;
using LD.Sber.GigaChatSDK.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LD.Sber.GigaChatSDK
{
    public class TokenService: ITokenService
    {
        private readonly IHttpService httpService;
        /// <summary>
        /// Авторизационные данные
        /// </summary>
        private readonly string secretKey;
        /// <summary>
        /// Версия API, к которой предоставляется доступ. Нужное значение параметра scope вы найдете в личном кабинете после создания проекта.
        /// GIGACHAT_API_PERS — доступ для физических лиц.
        /// GIGACHAT_API_CORP — доступ для юридических лиц.
        /// </summary>
        private readonly bool isCommercial;
        public long? ExpiresAt { get; private set; }
        public Token Token { get; private set; }

        public TokenService(IHttpService httpService, string secretKey, bool isCommercial)
        {
            this.httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            this.secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            this.isCommercial = isCommercial;
        }

        public async Task<Token> CreateTokenAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(secretKey))
                    throw new InvalidOperationException("Secret key is missing.");

               
                var request = new HttpRequestMessage(HttpMethod.Post, "https://ngw.devices.sberbank.ru:9443/api/v2/oauth");
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

                var responseBody = await httpService.SendAsync(request);
                Token = JsonSerializer.Deserialize<Token>(responseBody)
                        ?? throw new JsonException("Failed to deserialize token response.");

                ExpiresAt = Token.ExpiresAt;
                return Token;
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Ошибка десериализации ответа токена.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Неизвестная ошибка при создании токена: {ex.Message}", ex);
            }
        }

        public void SetToken(Token token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresAt = token.ExpiresAt;
        }
    }
}
