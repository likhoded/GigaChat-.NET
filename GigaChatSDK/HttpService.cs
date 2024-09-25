using LD.Sber.GigaChatSDK.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace LD.Sber.GigaChatSDK
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient client;
        /// <summary>
        /// true - включает игнорирование сертификатов безопасности
        /// Необходимо для систем имеющих проблемы с сертификатами МинЦифр.
        /// </summary>
        private readonly bool ignoreTLS;

        public HttpService(bool ignoreTLS)
        {
            this.ignoreTLS = ignoreTLS;
            client = new HttpClient(CreateHttpClientHandler(this.ignoreTLS));
        }
        /// <param name="ignoreTLS">true - включает игнорирование сертификатов безопасности.Необходимо для систем имеющих проблемы с сертификатами МинЦифр.</param>
        public HttpClientHandler CreateHttpClientHandler(bool ignoreTLS)
        {
            var handler = new HttpClientHandler();
            if (ignoreTLS)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            }
            return handler;
        }

        public async Task<string> SendAsync(HttpRequestMessage request)
        {
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); // Проверка успешности ответа
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Ошибка при отправке HTTP запроса: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Неизвестная ошибка: {ex.Message}", ex);
            }
        }
    }
}
